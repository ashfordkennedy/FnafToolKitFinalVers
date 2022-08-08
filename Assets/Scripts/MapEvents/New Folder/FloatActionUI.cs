using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ObjectActionEvents;
public class FloatActionUI : BaseObjectActionUI
{

    [SerializeField] Toggle[] toggles;
    [SerializeField] ToggleGroup operators;
    [SerializeField] SetFloatActionType operatorType = SetFloatActionType.equal;
    [SerializeField] TMPro.TMP_InputField valueInput;

    public void SetOperator()
    {
       operatorType = (SetFloatActionType)operators.GetFirstActiveToggle().transform.GetSiblingIndex();
    }



    public void Awake()
    {
      //  targetAction = new SetFloatAction();
      //  ActionTag = "DebugsetFloat";
    }

     
    /// <summary>
    /// Updates the target action class to reflect UI values
    /// </summary>
    public override void UpdateActionClass()
    {
        SetFloatAction target = targetAction as SetFloatAction;

        target.ActionTag = ActionTag;
        target.floatOperator = operatorType;

        float result;
        if(float.TryParse(valueInput.text, out result)){
            target.value = result;
        }
        else
        {
            target.value = 0f;
        }

        print(ActionTag + " " + operatorType + " " + result);
    }

    public override void RestoreActionUi(ObjectAction newTarget)
    {
        var target = (SetFloatAction)newTarget;
        targetAction = target;
        ActionTag = target.ActionTag;
        operatorType = target.floatOperator;
        valueInput.text = "" + target.value;

        ActionNameDisplay.text = target.actionName;

        toggles[(int)operatorType].Select();
    }



}
