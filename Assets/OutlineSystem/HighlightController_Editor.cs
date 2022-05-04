#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(HighlightController))]
public class HighlightController_Editor : Editor
{

    public override void OnInspectorGUI()
    {
        HighlightController target = this.target as HighlightController;
        base.OnInspectorGUI();
       //EditorGUILayout.ObjectField("Animation Controller", HighlightController.highlightMaterial, typeof(Material), true);
        HighlightController.highlightMaterial = (Material)EditorGUILayout.ObjectField("Animation Controller", HighlightController.highlightMaterial, typeof(Material), true);


        if (GUILayout.Button("TestEnable"))
        {

        }
        if (GUILayout.Button("TestDisable"))
        {

        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(target.gameObject.scene);
        }
    }
}
#endif