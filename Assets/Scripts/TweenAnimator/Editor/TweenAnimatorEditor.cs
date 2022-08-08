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


        EditorGUILayout.LabelField("Click generate animation to store the current animation ");
        EditorGUILayout.LabelField("commands in the objects sequence value");
        if (GUILayout.Button("GenerateAnimation"))
        {
            targetObject.tweenSequence = targetObject.GenerateTween();
        }
        if (m_ExpandUIOptions)
        {
            ExtendedUI_Functions.DisplayUITools(targetObject.animationCommands);
            //DisplayUITools(targetObject);
        }


        EditorGUILayout.LabelField("");
        EditorGUILayout.Space();

    } 
}



