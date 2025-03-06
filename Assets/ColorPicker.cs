using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    [SerializeField] private SliderArea SatValSlider;
    [SerializeField] private Slider HueSlider;
    [SerializeField] private Image DisplayColor;
    [SerializeField] private Image SVDisplayColor;
    private Color _color;
    private IEnumerator coroutine;

    private bool _pipetteMode = false;
    public bool Pipette { get { return _pipetteMode; } }

    private static ColorPicker _instance;
    public static ColorPicker Instance { get { return _instance; } }
    public Color CurrentColor { get { return _color; } }

    public UnityEvent onValueChanged;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (onValueChanged == null)
            onValueChanged = new UnityEvent();

        UpdateColor();
    }


    void UpdateColor()
    {
        Vector2 satValValue;
        float hue;
        float saturation;
        float value;


        satValValue = SatValSlider.Value();
        hue = HueSlider.value;
        saturation = satValValue.x;
        value = satValValue.y;

        Color prevColor = _color;

        _color = Color.HSVToRGB(hue, saturation, value);

        Color currentColor = _color;

        bool valueChange = currentColor.r != prevColor.r || currentColor.g != prevColor.g || currentColor.b != prevColor.b;
        if (valueChange)
        {
            onValueChanged.Invoke();
            DisplayColor.color = _color;
            SVDisplayColor.color = Color.HSVToRGB(hue, 1, 1);
        }
    }

    public void PipetteModeToggle()
    {
        _pipetteMode = !_pipetteMode;
    }

    public void PipetteColor(Color color)
    {
        _pipetteMode = false;
        //_color = color;
        onValueChanged.Invoke();
        float hue;
        float saturation;
        float value;
        Color.RGBToHSV(color, out hue, out saturation, out value);
        SVDisplayColor.color = Color.HSVToRGB(hue, 1, 1);
        HueSlider.value = hue;
        SatValSlider.SetValue(saturation, value);
        _color = Color.HSVToRGB(hue, saturation, value);
        DisplayColor.color = _color;
    }


    void OnEnable()
    {
        SatValSlider.onValueChanged.AddListener(delegate { UpdateColor(); });
        HueSlider.onValueChanged.AddListener(delegate { UpdateColor(); });
    }


    void OnDisable()
    {
        SatValSlider.onValueChanged.RemoveAllListeners();
        HueSlider.onValueChanged.RemoveAllListeners();
    }
}
