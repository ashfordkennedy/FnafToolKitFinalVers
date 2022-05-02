using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;


namespace UIAnimation {
    [System.Serializable]
    public class TweenAnimator : MonoBehaviour
    {
#if UNITY_EDITOR
        public string animatorName = "TweenAnimator";
        [TextArea(0, 5)] public string Notes = "";
#endif

        public bool playOnEnable = false;
        public float startDelay = 0f;
        public Sequence tweenSequence;


        [SerializeReference] public List<TweenAnimation_Abstract> animationCommands = new List<TweenAnimation_Abstract>();


        private void OnEnable()
        {
            DOTween.Init();
            tweenSequence = DOTween.Sequence();
            tweenSequence = ExtendedUI_Functions.ProcessAnimatorData(animationCommands);

            if (playOnEnable == true)
            {

                StartCoroutine(PlayAnimation());
            }

        }


        private void OnDisable()
        {
            tweenSequence.Kill();
        }


        public IEnumerator PlayAnimation()
        {

            yield return new WaitForSeconds(startDelay);
            tweenSequence.PlayForward();



            yield return null;

        }


        public void ReverseAnimation()
        {
            tweenSequence.PlayBackwards();
        }

        public void PlayReverse()
        {

        }

        public void SetToStart()
        {
            tweenSequence.Goto(0f);
        }

        public void SetToEnd()
        {
            tweenSequence.Goto(1f);
        }

        public void AnimateFromBegining()
        {
            tweenSequence.Goto(0f);
          tweenSequence.PlayForward();
        }

    }

}

