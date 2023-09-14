using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Editor_MouseMode_Abstract : MonoBehaviour
{
    public static GameObject mouseTarget => Editor_Mouse.MouseTarget;
    public static Vector3 mouseHitPose => Editor_Mouse.MouseHitPos;

    [TextArea] public string MouseModeGuide = "";
    //public static Grabable mouseTargetGrabable => Editor_Mouse.GrabTarget;


    /// <summary>
    /// Clear the unassign method, then add listeners to required mouse events
    /// </summary>
    public virtual void EnableMouseMode()
    {
        Editor_Mouse.UnassignMouseMode.Invoke();
        Editor_Mouse.UnassignMouseMode.RemoveAllListeners();
        Editor_Mouse.UnassignMouseMode.AddListener(DisableMouseMode);
        
    }


    /// <summary>
    /// unsubscribe all this scripts listeners from the mouse events
    /// </summary>
    public virtual void DisableMouseMode()
    {
       
    }


    public bool MouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }




}