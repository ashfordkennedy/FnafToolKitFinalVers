using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraUiButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{


    [SerializeField] float ScrollAmnt = 0.25f;
    [SerializeField] int CamAmnt = 1;

    public void OnPointerEnter(PointerEventData eventData)
    {
      GameManager.GM.MC_Ctrl.CameraPan(ScrollAmnt, CamAmnt);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.GM.MC_Ctrl.CameraPanStop();

    }
}
