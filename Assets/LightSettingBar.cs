using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightSettingBar : MonoBehaviour
{
    public enum settingMode{intensity, volume, range }
    [SerializeField] public settingMode _mode = settingMode.intensity;
   // [SerializeField] private InputField _inputField;
   // [SerializeField] private Slider _slider;


   public void UpdateSlider(Slider slider)
    {
   
        switch (_mode)
        {
            case settingMode.intensity:
              //  LightSettingUI.Instance.UpdateIntensity(slider);
                break;

            case settingMode.range:
             //   LightSettingUI.Instance.UpdateRange(slider);
                break;

            case settingMode.volume:
             //   LightSettingUI.Instance.UpdateVolume(slider);
                break;
        }

    }

    public void UpdateSlider(InputField inputField)
    {

        switch (_mode)
        {
            case settingMode.intensity:
                LightSettingUI.Instance.UpdateIntensity(inputField);
                break;

            case settingMode.range:
                LightSettingUI.Instance.UpdateRange(inputField);
                break;

            case settingMode.volume:
                LightSettingUI.Instance.UpdateVolume(inputField);
                break;
        }

    }





}

