using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TooltipTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   [TextArea] [SerializeField] string _tooltip;

    /*
    private void OnMouseOver()
    {
        TooltipMenu.instance.RepositionTooltip();
    }
    */

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipMenu.instance.DisplayToolTip(_tooltip);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipMenu.instance.WipeToolTip();
    }
}
