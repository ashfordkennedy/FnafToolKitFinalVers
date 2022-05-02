using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Text;
using System.Linq;
using UnityEditor.UI;
using UIAnimation;

[CustomEditor(typeof(ChargeButton)), CanEditMultipleObjects]
public class ChargeButtonEditor : ButtonEditor
{

    public Object chargeTargetObject;

    private SerializedProperty m_onReleaseEvent;
    private SerializedProperty m_onOverloadEvent;
    private SerializedProperty m_onContactEvent;

    private bool _displayEvents = false;
    private bool _displayHoldVariables = false;

   protected override void OnEnable()
    {
        base.OnEnable();
        m_onReleaseEvent = serializedObject.FindProperty("OnRelease");
        m_onContactEvent = serializedObject.FindProperty("OnContact");
        m_onOverloadEvent = serializedObject.FindProperty("OnOverload");
    }




    public override void OnInspectorGUI()
    {
        ChargeButton targetObject = (ChargeButton)target;

        base.OnInspectorGUI();

        targetObject.targetObject = (GameObject)EditorGUILayout.ObjectField("TargetObject",targetObject.targetObject, typeof(GameObject), true);

        targetObject.CooldownOnMax = EditorGUILayout.Toggle("Enable Cooldown On Max", targetObject.CooldownOnMax);


        #region Events

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Button Events", GUI.skin.box);
        EditorGUILayout.Space();
        _displayEvents = EditorGUILayout.Foldout(_displayEvents, "Expand Events");

        if(_displayEvents){
            EditorGUILayout.PropertyField(m_onContactEvent, new GUIContent("OnContact", "OnContact is called when the player first touches the button"));
            EditorGUILayout.PropertyField(m_onReleaseEvent, new GUIContent("OnRelease", "OnRelease is triggered once the player is no longer touching the button"));
            EditorGUILayout.PropertyField(m_onOverloadEvent, new GUIContent("OnOverload", "OnOverload is called once the hold time exceeds the maximum amount - Remember to enable cool-down"));
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();

        #endregion




        targetObject.Animation_Controller = (Touch_Animation)EditorGUILayout.ObjectField("Animation Controller", targetObject.Animation_Controller, typeof(Touch_Animation), true);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();




        #region Charge_Settings

        EditorGUILayout.LabelField("Charge Settings", GUI.skin.box);
        _displayHoldVariables = EditorGUILayout.Foldout(_displayHoldVariables, "Expand Settings");

        if (_displayHoldVariables)
        {
            targetObject.holdIncrement = EditorGUILayout.FloatField("hold Increment", targetObject.holdIncrement);
            targetObject.maxHoldTime = EditorGUILayout.FloatField("Max Hold Time", targetObject.maxHoldTime);
            targetObject.holdTime = EditorGUILayout.FloatField("hold Time", targetObject.holdTime);
        }
        #endregion

        serializedObject.ApplyModifiedProperties();

    }
}
