using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.HighDefinition;
public class LightSettingUI : EditorMenuAbstract
{
    public static LightSettingUI Instance;
    [SerializeField] GameObject LightSettingCanvas;
    [SerializeField] Transform Content;
    public DecorLighting TargetLight = null;
    private int selectedLight = 0;

    public IndexHandler LightIndex = new IndexHandler();

    [SerializeField] TMPro.TextMeshProUGUI SelectedLightName;


    [SerializeField] TMPro.TMP_InputField powerInput;
    [SerializeField] Toggle activeOnStartToggle;
    [SerializeField] Toggle enabledToggle;


    float outerAngle;

    [Header("Sliders")]
    [SerializeField] Ext_Slider m_intensity;
    [SerializeField] Ext_Slider m_volume;
    [SerializeField] Ext_Slider m_range;
    [SerializeField] Ext_Slider m_outerRange;
    [SerializeField] Ext_Slider m_innerRange;
    [SerializeField] Ext_Slider m_radius;


    public void NextLightIndex() { 
        LightIndex.NextIndex();
        MenuSetup();
        ColourSelector.instance.CloseMenu();
    }

    public void PreviousLightIndex()
    {
        LightIndex.PreviousIndex();
        MenuSetup();
        ColourSelector.instance.CloseMenu();
    }

    //method replacement
    public float targetIntensity { get => TargetLight._intensity; set => TargetLight._intensity = value; }
    public float targetVolume { get => TargetLight._volume; set => TargetLight._volume = value; }
    public float targetRange { get => TargetLight._range; set => TargetLight._range = value; }

    public float targetInnerAngle { get => TargetLight._InnerAngle; set => TargetLight._InnerAngle = value; }
    public float targetOuterAngle { get => TargetLight._OuterAngle; set => TargetLight._OuterAngle = value; }
    public float targetRadius { get => TargetLight._radius; set => TargetLight._radius = value; }
    


    /// <summary>
    /// light default values
    /// </summary>
    public float DefaultIntensity { get => TargetLight.defaultIntensity; set => TargetLight.defaultIntensity = value; }
    public float DefaultVolume { get => TargetLight.defaultVolume; set => TargetLight.defaultVolume = value; }
    public float DefaultRange { get => TargetLight.defaultRange; set => TargetLight.defaultRange = value; }

    public Color DefaultColor { get => TargetLight.defaultColor; set => TargetLight.defaultColor = value; }

    public float DefaultInnerAngle { get => TargetLight.defaultinnerAngle; set => TargetLight.defaultinnerAngle = value; }
    public float DefaultOuterAngle { get => TargetLight.defaultOuterAngle; set => TargetLight.defaultOuterAngle = value; }
    public float DefaultRadius { get => TargetLight.defaultRadius; set => TargetLight.defaultRadius = value; }



    // Start is called before the first frame update
    private void Awake()
    {
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
                LightIndex.SetMaxIndexValue(TargetLight.LightCount - 1);
                print("this is the fucking total lights " + TargetLight.LightCount);
                LightIndex.SetcurrentIndex(0);

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
        DecorLighting.l = LightIndex.index;
        print("index is now set to " + LightIndex.index);
        SelectedLightName.text = TargetLight.GetSubLightName(LightIndex.index);
        m_intensity.SetValueWithoutNotify(TargetLight._intensity);

        m_volume.SetValueWithoutNotify(TargetLight._volume);

        m_range.SetValueWithoutNotify(TargetLight._range);

        
        Debug.LogWarning("The power value code needs to be removed and properly integrated into the power menu");
        /*
        powerInput.SetTextWithoutNotify("" + TargetLight.powerSettings._powerDrain);
        activeOnStartToggle.SetIsOnWithoutNotify(TargetLight.powerSettings._activeOnStart);
        enabledToggle.SetIsOnWithoutNotify(TargetLight.powerSettings._active);
        */
    }
 

    public void OpenColorPicker()
    {
        ColourSelector.instance.CloseMenu();
        ColourSelector.instance.updateEvent.AddListener(RecieveColorSelectorColour);
        ColourSelector.instance.OpenMenu();
    }

    public void RecieveColorSelectorColour()
    {
        TargetLight._color = ColourSelector.instance.tempColour;
        TargetLight.defaultColor = ColourSelector.instance.tempColour;
    }


    /// <summary>
    /// Menu function to enable/disable the light
    /// </summary>
    /// <param name="toggle"></param>
    public void ToggleLight(Toggle toggle)
    {
        // needs multilight support
        TargetLight.LightToggle(toggle.isOn);
    }



    public void SetTargetLight(DecorLighting targetObject)
    {
        TargetLight = targetObject;

        m_intensity.value = TargetLight._intensity;
       m_volume.value = TargetLight._volume;
        m_range.value = TargetLight._range;



       // activeOnStartToggle.isOn = TargetLight._activeOnStart;
       // enabledToggle.isOn = TargetLight._active;
       // powerInput.text = "" + TargetLight._powerDrain;


        //  this.TargetLight = targetObject.GetComponentInChildren<Light>();
        // this.TargetLightHD = targetObject.GetComponentInChildren<HDAdditionalLightData>();
    }
}
