using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
public class NightSettingsPanel : EditorMenuAbstract
{
    public static NightSettingsPanel instance;
    private int _targetNight = 0;
    public int targetNight { get { return _targetNight; } set { _targetNight = value; nightSettings = NightManager.instance.nightSettings[value]; } }
    NightSettings nightSettings = null;


    /// <summary>
    /// properties! use properties! no methods for updating 
    /// </summary>
    public int startHour { get => nightSettings.startHour; set { nightSettings.startHour = value; } }
    public int endHour { get => nightSettings.endHour; set { nightSettings.endHour = value; } }
    public float basePowerLoss { get => nightSettings.basePowerLoss; set => nightSettings.basePowerLoss = value; }


    [SerializeField] private VolumeProfile sceneVolume;
    [SerializeField] private Slider _lightIntensitySlider;


    [SerializeField]private TMP_Text _targetNightText;

    


    // sadly still needed for putting values on fields
    [SerializeField]private TMP_InputField _startHourInput;
    [SerializeField]private TMP_InputField _endHourInput;
    [SerializeField] private TMP_InputField _hourLengthInput;

    [SerializeField]private TMP_InputField _totalPowerInput;
    [SerializeField]private TMP_InputField _powerLossInput;
    

    void Awake()
    {
        instance = this;
        /*
        // change the map settings automatically
        _startHourInput.onEndEdit.AddListener(delegate { UpdateNightSetting(NightSetting.starthour, _startHourInput); });

        _endHourInput.onEndEdit.AddListener(delegate { UpdateNightSetting(NightSetting.endhour, _endHourInput); });
        */
        /*
        _totalPowerInput.onEndEdit.AddListener(delegate { UpdateNightSetting(NightSetting.totalpower, _totalPowerInput); });

        _powerLossInput.onEndEdit.AddListener(delegate { UpdateNightSetting(NightSetting.basepowerloss, _powerLossInput); });
        */
        targetNight = 0;
        UpdateDisplay();
        
       
    }





    /// <summary>
    /// sets the target night and auto updates the menu. Night settings reference is set automatically by the get set methods of _target night
    /// </summary>
    /// <param name="SwitchTotal"></param>
    public void SetTargetNight(int SwitchTotal)
    {
        targetNight += SwitchTotal;

        if (targetNight > 6)
        {
            targetNight = 0;
        }
    if (targetNight < 0)
        {
            targetNight = 6;
        }


        _targetNightText.text = "Night " + (_targetNight +1);
        UpdateDisplay();
    }


    public enum NightSetting { starthour, endhour, totalpower, basepowerloss }
    public void UpdateNightSetting(NightSetting setting, TMP_InputField inputField)
    {


        if (int.Parse(inputField.text) > 0)
        {

            switch (setting)
            {

                case NightSetting.starthour:
                    nightSettings.startHour = int.Parse(_startHourInput.text);
                    break;

                case NightSetting.endhour:
                    nightSettings.endHour = int.Parse(_endHourInput.text);
                    break;

                case NightSetting.totalpower:
                    nightSettings.totalPower = float.Parse(_totalPowerInput.text);
                    break;


                case NightSetting.basepowerloss:
                    nightSettings.basePowerLoss = float.Parse(_powerLossInput.text);
                    break;

            }
        }
        else
        {
            UpdateDisplay();
        }




    }


    public void SetHourLength(TMP_InputField inputField)
    {

        var newValue = InputFieldClamper(inputField, 30f);
        nightSettings.hourLength = newValue;
    }

    public void SetEndHour(TMP_InputField inputField)
    {

        var newValue = InputFieldClamper(inputField, 6f);
        nightSettings.endHour = (int)newValue;
    }

    public void SetStartHour(TMP_InputField inputField)
    {

        var newValue = InputFieldClamper(inputField, 12);
        nightSettings.startHour = (int)newValue;
    }

    public void SetTotalPowerLoss(TMP_InputField inputField)
    {

        var newValue = InputFieldClamper(inputField,0.05f);
        nightSettings.basePowerLoss = newValue;
    }


    public void SetTotalPower(TMP_InputField inputField)
    {

        var newValue = InputFieldClamper(inputField,5);
        nightSettings.totalPower = newValue;
    }

    public float InputFieldClamper(TMP_InputField inputField, float defaultValue = 100)
    {
        float value;
        if (float.TryParse( inputField.text, out value) && value >-0.1f)
        {

            return value;
            
        }

        else
        {
            return defaultValue;
        }


    }

    public void SetFogColor(Color colour)
    {
        nightSettings.fogColor = new SavableColour(colour);

       if (sceneVolume.TryGet<Fog>(out var fog)){
            fog.color.value = colour;
        }

        if (sceneVolume.TryGet<GradientSky>(out var sky))
        {
            sky.top.value = colour;
            sky.middle.value = colour;
            sky.bottom.value = colour;
        }
    }


    public void SetFogExposure(Slider slider)
    {
        if (sceneVolume.TryGet<Exposure>(out var light))
        {
            light.fixedExposure.value = slider.value;
            nightSettings.fogIntensity = slider.value;
        }
    }


    public void UpdateDisplay()
    {
        Debug.Log("loading night data");

        _startHourInput.text = "" + nightSettings.startHour;
        _endHourInput.text = "" + nightSettings.endHour;
        _hourLengthInput.text = "" + nightSettings.hourLength;

        _totalPowerInput.text = "" + nightSettings.totalPower;
        _powerLossInput.text = "" + nightSettings.basePowerLoss;
        _lightIntensitySlider.value = nightSettings.fogIntensity;


}


}
