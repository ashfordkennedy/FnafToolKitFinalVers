using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;
namespace UIAnimation
{

    /// <summary>
    /// Created by: James Jordan
    /// created 13/07/2021
    /// 
    /// This class is used for custom touch animations using the basic functionality of the TweenAnimator with the utility of the Touch_Animation class
    /// to allow heavily customisable animations to be used instead of hard-coded animations.
    /// </summary>
    /// 
    public enum UIAnimationResetMode {none, reset, Gradual }
    public class Animation_Custom : Touch_Animation
    {
        #region Input_Settings
        public bool UseControllerValues = true;
        public float DragIncrementMultiplier = 1f;
        public float maximumDragDuration = 1;
        #endregion

        #region Animation_Settings
        public float animationSpeed = 1;
        public float animationResetMultiplier = 1;
        private float _sequencePosition = 0;
        public Sequence tweenSequence;
        public UIAnimationResetMode resetMode = UIAnimationResetMode.reset;
        #endregion


        

        [SerializeReference] public List<TweenAnimation_Abstract> animationCommands = new List<TweenAnimation_Abstract>();


        private void OnEnable()
        {
            DOTween.Init();
            tweenSequence = DOTween.Sequence();
            tweenSequence = ExtendedUI_Functions.ProcessAnimatorData(animationCommands); // ProcessAnimatorData(animationCommands, tweenSequence);

           // tweenSequence.SetLoops(-1);
            tweenSequence.timeScale = animationSpeed;
        }

        
        public override void StartAnimation(float dragIncrement, float maximumDragDuration)
        {
            tweenSequence.timeScale = animationSpeed;

            if (UseControllerValues == true)
            {                
                this.DragIncrementMultiplier = dragIncrement;
                this.maximumDragDuration = maximumDragDuration;
            }
            StopCoroutine("ResetLerpAnimation");
        }
        

        /// <summary>
        /// plays animation based on charge button data
        /// </summary>
        /// <param name="chargeData"></param>
        public override void AnimationReciever(ChargeData chargeData)
        {
            var percentage = chargeData.holdDuration / maximumDragDuration;

            var rounded = Mathf.Floor(percentage);
            _sequencePosition = percentage - rounded;

            tweenSequence.Goto(_sequencePosition, false);
        }
   
        public override void EndAnimation(UIAnimationResetDelegate resetDelegate)
        {
            switch (resetMode)
            {
                case UIAnimationResetMode.none:
                    resetDelegate();
                    break;


                case UIAnimationResetMode.reset:
                    tweenSequence.Goto(0, false);
                    resetDelegate();
                    break;

                case UIAnimationResetMode.Gradual:
                    StartCoroutine("ResetLerpAnimation", resetDelegate);
                    break;
            }
        }

        

        private IEnumerator ResetLerpAnimation(UIAnimationResetDelegate resetDelegate)
        {
            while (tweenSequence.position != 0f)
            {
                _sequencePosition -= Time.deltaTime * animationResetMultiplier;
                tweenSequence.Goto(_sequencePosition, false);              
                yield return new WaitForSeconds(0.01f);
            }
            resetDelegate();
            yield return null;
        }
        


    }


    
    }