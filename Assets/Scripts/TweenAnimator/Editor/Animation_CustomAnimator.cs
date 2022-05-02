using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;
namespace UIAnimation
{
    [CustomEditor(typeof(Animation_Custom)), CanEditMultipleObjects]
    public class Animation_CustomAnimator : Editor
    {
        private bool m_ExpandUIOptions = false;

        private Ease stuff;
        private AnimationCurve _demoCurve = new AnimationCurve();
        public override void OnInspectorGUI()
        {
            Animation_Custom targetObject = (Animation_Custom)target;
            base.OnInspectorGUI();

            EditorGUILayout.LabelField("");

            m_ExpandUIOptions = EditorGUILayout.BeginFoldoutHeaderGroup(m_ExpandUIOptions, "UI animations");

            if (m_ExpandUIOptions)
            {


                ExtendedUI_Functions.DisplayUITools(targetObject.animationCommands);



            }
        }

    }
}
