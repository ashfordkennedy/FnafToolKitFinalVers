using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicControllerMenu : EditorMenuAbstract
{

    public static ClassicControllerMenu instance;

    public DecorClassicStart target;

    [SerializeField] Ext_Slider upperBoundSlider;
    [SerializeField] Ext_Slider lowerBoundSlider;

    private void Awake()
    {
        instance = this;
    }



    public void SetTarget(DecorClassicStart target)
    {
        this.target = target;
    }

    public override void OpenMenu()
    {
        SetMenuValues();
        base.OpenMenu();

    }


    public void SetUpperBound(Ext_Slider slider)
    {
        target.upperBound = slider.value;
    }

    public void SetLowerBound(Ext_Slider slider)
    {
        target.lowerBound = slider.value;
    }

    public void SetMenuValues()
    {
        upperBoundSlider.SetValueWithoutNotify(target.upperBound);
        lowerBoundSlider.SetValueWithoutNotify(target.lowerBound);

    }

    public void PreviewCharacter()
    {
        target.StartPreview();
    }

}
