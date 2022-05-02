using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// Causes the triggering of the event when mouse enters the image.
/// </summary>
public class Button_OnEnter : Image, IPointerEnterHandler, IPointerExitHandler
{
    public Color defaultColour = Color.white;
    public Color highlightColour = Color.gray;
    public bool TriggerHoverEvent = false;
    public UnityEvent onPointerEnter;
    public UnityEvent onPointerHover;
    public UnityEvent onPointerExit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter.Invoke();
        StartCoroutine(OnHover());
        color = highlightColour;
    }

    public IEnumerator OnHover()
    {
        while(Time.time != 0)
        {
            onPointerHover.Invoke();
          yield return new WaitForSeconds(1);
            yield return null;
        }


        yield return null;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        color = defaultColour;
        StopCoroutine(OnHover());
    }
}



