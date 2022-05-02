using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class PlaymodeClickable : MonoBehaviour,IPlayModeInteractable
{
    public UnityEvent OnLeftClick;
    public UnityEvent OnRightClick;

    public void LeftClick()
    {
        Debug.Log("buttonpressed");
        OnLeftClick.Invoke();
    }

    public void RightClick()
    {
        OnRightClick.Invoke();
    }

   
}
