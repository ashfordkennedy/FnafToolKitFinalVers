using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ObjectActionEvents;
using TMPro;

public class ColorActionUi : BaseObjectActionUI
{

    [SerializeField] SetColorActionType colorActionType = SetColorActionType.Gradual;
    [SerializeField] Toggle[] toggles;
    [SerializeField] ToggleGroup operators;
    [SerializeField] Color color;
    [SerializeField] TMP_InputField transitionField;
    [SerializeField] Button ColorButton;


    /// <summary>
    /// Updates the target action class to reflect UI values
    /// </summary>
    public override void UpdateActionClass()
    {
        SetColorAction target = targetAction as SetColorAction;

        target.ActionTag = ActionTag;
        target.colorActionType = colorActionType;
        target.color = color;

        float.TryParse(transitionField.text, out float transitionTime);
        target.transitionTime = transitionTime;
        target.color = ColorButton.targetGraphic.color;

    }

    public override void RestoreActionUi(ObjectAction newTarget)
    {
        var target = (SetColorAction)newTarget;
        targetAction = target;

        ActionTag = target.ActionTag;
        colorActionType = target.colorActionType;

        //restore Display
        ActionNameDisplay.text = target.actionName;
        operators.SetAllTogglesOff(false);
        toggles[(int)colorActionType].Select();
    }

    public void SetColor()
    {
        ColourSelector.instance.CloseMenu();
        ColourSelector.instance.updateEvent.AddListener(() => RecieveColor());
        ColourSelector.instance.OpenMenu();

    }

    public void RecieveColor()
    {
      Color color =  ColourSelector.instance.TargetColour;
      ColorButton.targetGraphic.color = color;
        UpdateActionClass();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
