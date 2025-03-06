using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Palette : MonoBehaviour
{
    [SerializeField] private PaletteItem _paletteItemPrefab;

    private List<Color> _palette = new();
    private List<PaletteItem> _items = new();

    private void Start()
    {
        CreatorSaver.Instance.NewPalette += UpdatePalette;
    }

    private void OnDisable()
    {
        CreatorSaver.Instance.NewPalette -= UpdatePalette;
    }


    private void UpdatePalette(List<Color> colors)
    {
        if (_items.Count > 0)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                Destroy(_items[i].gameObject);
            }
        }
        _items = new();
        foreach (Color color in colors) {
            PaletteItem item = Instantiate(_paletteItemPrefab, transform);
            _items.Add(item);
            item.Color = color;
        }

    }
}
