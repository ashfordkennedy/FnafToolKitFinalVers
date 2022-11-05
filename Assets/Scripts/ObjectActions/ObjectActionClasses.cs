using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using ObjectActionEvents.Savables;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ObjectActionClasses : MonoBehaviour
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


namespace ObjectActionEvents
{
    #region ObjectActions


    public enum ObjectActionType { none, SetFloat, SetBool,SetColor, SetLight }

    /// <summary>
    /// An object action is a task performed as part of an event in-game. this base class is useless, with its children containing their own
    /// targets, values and functionality.
    /// </summary>
    [Serializable]
    public class ObjectAction
    {
        /// <summary>
        /// Action tags are used by the actionManager to target the correct event
        /// </summary>
        public string ActionTag = "";
        public string actionName = "";
        public DecorObject TargetObject;
        /// <summary>
        /// Enum describing the action type for easy parsing of data
        /// </summary>
        public ObjectActionType ObjectActionType = ObjectActionType.none;
        // each class must contain a conversion constructor between its in-engine and savable equivalent

        /// <summary>
        /// Base method for generating the correct unity event for the given object action.
        /// </summary>
        /// <param name="TargetEvent"></param>
        public virtual void GenerateEventMethod(UnityEvent TargetEvent)
        {

        }

       
        public virtual SavableObjectAction ConvertToSavable()
        {

            return null;
        }
    }




    [Serializable]
    public class ChangeLightSettingsAction : ObjectAction
    {
        public DecorLighting targetLight = null;
        public Color newColor = Color.white;
        public float volume = 1f;
        public float intensity = 1f;
        public float range = 1f;
        public bool OnOff = true;

        public ChangeLightSettingsAction(DecorLighting target, Color color, float volume, float intensity, float range, bool OnOff)
        {
            targetLight = target;
            newColor = color;
            this.volume = volume;
            this.intensity = intensity;
            this.range = range;
            this.OnOff = OnOff;
        }

        public void SetActionSettings()
        {

        }

        public override void GenerateEventMethod(UnityEvent TargetEvent)
        {
            //  ChangeLightSettingsAction refrence = this;


            TargetEvent.AddListener(() => targetLight.ActionEvent_SetLight(this));
        }


    }





    public enum SetFloatActionType { equal, minus, add, divide, multiply }
    /// <summary>
    /// An action that will alter a variable on the target object, decided by the action tag
    /// </summary>
    [Serializable]
    public class SetFloatAction : ObjectAction
    {
        public SetFloatActionType floatOperator = SetFloatActionType.equal;
        public float value = 1f;


        public SetFloatAction(string ActionTag, SetFloatActionType floatOperator, float value, DecorObject target, string ActionName)
        {
            this.value = value;
            this.floatOperator = floatOperator;
            this.TargetObject = target;
            this.ActionTag = ActionTag;
            this.ObjectActionType = ObjectActionType.SetFloat;
            this.actionName = ActionName;
        }

        public SetFloatAction(SavableFloatAction targetAction)
        {
            this.ActionTag = targetAction.ActionTag;
            this.value = targetAction.value;
            this.floatOperator = targetAction.floatOperator;
            this.TargetObject = EditorController.Instance.MapDecor[targetAction.targetID];
            this.ObjectActionType = ObjectActionType.SetFloat;
            this.actionName = targetAction.ActionName;
        }

        public override void GenerateEventMethod(UnityEvent TargetEvent)
        {

            switch (this.ActionTag)
            {

                case "SetLightIntensity":
                    var IntensityTarget = (DecorLighting)this.TargetObject;
                    TargetEvent.AddListener(() => IntensityTarget.SetIntensity(this.value));
                    break;

                case "SetLightRange":
                    var RangeTarget = (DecorLighting)this.TargetObject;
                    TargetEvent.AddListener(() => RangeTarget.SetRange(this.value));
                    break;

                case "SetLightOn":
                    DecorLighting light = (DecorLighting)this.TargetObject;

                    break;


            }
        }


    }

    public enum SetBoolActionType {On,Off,Switch}
    [Serializable]
    public class SetBoolAction : ObjectAction
    {
        public SetBoolActionType boolActionType = SetBoolActionType.Off;


        public SetBoolAction(string ActionTag, SetBoolActionType boolActionType, DecorObject target, string actionName)
        {
            this.ObjectActionType = ObjectActionType.SetBool;
            this.ActionTag = ActionTag;
            this.boolActionType = boolActionType;
            this.TargetObject = target;
            this.actionName = actionName;

        }

        public SetBoolAction(SavableBoolAction targetAction)
        {
            this.ActionTag = targetAction.ActionTag;
            this.actionName = targetAction.ActionName;
            this.TargetObject = EditorController.Instance.MapDecor[targetAction.targetID];
            this.ObjectActionType = targetAction.ActionType;
            this.boolActionType = targetAction.boolType;
        }

        public override void GenerateEventMethod(UnityEvent TargetEvent)
        {
            switch (this.ActionTag)
            {
                case "SetLightOn":

                    break;
            }

        }
    }



    public enum SetColorActionType {Instant,Gradual }
    [Serializable]
    public class SetColorAction : ObjectAction
    {
        public SetColorActionType colorActionType = SetColorActionType.Instant;
        public float transitionTime = 1f;
        public Color color = Color.white;


        public SetColorAction(string ActionTag,SetColorActionType colorActionType, DecorObject target, string actionName, Color color, float transitionTime = 1f)
        {
            this.ObjectActionType = ObjectActionType.SetColor;
            this.ActionTag = ActionTag;
            this.colorActionType = colorActionType;
            this.TargetObject = target;
            this.actionName = actionName;
            this.transitionTime = transitionTime;
            this.color = color;
        }

        public SetColorAction(SavableColorAction targetAction)
        {
            this.ActionTag = targetAction.ActionTag;
            this.actionName = targetAction.ActionName;
            this.TargetObject = EditorController.Instance.MapDecor[targetAction.targetID];
            this.ObjectActionType = targetAction.ActionType;
            this.colorActionType = targetAction.colorActionType;
            this.transitionTime = targetAction.transitionTime;
            this.color = targetAction.color.ToColor();
        }




    }



    #endregion
}
