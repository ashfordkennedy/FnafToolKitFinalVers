using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UIAnimation;

[DisallowMultipleComponent]
public class TouchField : Image, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IDragHandler
{

    [Tooltip("The object that will receive DragData after the completion of a successful drag")]
    public GameObject Target;
    public bool interactable = true;
    [Tooltip("if enabled, the input will complete when the player drags outside of the Touch Field")]
    public bool endOnTouchExit = true;
    //animator
    public Touch_Animation Animation_Controller;
    public UIAnimationResetDelegate resetDelegate;


    #region Drag_Settings
    public float dragIncrementMultiplier = 1f;
    public float dragDuration = 0;
    public float maximumDragDuration = 5f;
    public bool cooldownOnMax = false;
    #endregion


    #region dragData
    private Vector2 _dragOrigin;
    private Vector2 _dragRelease;
    [SerializeField] SwipeData Swipe_Data;
    public bool touching;
    #endregion



    public void OnPointerDown(PointerEventData eventData)
    {
        touching = true;
        _dragOrigin = eventData.pressPosition;
        if (Animation_Controller != null)
        {
            Animation_Controller.StartAnimation(dragIncrementMultiplier, maximumDragDuration);
            StartCoroutine("HoldTimer");
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _dragRelease = eventData.position;
        resetDelegate = ResetDelegatePlaceholder;
        Animation_Controller.EndAnimation(resetDelegate);
        touching = false;
        dragDuration = 0;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        /// End on exit not currently working
        /*
        if (dragDuration > 0f && endOnTouchExit == true)
        {
            OnPointerUp(eventData);
        }
        */
    }


    /// <summary>
    /// Creates DragData and passes to the Target, Is called by onPointerDown and cancelled by OnPointerUp.
    /// </summary>
    /// <returns></returns>  
    public void OnDrag(PointerEventData eventData)
    {
        if (touching == true)
        {
            if (Target != null)
            {
                Target.SendMessage("OnFieldDrag", new DragData(_dragOrigin, eventData.position), SendMessageOptions.DontRequireReceiver);
            }
        }
    }


    public void ResetDelegatePlaceholder()
    {
        print("Reset was triggered on touchfield");
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
        // while touching, calculate holdData and send to the animator
        while (touching == true)
        {
            holdData = ExtendedUI_Functions.CalculateDuration(startTime, Time.time, maximumDragDuration);
            dragDuration = holdData.holdDuration;
            // break loop for cooldown
            if (holdData.chargePercentage >= 1 && cooldownOnMax == true)
            {
                touching = false;
                Debug.Log("hold time reached");
                interactable = false;
                break;
            }

            if (Animation_Controller != null)
            {
                Animation_Controller.AnimationReciever(holdData);
            }


            yield return new WaitForSeconds(0.01f * Time.deltaTime);

        }
        //Loop is broken by no longer touching. Process swipe from generated data
        SwipeProcessing(holdData);

        yield return null;
    }





    /// <summary>
    /// Processes swipe input for the target object after completion of drag
    /// </summary>
    void SwipeProcessing(ChargeData chargeData)
    {
        SwipeData swipeData = new SwipeData();

        #region Generate_Direction
        swipeData.directionRaw = (_dragOrigin - _dragRelease).normalized * -1;

        float factor = 100f;
        var roundedX = (Mathf.Round(swipeData.directionRaw.x * factor)) / factor;
        var roundedY = (Mathf.Round(swipeData.directionRaw.y * factor)) / factor;

        swipeData.direction = new Vector2(roundedX, roundedY);
        #endregion

        swipeData.swipeTime = chargeData.holdDuration;
        swipeData.heldPercentage = chargeData.chargePercentage;

        if (Target != null)
        {
            Target.SendMessage("OnSwipe", swipeData, SendMessageOptions.DontRequireReceiver);
        }

        Swipe_Data = swipeData;
    }






}


public struct DragData
{
    public Vector2 StartPosition;
    public Vector2 CurrentPosition;

    public DragData(Vector2 startPosition, Vector2 currentPosition)
    {
        StartPosition = startPosition;
        CurrentPosition = currentPosition;
    }

}


[Serializable]
public struct SwipeData
{
    public Vector2 direction;
    public Vector2 directionRaw;
    public float swipeTime;
    public float heldPercentage;
}



/// <summary>
/// Receives swipeData from a TouchField
/// </summary>
public interface ITouchFieldReciever
{
    /// <summary>
    /// Receives the completed Swipe data from the TouchField
    /// </summary>
    /// <param name="swipeData"></param>
    void OnSwipe(SwipeData swipeData);


    /// <summary>
    /// Receives the start and current position of the drag event.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    void OnFieldDrag(DragData dragData);

}