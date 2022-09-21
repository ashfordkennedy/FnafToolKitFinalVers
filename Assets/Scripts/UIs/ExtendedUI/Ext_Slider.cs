using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Ext_Slider : Slider
{
    [SerializeField] TMP_Text valueDisplay = null;
    
    [SerializeField] public bool useAdditions;
    [SerializeField] public string valuePrefix = "";
    [SerializeField] public string valueSuffix = "";

    protected override void Set(float input, bool sendCallback = true)
    {
        base.Set(input, sendCallback);
        UpdateDisplayValue();       
    }

    public override void SetValueWithoutNotify(float input)
    {
        base.SetValueWithoutNotify(input);
        UpdateDisplayValue();
    }


    protected void UpdateDisplayValue()
    {
        float correctedValue = value;

        if (useAdditions)
        {
            valueDisplay.text = valuePrefix + correctedValue + valueSuffix;
        }
        else
        {
            valueDisplay.text = "" + correctedValue;
        }

    }

}
