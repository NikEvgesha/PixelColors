using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteItem : MonoBehaviour
{
    [SerializeField] private Image _img;
    private Color _color;
    public Color Color { 
        get 
        {
            return _color;
        }
        set 
        {
            _color = value;
            _img.color = value;
        }
    }

    public void OnClick()
    {
        ColorPicker.Instance.PipetteColor(_color);
    }

}