using System;
using Unity.VisualScripting;
using UnityEngine;

public class GridField : MonoBehaviour
{
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private GridLayout _layout;
    [SerializeField] private int _borderWidth;

    private RectTransform _rect;
    private float _width;
    private float _height;
    private float _cellSize;
    private int _size;
    private Color _defaultColor = Color.white;

    private Cell[,] grid;

    private static GridField _instance;
    public static GridField Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }

    

    public void CreateGrid(int size)
    {
        ClearGrid();
        _size = size;
        grid = new Cell[size, size];
        _cellSize = ((_width - _borderWidth) / size) - _borderWidth;
        float startX = _borderWidth;
        float startY = -_borderWidth;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector3 position = new Vector3(
                    startX + (x * (_cellSize + _borderWidth)) ,
                    startY - (y * (_cellSize + _borderWidth)) ,
                    0f
                );

                Cell cell = Instantiate(_cellPrefab, transform);
                cell.SetPosition(position);
                cell.SetSize(new Vector2(_cellSize, _cellSize));
                cell.SetColor(_defaultColor);
                cell.name = $"Cell_{x}_{y}";

                grid[x, y] = cell;
            }
        }
    }

    public void ClearGrid()
    {
        if (grid != null)
        {
            foreach (Cell cell in grid)
            {
                if (cell != null)
                {
                    Destroy(cell.gameObject);
                }
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
        {
            return grid[x, y];
        }
        return null;
    }

    public void SetCellColor(int x, int y, Color color)
    {
        grid[x, y].SetColor(color);
    }

    // Clean up when object is destroyed
    private void OnDestroy()
    {
        ClearGrid();
    }

    public int GetSize()
    {
        return _size;
    }

    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        _width = _rect.rect.width;
        _height = _rect.rect.height;
    }

}
