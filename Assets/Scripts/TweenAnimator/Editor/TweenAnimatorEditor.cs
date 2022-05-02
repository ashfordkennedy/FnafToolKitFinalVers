using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using UIAnimation;
[CustomEditor(typeof(TweenAnimator)), CanEditMultipleObjects]
public class TweenEdtorAnimator : Editor
{
    private bool m_ExpandUIOptions = false;


    public override void OnInspectorGUI()
    {
        TweenAnimator targetObject = (TweenAnimator)target;
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("");

        m_ExpandUIOptions = EditorGUILayout.BeginFoldoutHeaderGroup(m_ExpandUIOptions, "UI animations");

        if (m_ExpandUIOptions)
        {
            ExtendedUI_Functions.DisplayUITools(targetObject.animationCommands);
            //DisplayUITools(targetObject);
        }


        EditorGUILayout.LabelField("");
        EditorGUILayout.Space();

    } 
}



