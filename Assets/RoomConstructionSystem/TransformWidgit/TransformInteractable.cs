using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TransformInteractable : MonoBehaviour
{
    
   public ObjectTransformController TransformController;
    public AxisMode axisMode;

    private Vector3 _mouseOrigin;


    public void OnMouseDown()
    {
        _mouseOrigin = Input.mousePosition;
        print(_mouseOrigin + "is new origin");
       // TransformController.ScreenPoint = Camera.main.WorldToScreenPoint(TransformController.TargetTransformObject.transform.position);

        // TransformController.Offset = TransformController.TargetTransformObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));


    }

    public void OnMouseDrag()
    {
       // print("clicked");

        switch (axisMode)
        {
            case AxisMode.X:
                TransformController.DragX();

                break;

            case AxisMode.Y:
                TransformController.DragY(_mouseOrigin);

                break;

            case AxisMode.Z:
                TransformController.DragZ();

                break;
        }

        TransformController.UpdateTransformUI();


    }
}
