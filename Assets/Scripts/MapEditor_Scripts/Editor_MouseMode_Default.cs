using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor_MouseMode_Default : Editor_MouseMode_Abstract
{
    public static Editor_MouseMode_Default instance;

    private void Awake()
    {
        instance = this;
    }


    public void ObjectRaycast()
    {
        RaycastHit hit;
        Ray ray = Camera.current.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {


            switch (hit.transform.gameObject.layer)
            {

                // decor Object
                case 12:
                    DecorObject target = hit.transform.gameObject.GetComponentInParent<DecorObject>();
                    AnimatronicWaypoint output;
                    if (target.gameObject.TryGetComponent<AnimatronicWaypoint>(out output) == false)
                    {
                        print(target.name + "successfully hit waypoint object");
                        target.SendMessage("SetObjectTarget", target, SendMessageOptions.DontRequireReceiver);
                    }
                    break;
            }
        }
    }

    public void PickUpObject()
    {
        if(Editor_Mouse.highlightedObject != null)
        {
            Debug.Log("object picked up. fuck all in this method though. fix it");

        }
    }


    public void OpenObjectMenu()
    {

        var targetObject = Editor_Mouse.highlightedObject;

        if (targetObject != null)
        {
            switch (Editor_Mouse.highlightedObject.decorType)
            {
                case DecorObjectType.Light:
                    LightSettingUI.Instance.ToggleLightUI(true, (DecorLighting)targetObject);
                    break;

                case DecorObjectType.Waypoint:

                    break;




            }
        }
    }


    public void OpenContextMenu()
    {
        ;
    }


    public override void EnableMouseMode()
    {
        base.EnableMouseMode();
        Editor_Mouse.RightClick.AddListener(OpenObjectMenu);
        Editor_Mouse.LeftClick.AddListener(PickUpObject);
        
    }

    public override void DisableMouseMode()
    {
        Editor_Mouse.RightClick.RemoveListener(OpenObjectMenu);
        Editor_Mouse.LeftClick.RemoveListener(PickUpObject);
        //Editor_Mouse.ShiftRightClickDown.RemoveListener(ContextMenu.instance.OnShiftRightClick);
        //Editor_Mouse.ShiftRightClickUp.RemoveListener(ContextMenu.instance.OnShiftRightClickUp);
        base.DisableMouseMode();
    }
}

