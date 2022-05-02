using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UIAnimation;
using UnityEngine.Events;
namespace UnityEngine.UI
{

    [AddComponentMenu("UI/ChargeButton", 32)]
    /// <summary>
    /// Created by: James Jordan
    /// created 14/04/2021
    /// 
    /// This class is an experiment as to whether the behaviour of the TouchField component can be added to buttons through a simple extension rather
    /// than a completely new class. Touch_Animation derived classes are designed to work with this class in the same way they do TouchFields.
    /// </summary>
    public class ChargeButton : Button
    {
        public bool CooldownOnMax = false;
        public bool touching = false;

        public float holdTime = 0f;
        public float holdIncrement = 1f;
        public float maxHoldTime = 5f;

        [Tooltip("Any object you wish to receive the completed input data from this button, implement 'IChargeDataReciever' on a script designed to process this data and attach to the target object")]
        public GameObject targetObject = null;

        public Touch_Animation Animation_Controller;
        public UIAnimationResetDelegate resetDelegate;


        public UnityEvent OnOverload = new UnityEvent();
        public UnityEvent OnContact = new UnityEvent();
        public UnityEvent OnRelease = new UnityEvent();


        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (interactable == true)
            {
                touching = true;
                OnContact.Invoke();
                Animation_Controller.StartAnimation(holdIncrement, maxHoldTime);
                StartCoroutine("HoldTimer");
            }


        }


        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            resetDelegate = ReenableButton;
            OnRelease.Invoke();
            Animation_Controller.EndAnimation(resetDelegate);
            touching = false;
            
            
        }

        /// <summary>
        /// Simple method used by resetDelegate, to be called by the animator once cooldown is complete
        /// </summary>
        public void ReenableButton()
        {
            interactable = true;
            print("delegate online");
        }


        /// <summary>
        /// Is called by onPointerDown and cancelled by OnPointerUp. Duration is passed off to the animation component each loop
        /// </summary>
        /// <returns></returns>
        private IEnumerator HoldTimer()
        {
            float startTime;
            startTime = Time.time;

            ChargeData holdData = new ChargeData();
            while (touching == true)
            {
                holdData = ExtendedUI_Functions.CalculateDuration(startTime, Time.time, maxHoldTime);

                if (holdData.chargePercentage >= 1 && CooldownOnMax == true)
                {
                    touching = false;
                    Debug.Log("hold time reached");
                    interactable = false;
                    OnOverload.Invoke();
                    break;
                }


                if (Animation_Controller != null)
                {
                    Animation_Controller.AnimationReciever(holdData);
                }

                yield return new WaitForSeconds(0.01f * Time.deltaTime);
                
            }



            if(targetObject != null)
            {
                targetObject.SendMessage("RecieveChargeData", holdData, SendMessageOptions.DontRequireReceiver);
            }




            yield return null;
        }



        private IEnumerator CoolDownTimer()
        {
            while (touching == false)
            {
                holdTime = 0f;
            }
            yield return null;
        }



        private void SendEventData()
        {

        }
    }


    public struct ChargeData{
        public float holdDuration;
        public float chargePercentage;


        public ChargeData(float holdDuration, float chargePercentage)
        {
            this.holdDuration = holdDuration;
            this.chargePercentage = chargePercentage;
        }
    }


    /// <summary>
    /// Interface for receiving information from a ChargeButton class. Use it to process ChargeData accordingly.
    /// </summary>
    public interface IChargeDataReciever
    {

        void RecieveChargeData(ChargeData chargeData);

    }
}
