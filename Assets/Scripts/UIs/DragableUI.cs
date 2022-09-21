using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DragableUI : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private RectTransform Rtransform;
    [SerializeField] GameObject dragTarget;
    private Vector3 OriginalPos;
    private Vector2 LastMousePos;
    public void Awake()
    {

        Rtransform = dragTarget.GetComponent<RectTransform>();
        OriginalPos = Rtransform.position;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData){
    {
            Vector2 Mousepos = eventData.position;
            Vector2 diff = Mousepos - LastMousePos;

            Vector3 newpos = Rtransform.position + new Vector3(diff.x,diff.y, transform.position.z);
            Vector3 oldpos = Rtransform.position;

            Rtransform.position = newpos;
            if(WindowOnScreen(Rtransform) == false)
            {
                Rtransform.position = oldpos;
            }
            LastMousePos = Mousepos;
    }


}

    public void OnEndDrag(PointerEventData eventData)
    {
   
    }




    bool WindowOnScreen(RectTransform WindowRect)
    {
        bool OnScreen = false;

        Vector3[] RectCorners = new Vector3[4];

        WindowRect.GetWorldCorners(RectCorners);
        int CornersVisible = 0;
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);

        foreach(Vector3 corner in RectCorners)
        {
            if (rect.Contains(corner))
            {
                CornersVisible++;

            }

        }

        if(CornersVisible == 4)
        {
            OnScreen = true;
        }

        return OnScreen;
    }
}


//// Drag code put together from tutorial. Check out rect.Contains. Imagine a dragable Item in an inventory UI and using .Contains to check for >=2 corners inside the rect behind the dragged object
///could work for easy targeting for a more packed in ui. Or not. you could just check you hit the rect at that point and use that. But keep it in mind.