using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Abstract_Hud : MonoBehaviour
{
    [SerializeField] internal CanvasGroup canvasGroup;
    [SerializeField] internal GameObject canvasContainer;

    public void EnableHud()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasContainer.SetActive(true);
    }

    public void DisableHud()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasContainer.SetActive(false);
    }
}
