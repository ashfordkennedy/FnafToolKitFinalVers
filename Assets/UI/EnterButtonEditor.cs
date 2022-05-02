using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Button_OnEnter))]
public class EnterButtonEditor : ImageEditor
{

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Button_OnEnter target = this.target as Button_OnEnter;


        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Interaction Colours", GUI.skin.box);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("defaultColour"), true);
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("highlightColour"), true);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Touch Events", GUI.skin.box);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onPointerEnter"), true);
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onPointerExit"), true);

        target.TriggerHoverEvent = EditorGUILayout.Toggle("Trigger Hover Event", target.TriggerHoverEvent);
        if(target.TriggerHoverEvent == true)
        {
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onPointerHover"), true);

        }


        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(target.gameObject.scene);
        }

    }
}
