using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public abstract class EditorMenuAbstract : MonoBehaviour
{
    public GameObject UICanvas;
    public bool MenuOpen { get; private set; }

    public virtual void OpenMenu()
    {
        UICanvas.SetActive(true);
        MenuOpen = true;
    }

    public virtual void CloseMenu()
    {
       UICanvas.SetActive(false);
        MenuOpen = false;
    }

    public virtual void ToggleMenu()
    {
        switch(UICanvas.activeInHierarchy == true)
        {
            case true:
                CloseMenu();
                break;

            case false:
                OpenMenu();
                break;
               
        }



      //  UICanvas.SetActive(!UICanvas.activeInHierarchy);
      //  _menuOpen = UICanvas.activeInHierarchy;
    }

    public virtual bool MouseOverUICheck()
    {
        //  var pointerData = new PointerEventData(EventSystem.current);
        //  List<RaycastResult> results = new List<RaycastResult>();

        // EventSystem.current.RaycastAll(pointerData, results);

        //  print("all hit ui objects" + results.Count);
        //   return (results.Count > 0);

        return EventSystem.current.IsPointerOverGameObject();
    }



}
