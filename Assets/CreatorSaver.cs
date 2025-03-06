using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class CreatorSaver : MonoBehaviour
{
    [SerializeField] private InputField _saveInput;
    [SerializeField] private InputField _loadInput;


    private List<string> _colors;
    private Dictionary<string, int> _palette_html;

    public Action<List<Color>> NewPalette;


    private static CreatorSaver _instance;
    public static CreatorSaver Instance { get { return _instance; } }

    private int colorByteLength = 9;


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
    public void SaveByte()
    {
        /*
            format:
            0: N - grid size
            1: M - palette size
            2...M+1: #FFFFFFFF - RGBA palette color
            M+2...M+2+N*N: c - index of color in palette
         */
        _colors = new();
        _palette_html = new();
        string filePath = Application.dataPath + "/Pixels/" + _saveInput.text;

        using (FileStream
            fileStream = new FileStream(filePath, FileMode.Create))
        {
            byte size = (byte)GridField.Instance.GetSize();
            fileStream.WriteByte(size);

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
            fileStream.WriteByte((byte)_palette_html.Count);
            byte i = 0;
            foreach (string colorHtml in _palette_html.Keys.ToList())
            {
                byte[] bytes = Encoding.ASCII.GetBytes("#" + colorHtml);
                Debug.Log("Color length: " + bytes.Length);
                fileStream.Write(bytes);
                _palette_html[colorHtml] = i++;
            }

            foreach (string color in _colors)
            {
                fileStream.WriteByte((byte)_palette_html[color]);
            }

        }
    }



    public void LoadByte()
    {
        string filePath = Application.dataPath + "/Pixels/" + _loadInput.text;
        Debug.Log("loading file " + filePath);
        try
        {
            string line;
            Color color;
            byte[] colBuffer = new byte[9];
            int colorIdx;
            using (FileStream
            fileStream = new FileStream(filePath, FileMode.Open))
            {
                int read = 0;
                int gridSize = fileStream.ReadByte();
                int paletteSize = fileStream.ReadByte();

                Debug.Log("grid size: " + gridSize);
                Debug.Log("palette size: " + paletteSize);

                List<Color> palette = new();

                GridField.Instance.CreateGrid(gridSize);

                for (int i = 0; i < paletteSize; i++)
                {
                    fileStream.Read(colBuffer, read, colorByteLength);
                    string colString = Encoding.ASCII.GetString(colBuffer);
                    if (ColorUtility.TryParseHtmlString(colString, out color))
                    {
                        palette.Add(color);
                    }
                }

                NewPalette?.Invoke(palette);

                int idx = 0;
                while ((colorIdx = fileStream.ReadByte()) != -1)
                {
                    GridField.Instance.SetCellColor(idx / gridSize, idx % gridSize, palette[colorIdx]);
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




/*    public void Save()
    {
        *//*
            format:
            0: N - grid size
            1: M - palette size
            2...M+1: #FFFFFFFF - RGBA palette color
            M+2...M+2+N*N: c - index of color in palette
         *//*
        _colors = new();
        _palette_html = new();
        string filePath = Application.dataPath + "/Pixels/" + _saveInput.text;
        StreamWriter sw;
        FileInfo file = new FileInfo(filePath);
        if (file.Exists)
        {
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
        byte i = 0;
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
    }*/


}
