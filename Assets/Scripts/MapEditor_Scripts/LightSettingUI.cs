using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.HighDefinition;
public class LightSettingUI : EditorMenuAbstract
{
    public static LightSettingUI Instance;
    [SerializeField] GameObject LightSwatchPrefab;
    [SerializeField] GameObject LightSettingCanvas;
    [SerializeField] Transform Content;
    public Color[] LightColourPresets;
    public DecorLighting TargetLight = null;
    private int selectedLight = 0;

    [SerializeField] TMPro.TMP_InputField powerInput;
    [SerializeField] Toggle activeOnStartToggle;
    [SerializeField] Toggle enabledToggle;

    [Header("Sliders")]
    [SerializeField] Ext_Slider m_intensity;
    [SerializeField] Ext_Slider m_volume;
    [SerializeField] Ext_Slider m_range;
    [SerializeField] Ext_Slider m_outerRange;
    [SerializeField] Ext_Slider m_innerRange;
    [SerializeField] Ext_Slider m_radius;
    // Start is called before the first frame update
    private void Awake()
    {
        //Light swatches being phased out due to color menu
        //GenerateLightSwatches();
        Instance = this;
    }




    /// <summary>
    /// Opens The lightUI. Set the target light  before opening.
    /// </summary>
    /// <param name="opening"></param>
    public void ToggleLightUI(bool opening, DecorLighting target = null)
    {
        switch (opening)
        {
            case true:
                TargetLight = target;
                MenuSetup();
                OpenMenu();
                break;

            case false:
                CloseMenu();
                break;
        }
       
    }

    private void MenuSetup()
    {
        

        m_intensity.SetValueWithoutNotify(TargetLight._intensity);

        m_volume.SetValueWithoutNotify(TargetLight._volume);

        m_range.SetValueWithoutNotify(TargetLight._range);

       //
        
        powerInput.SetTextWithoutNotify("" + TargetLight._powerDrain);
        activeOnStartToggle.SetIsOnWithoutNotify(TargetLight._activeOnStart);
        enabledToggle.SetIsOnWithoutNotify(TargetLight._active);
    }
   



    /// <summary>
    /// Used To set the target lights colour to a given preset.
    /// </summary>
    /// <param name="ColourPresetID">The UI objects sibling index to access correct preset in LightColourPresets array</param>
    public void SetLightColour(GameObject ColourToggle){
        if (TargetLight != null) {
            AudioManager.Audio_M.PlayUIClick();
            TargetLight.SetColour(LightColourPresets[ColourToggle.transform.GetSiblingIndex()]);
        }
        }

    public void OpenColorPicker()
    {
        ColourSelector.instance.CloseMenu();
        ColourSelector.instance.updateEvent.AddListener(RecieveColorSelectorColour);
        ColourSelector.instance.OpenMenu();
    }

    public void RecieveColorSelectorColour()
    {
        TargetLight.SetColour(ColourSelector.instance.tempColour);
    }


    /// <summary>
    /// Menu function to enable/diable the light
    /// </summary>
    /// <param name="toggle"></param>
    public void ToggleLight(Toggle toggle)
    {
        TargetLight.LightToggle(toggle.isOn);
    }

    public void SetLightStartActive(Toggle toggle)
    {
        TargetLight.SetOnStartActive(toggle.isOn);
    }



    public void UpdateIntensity(InputField InputField)
    {
        float value;
        if (TargetLight != null && float.TryParse( InputField.text, out value) == true)
        {
            Mathf.Clamp(value,0, 20);
            InputField.text = "" + value;

            TargetLight.SetIntensity(value);
        }

    }

    public void UpdateIntensity(Ext_Slider slider)
    {
        if (TargetLight != null)
        {
            TargetLight.SetIntensity(slider.value);           
        }

    }





    public void UpdateVolume(InputField InputField)
    {
        float value;
        if (TargetLight != null && float.TryParse(InputField.text, out value) == true)
        {
            TargetLight.SetVolume(value);
        }
    }

    public void UpdateVolume(Ext_Slider slider)
    {
        if (TargetLight != null)
        {
            TargetLight.SetVolume(slider.value);           
        }

    }

    public void UpdatePower(TMPro.TMP_InputField iField)
    {
        float value;
        if (TargetLight != null && float.TryParse(iField.text, out value) == true)
        {
            Mathf.Clamp(value, -10, 10);
            iField.text = "" + value;
            TargetLight.SetPowerValue(value);
        }

    }


    public void UpdateRange(Ext_Slider slider)
    {
        if (TargetLight != null)
        {
            TargetLight.SetRange(slider.value);
        }
    }

    public void UpdateRange(InputField iField)
    {
        float value;
        if (TargetLight != null && float.TryParse(iField.text, out value) == true)
        {
            Mathf.Clamp(value, 0, 20);
            iField.text = "" + value;
            TargetLight.SetRange(value);

        }
    }















    public void SetTargetLight(DecorLighting targetObject)
    {
        TargetLight = targetObject;

        m_intensity.value = TargetLight._intensity;
       m_volume.value = TargetLight._volume;
        m_range.value = TargetLight._range;
        activeOnStartToggle.isOn = TargetLight._activeOnStart;
        enabledToggle.isOn = TargetLight._active;
        powerInput.text = "" + TargetLight._powerDrain;


        //  this.TargetLight = targetObject.GetComponentInChildren<Light>();
        // this.TargetLightHD = targetObject.GetComponentInChildren<HDAdditionalLightData>();
    }

    public void GenerateLightSwatches()
    {
        for (int i = 0; i < LightColourPresets.Length; i++)
        {
            Toggle NewSwatch = Instantiate(LightSwatchPrefab, Content).GetComponent<Toggle>();
            NewSwatch.image.color = LightColourPresets[i];
            NewSwatch.gameObject.SetActive(true);
        }
    }
}
