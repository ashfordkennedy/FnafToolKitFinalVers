using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace UIAnimation
{
    /// <summary>
    /// Created by: James Jordan
    /// created 15/07/2021
    /// 
    ///This class contains any methods useful to UIAnimation systems that find use beyond a single system and its children
    /// </summary>
    public class ExtendedUI_Functions : MonoBehaviour
    {



        /// <summary>
        /// Loads given TweenAnimations into provided sequence.
        /// Used by any class using variations of TweenAnimator functionality
        /// </summary>
        /// <param name="AnimationList"></param>
        /// <param name="sequence"></param>
       public static Sequence ProcessAnimatorData(List<TweenAnimation_Abstract> AnimationList)
        {
            Sequence sequence = DOTween.Sequence();


            for (int i = 0; i < AnimationList.Count; i++)
            {

                switch (AnimationList[i].AnimationType)
                {
                    #region No_Animation
                    case TweenAnimationType.none:

                        break;
                    #endregion

                    #region UI_Fade
                    case TweenAnimationType.UIfade:

                        var fadeTarget = (TweenAnimation_UIFade)AnimationList[i];
                        if (fadeTarget.target != null)
                        {
                            float startOpacity = fadeTarget.startOpacity;
                            fadeTarget.target.alpha = startOpacity;
                            //  fadeTarget.target.alpha = 0f;



                            if (fadeTarget.ease == Ease.Unset)
                            {
                                sequence.Join(fadeTarget.target.DOFade(fadeTarget.TargetOpacity, fadeTarget.animationTime).SetEase(fadeTarget.animationCurve));
                            }
                            else
                            {
                                sequence.Join(fadeTarget.target.DOFade(fadeTarget.TargetOpacity, fadeTarget.animationTime).SetEase(fadeTarget.ease));
                            }
                        }

                        break;
                    #endregion

                    #region UI_Position
                    case TweenAnimationType.UIposition:

                        var posTarget = (TweenAnimation_UIPosition)AnimationList[i];
                        if (posTarget.target != null)
                        {
                            posTarget.target.position = posTarget.startPosition;


                            if (posTarget.ease == Ease.Unset)
                            {
                                sequence.Join(posTarget.target.DOMove(posTarget.targetPosition, posTarget.animationTime, false).SetEase(posTarget.animationCurve));
                            }
                            else
                            {
                                sequence.Join(posTarget.target.DOMove(posTarget.targetPosition, posTarget.animationTime, false).SetEase(posTarget.ease));
                            }
                        }
                        break;
                    #endregion

                    #region UI_Rotation
                    case TweenAnimationType.UIrotation:

                        var rotTarget = (TweenAnimation_UIRotation)AnimationList[i];
                        if (rotTarget.target != null)
                        {
                            rotTarget.target.eulerAngles = rotTarget.startRotation;

                            if (rotTarget.ease == Ease.Unset)
                            {
                                sequence.Join(rotTarget.target.DORotate(rotTarget.targetRotation, rotTarget.animationTime, RotateMode.FastBeyond360).SetEase(rotTarget.animationCurve));
                            }
                            else
                            {
                                sequence.Join(rotTarget.target.DORotate(rotTarget.targetRotation, rotTarget.animationTime, RotateMode.FastBeyond360).SetEase(rotTarget.ease));
                            }
                        }
                        break;
                    #endregion

                    #region UI_Scale
                    case TweenAnimationType.UIscale:

                        var scaleTarget = (TweenAnimation_UIScale)AnimationList[i];

                        if (scaleTarget.target != null)
                        {
                            scaleTarget.target.localScale = scaleTarget.startScale;


                            if (scaleTarget.ease == Ease.Unset)
                            {
                                sequence.Join(scaleTarget.target.DOScale(scaleTarget.targetScale, scaleTarget.animationTime).SetEase(scaleTarget.animationCurve));
                            }
                            else
                            {
                                sequence.Join(scaleTarget.target.DOScale(scaleTarget.targetScale, scaleTarget.animationTime).SetEase(scaleTarget.ease));
                            }
                        }

                        break;
                    #endregion

                    #region UI_Color
                    case TweenAnimationType.UIcolor:
                        var colTarget = (TweenAnimation_UIColor)AnimationList[i];

                        if (colTarget.target != null)
                        {
                            colTarget.target.color = colTarget.startColor;

                            if (colTarget.ease == Ease.Unset)
                            {
                                sequence.Join(colTarget.target.DOColor(colTarget.targetColor, colTarget.animationTime).SetEase(colTarget.animationCurve));
                            }
                            else
                            {
                                sequence.Join(colTarget.target.DOColor(colTarget.targetColor, colTarget.animationTime).SetEase(colTarget.ease));
                            }
                        }
                        break;
                    #endregion

                    #region UI_Target_Position
                    case TweenAnimationType.UItargetposition:
                        var transTarget = (TweenAnimation_UITargetPosition)AnimationList[i];

                        if (transTarget != null)
                        {
                            transTarget.target.position = transTarget.startTransform.position;
                            transTarget.target.eulerAngles = transTarget.startTransform.eulerAngles;
                            transTarget.target.localScale = transTarget.startTransform.localScale;



                            if (transTarget.ease == Ease.Unset)
                            {
                                sequence.Join(transTarget.target.DOMove(transTarget.targetTransform.position, transTarget.animationTime, false).SetEase(transTarget.animationCurve));
                                sequence.Join(transTarget.target.DOScale(transTarget.targetTransform.localScale, transTarget.animationTime).SetEase(transTarget.animationCurve));
                                sequence.Join(transTarget.target.DORotate(transTarget.targetTransform.rotation.eulerAngles, transTarget.animationTime).SetEase(transTarget.animationCurve));
                            }
                            else
                            {
                                sequence.Join(transTarget.target.DOMove(transTarget.targetTransform.position, transTarget.animationTime, false).SetEase(transTarget.ease));
                                sequence.Join(transTarget.target.DOScale(transTarget.targetTransform.localScale, transTarget.animationTime).SetEase(transTarget.ease));
                                sequence.Join(transTarget.target.DORotate(transTarget.targetTransform.rotation.eulerAngles, transTarget.animationTime).SetEase(transTarget.ease));
                            }
                        }

                        break;
                    #endregion

                    #region UI_Shake
                    case TweenAnimationType.UIshake:
                        var shakeTarget = (TweenAnimation_UIShake)AnimationList[i];

                        if (shakeTarget.target != null)
                        {
                            //   duration strength vibrato randomness fadeout


                            if (shakeTarget.ease == Ease.Unset)
                            {
                                sequence.Join(shakeTarget.target.DOShakeAnchorPos(shakeTarget.animationTime, shakeTarget.strength, shakeTarget.vibrato, shakeTarget.randomness, false, shakeTarget.fadeout).SetEase(shakeTarget.ease));
                            }
                            else
                            {
                                sequence.Join(shakeTarget.target.DOShakeAnchorPos(shakeTarget.animationTime, shakeTarget.strength, shakeTarget.vibrato, shakeTarget.randomness, false, shakeTarget.fadeout).SetEase(shakeTarget.ease));
                            }
                        }

                        break;
                    #endregion

                    #region UI_Fill
                    case TweenAnimationType.UIfill:

                        var fillTarget = (TweenAnimation_UIFill)AnimationList[i];
                        if (fillTarget.target != null)
                        {
                            float startFill = fillTarget.startValue;
                            fillTarget.target.fillAmount = startFill;
                            //  fadeTarget.target.alpha = 0f;



                            if (fillTarget.ease == Ease.Unset)
                            {
                                sequence.Join(fillTarget.target.DOFillAmount(fillTarget.endValue, fillTarget.animationTime).SetEase(fillTarget.animationCurve));
                            }
                            else
                            {
                                sequence.Join(fillTarget.target.DOFillAmount(fillTarget.endValue, fillTarget.animationTime).SetEase(fillTarget.ease));
                            }
                        }

                        break;
                    #endregion

                    #region UI_Pivot
                    case TweenAnimationType.UIPivot:

                        var PivotTarget = (TweenAnimation_UIPivot)AnimationList[i];
                        if (PivotTarget.target != null)
                        {
                           
                           


                            if (PivotTarget.ease == Ease.Unset)
                            {
                                sequence.Join(PivotTarget.target.DOPivot(PivotTarget.endValue, PivotTarget.animationTime).SetEase(PivotTarget.animationCurve));
                            }
                            else
                            {
                                sequence.Join(PivotTarget.target.DOPivot(PivotTarget.endValue, PivotTarget.animationTime).SetEase(PivotTarget.ease));
                            }
                        }

                        break;
                    #endregion

                    #region UI_canvasInteractable
                    case TweenAnimationType.UICanvasInteractable:

                        var canvasTarget = (TweenAnimation_UICanvasInteractable)AnimationList[i];
                        if (canvasTarget.target != null)
                        {
                            bool startInteractable = canvasTarget.startValue;
                           
                            //  fadeTarget.target.alpha = 0f;



                            if (canvasTarget.ease == Ease.Unset)
                            {
                               // sequence.Join(canvasTarget.target.(fillTarget.endValue, fillTarget.animationTime).SetEase(fillTarget.animationCurve));
                            }
                            else
                            {
                               // sequence.Join(canvasTarget.DOFillAmount(fillTarget.endValue, fillTarget.animationTime).SetEase(fillTarget.ease));
                            }
                        }

                        break;
                        #endregion
                }
            }

            return sequence;
        }


#if UNITY_EDITOR

        /// <summary>
        /// This is an Editor function, Calling it will create an organised list of buttons which will add animation settings to the given list
        /// </summary>
        /// <param name="targetList"></param>
        public static void DisplayUITools(List<TweenAnimation_Abstract> targetList)
        {

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Pos"))
            {
                targetList.Add(new TweenAnimation_UIPosition());
            }

            if (GUILayout.Button("Scale"))
            {
                targetList.Add(new TweenAnimation_UIScale());
            }

            if (GUILayout.Button("Rotation"))
            {
                targetList.Add(new TweenAnimation_UIRotation());
            }

            EditorGUILayout.EndHorizontal();




            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("TargetPos"))
            {
                targetList.Add(new TweenAnimation_UITargetPosition());
            }
            if (GUILayout.Button("Fade"))
            {
                targetList.Add(new TweenAnimation_UIFade());
            }
            if (GUILayout.Button("Color"))
            {
                targetList.Add(new TweenAnimation_UIColor());
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Shake"))
            {
                targetList.Add(new TweenAnimation_UIShake());
            }

            if (GUILayout.Button("Fill"))
            {
                targetList.Add(new TweenAnimation_UIFill());
            }

            if (GUILayout.Button("Pivot"))
            {
                targetList.Add(new TweenAnimation_UIPivot());
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndFoldoutHeaderGroup();

        }

#endif



        /// <summary>
        /// Generates a value between 0-1 using time values to determine the desired animation position
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="currentTime"></param>
        /// <param name="MaximumTime"></param>
        /// <returns></returns>
        public static ChargeData CalculateDuration(float startTime, float currentTime, float MaximumTime)
        {
            float holdTime = currentTime - startTime;
            float percentage = Mathf.InverseLerp(0, MaximumTime, holdTime);

            var data = new ChargeData(holdTime, percentage);
            return data;
        }


    }
}
