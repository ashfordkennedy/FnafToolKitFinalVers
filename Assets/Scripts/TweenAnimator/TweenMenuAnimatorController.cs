using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIAnimation
{
    public enum MenuAnimatorCloseMode { Seperate, Reverse }
    public class TweenMenuAnimatorController : MonoBehaviour
    {
        public MenuAnimatorCloseMode menuAnimatorMode = MenuAnimatorCloseMode.Seperate;
        public TweenAnimator openAnimator;
        public TweenAnimator closeAnimator;


        public void PlayOpenAnimation()
        {
            if (openAnimator != null)
            {
                openAnimator.StartCoroutine("PlayAnimation");
            }

        }


        /// <summary>
        /// triggers the closing animation of the menu, using the MenuAnimatorCloseMode
        /// </summary>
        public void PlayCloseAnimation()
        {

            switch (menuAnimatorMode)
            {
                case MenuAnimatorCloseMode.Reverse:
                    if (openAnimator != null)
                    {
                        openAnimator.ReverseAnimation();
                    }
                    break;




                case MenuAnimatorCloseMode.Seperate:
                    if (closeAnimator != null)
                    {

                        closeAnimator.PlayAnimation();

                    }
                    break;

            }
        }
    }
}