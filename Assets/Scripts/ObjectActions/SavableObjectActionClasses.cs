using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ObjectActionEvents;
public class SavableObjectActionClasses : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


namespace ObjectActionEvents.Savables
{
    #region SavableObjectActions
    [Serializable]
    public abstract class SavableObjectAction
    {
        public int targetID = -1;
        public string ActionTag = "";
        public string ActionName = "";
        public ObjectActionType ActionType = ObjectActionType.none;
        
    }

    [Serializable]
    public class SavableFloatAction : SavableObjectAction
    {
        public SetFloatActionType floatOperator = SetFloatActionType.equal;
        public float value = 1f;

        public SavableFloatAction(SetFloatAction action)
        {
            this.targetID = EditorController.Instance.MapDecor.IndexOf(action.TargetObject);
            this.value = action.value;
            this.floatOperator = action.floatOperator;
            this.ActionTag = action.ActionTag;
            this.ActionType = ObjectActionType.SetFloat;
            this.ActionName = action.actionName;
            
        }

    }

    [Serializable]
    public class SavableBoolAction : SavableObjectAction
    {
        public SetBoolActionType boolType = SetBoolActionType.Off;


        public SavableBoolAction(SetBoolAction action)
        {
            this.targetID = EditorController.Instance.MapDecor.IndexOf(action.TargetObject);
            this.ActionTag = action.ActionTag;
            this.ActionType = ObjectActionType.SetBool;
            this.ActionName = action.actionName;
            this.boolType = action.BoolActionType;
        }

    }

    [Serializable]
    public class ChangeLightSettingsActionSavable : SavableObjectAction
    {

    }

    #endregion
}