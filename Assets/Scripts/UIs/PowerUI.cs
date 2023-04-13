using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PowerUI : EditorMenuAbstract
{
    private DecorExt_Power target;
    [SerializeField] TMP_Text StatText;

    [SerializeField] int targetNight;
    public void OpenMenu(DecorExt_Power newTarget){

        target = newTarget;

        base.OpenMenu();
        }


    public override void CloseMenu()
    {
        target = null;
        base.CloseMenu();
    }






    public void UpdatePower(TMPro.TMP_InputField iField)
    {
        float value;
        if (target != null && float.TryParse(iField.text, out value) == true)
        {
            Mathf.Clamp(value, -100, 100);
            iField.text = "" + value;
            target.SetPowerValue(value);
        }       
    }

    public void UpdateStartOn(Toggle toggle)
    {

        target.SetOnStartActive(toggle.isOn);
    }


    private void GenerateStats()
    {
       var night = NightManager.instance.nightSettings[targetNight];

       

        StatText.text = "";

    }


}
