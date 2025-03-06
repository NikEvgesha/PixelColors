using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CreatorSaver : MonoBehaviour
{
    [SerializeField] private InputField _saveInput;
    [SerializeField] private InputField _loadInput;


    private List<string> _colors;
    private Dictionary<string, int> _palette_html;

    public Action<List<Color>> NewPalette;


    private static CreatorSaver _instance;
    public static CreatorSaver Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        /*
            Format:
            0: N - grid size
            1...N*N: #FFFFFFFF - RGBA color
         */

        /*
            TODO format:
            0: N - grid size
            1: M - palette size
            2...M+1: #FFFFFFFF - RGBA palette color
            M+2...M+2+N*N: c - index of color in palette
         */
        _colors = new();
        _palette_html = new();
        string filePath = Application.dataPath + "/Pixels/" + _saveInput.text;
        StreamWriter sw;
        FileInfo file = new FileInfo(filePath);
        if (file.Exists) {
            file.Delete();
        }
        sw = file.AppendText();

        int size = GridField.Instance.GetSize();
        sw.WriteLine(size);

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Cell cell = GridField.Instance.GetCell(x, y);
                string htmlColor = ColorUtility.ToHtmlStringRGBA(cell.Color);
                _colors.Add(htmlColor);
                if (!_palette_html.ContainsKey(htmlColor))
                {
                    _palette_html.Add(htmlColor, 0);
                }
            }
        }
        sw.WriteLine(_palette_html.Count);
        int i = 0;
        foreach (string colorHtml in _palette_html.Keys.ToList())
        {
            sw.WriteLine("#" + colorHtml);
            _palette_html[colorHtml] = i++;
        }

        foreach (string color in _colors)
        {
            sw.WriteLine(_palette_html[color]);
        }

        sw.Close();
        sw.Dispose();
    }


    public void Load()
    {
        string filePath = Application.dataPath + "/Pixels/" + _loadInput.text;
        Debug.Log("loading file " + filePath);
        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                Color color;
                int gridSize = Int32.Parse(sr.ReadLine());
                int paletteSize = Int32.Parse(sr.ReadLine());
                List<Color> palette = new();

                GridField.Instance.CreateGrid(gridSize);

                for (int i = 0; i < paletteSize; i++)
                {
                    line = sr.ReadLine();
                    if (ColorUtility.TryParseHtmlString(line, out color))
                    {
                        palette.Add(color);
                    }
                }

                NewPalette?.Invoke(palette);

                int idx = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    GridField.Instance.SetCellColor(idx / gridSize, idx % gridSize, palette[Int32.Parse(line)]);
                    idx++;
                }

                for (int i = 0; i < gridSize; i++)
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        Debug.Log(GridField.Instance.GetCell(i, j).Color);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
        }
        _saveInput.text = _loadInput.text;
    }
}
