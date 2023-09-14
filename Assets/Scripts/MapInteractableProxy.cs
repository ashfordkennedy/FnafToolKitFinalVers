using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInteractableProxy : MonoBehaviour
{
    [SerializeField] DecorObject target;
    public bool canInteract => EditorController.Instance.EditModeActive;



    public void OnMouseEnter()
    {
        target.gameObject.layer = 16;
    }

    public void OnMouseExit()
    {
        target.gameObject.layer = 12;
    }
}
