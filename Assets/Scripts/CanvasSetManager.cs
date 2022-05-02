using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CanvasSetManager : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;





    public virtual void HideCanvas(bool mode)
    {
        switch (mode)
        {
            case true:
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                break;

            case false:
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                break;
        }
    }
}
