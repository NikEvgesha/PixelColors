using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{ 
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Image _img;
    private Color _color;

    public Color Color { get { return _color; } }

    public void SetSize(Vector2 size)
    {
        _rect.sizeDelta = size;
    }

    public void SetPosition(Vector2 pos)
    {
        _rect.anchoredPosition = pos;
    }


    public void SetColor()
    {
        _color = ColorPicker.Instance.CurrentColor;
        _img.color = _color;
    }

    public void OnMouseDown()
    {
        if (!ColorPicker.Instance.Pipette)
        {
            SetColor();
        } else
        {
            ColorPicker.Instance.PipetteColor(_color);
        }
    }

    public void SetColor(Color color)
    {
        _color = color;
        _img.color = _color;
    }

    public void CheckDragColor()
    {
        if (!ColorPicker.Instance.Pipette && Input.GetMouseButton(0))
        {
            SetColor();
        }
    }

}
