using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using TMPro;
using UnityEditor.SceneManagement;


[CustomEditor(typeof(Ext_Slider))]
public class Ext_Slider_Editor : SliderEditor
{
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Ext_Slider targetExt = (Ext_Slider)target;

        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("valueDisplay"), true);

        targetExt.useAdditions = EditorGUILayout.Toggle("Use Suffix/prefix's", targetExt.useAdditions);
        if (targetExt.useAdditions)
        {
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("valuePrefix"), true);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("valueSuffix"), true);
        }

        this.Repaint();
        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
