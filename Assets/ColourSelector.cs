using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class ColourSelector : EditorMenuAbstract
{
    public static ColourSelector instance;
    public Color TargetColour;
    

    [SerializeField] Button[] ColourPreset;
    [SerializeField] Slider _hueSlider;
    [SerializeField] Slider _saturationSlider;
    [SerializeField] Slider _brightnessSlider;

    [SerializeField] GameObject _sliderContainer;

     public Color tempColour { get; private set; }
    private int SelectedPreset = -1;
    /// <summary>
    /// called each time the ui is altered. menus using colour picker should assign methods that will return tempColour for their own
    /// update needs.
    /// </summary>
     public UnityEvent updateEvent = new UnityEvent();
     void Awake()
    {
        instance = this;
    }

    public override void OpenMenu()
    {
        SetTarget(-1);
        base.OpenMenu();
       // SelectPresetColour(0);
    }

    public override void CloseMenu()
    {
        base.CloseMenu();
        updateEvent.RemoveAllListeners();       
    }

    public void SetTarget(int id)
    {
        SelectedPreset = id;

        if(id == -1)
        {
            _sliderContainer.SetActive(false);       
        }
        else
        {
            _sliderContainer.SetActive(true);
        }

    }

    public void SelectPresetColour(Transform target)
    {
      int id = target.GetSiblingIndex();
       SetTarget(id);
        tempColour = ColourPreset[id].image.color;
        float H;
        float S;
        float V;
        Color.RGBToHSV(tempColour, out H, out S, out V);

        _hueSlider.SetValueWithoutNotify(H);
        _saturationSlider.SetValueWithoutNotify(S);
        _brightnessSlider.SetValueWithoutNotify(V);
        updateEvent.Invoke();
    }

    public void SelectPresetColour(int id)
    {
        tempColour = ColourPreset[id].image.color;
        float H;
        float S;
        float V;
        Color.RGBToHSV(tempColour, out H, out S, out V);

        _hueSlider.SetValueWithoutNotify(H);
        _saturationSlider.SetValueWithoutNotify(S);
        _brightnessSlider.SetValueWithoutNotify(V);
        updateEvent.Invoke();
    }


    /// <summary>
    /// called by every slider when used to ensure all current values are used.
    /// </summary>
    public void GetSliderValues()
    {
       tempColour = Color.HSVToRGB(_hueSlider.value, _saturationSlider.value, _brightnessSlider.value);
        ColourPreset[SelectedPreset].image.color = tempColour;
        string hex = ColorUtility.ToHtmlStringRGBA(tempColour);
        updateEvent.Invoke();
    }
    

}
