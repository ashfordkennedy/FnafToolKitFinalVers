using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UIAnimation
{

    public delegate void UIAnimationResetDelegate();
    public abstract class Touch_Animation : MonoBehaviour, ITouchAnim_Behaviours
    {
        
        public UIAnimationResetDelegate resetDelegate;



        public virtual void StartAnimation(float dragIncrement, float maximumDragDuration)
        {

        }


        public virtual void AnimationReciever(float dragDuration)
        {
           
        }

        public virtual void AnimationReciever(ChargeData chargeData)
        {

        }

        public virtual void AnimationReciever(SwipeData swipeData)
        {

        }

        public virtual void EndAnimation(UIAnimationResetDelegate resetDelegate)
        {
           
        }
        public virtual void EndAnimation()
        {

        }

    }
}




