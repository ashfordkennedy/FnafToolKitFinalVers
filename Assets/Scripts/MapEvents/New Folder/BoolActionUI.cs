using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ObjectActionEvents;
using TMPro;
public class BoolActionUI : BaseObjectActionUI
{
    [SerializeField] SetBoolActionType operatorType = SetBoolActionType.Off;
    [SerializeField] Toggle[] toggles;
    [SerializeField] ToggleGroup operators;



    /// <summary>
    /// Updates the target action class to reflect UI values
    /// </summary>
    public override void UpdateActionClass()
    {
        SetBoolAction target = targetAction as SetBoolAction;

        target.ActionTag = ActionTag;
        target.boolActionType = operatorType;

    }






    public override void RestoreActionUi(ObjectAction newTarget)
    {
        var target = (SetBoolAction)newTarget;
        targetAction = target;

        ActionTag = target.ActionTag;
        operatorType = target.boolActionType;

        //restore Display
        ActionNameDisplay.text = target.actionName;
        operators.SetAllTogglesOff(false);
        toggles[(int)operatorType].Select();
    }

    public void SetOperator()
    {
        operatorType = (SetBoolActionType)operators.GetFirstActiveToggle().transform.GetSiblingIndex();
    }


    private void SetToggleSet()
    {

        switch (ActionTag)
        {
            default:
                toggles[0].GetComponentInChildren<TMP_Text>().text = "On";
                toggles[0].GetComponentInChildren<TMP_Text>().text = "Off";
                toggles[0].GetComponentInChildren<TMP_Text>().text = "Switch";
                break;



        }
        


    }
}
