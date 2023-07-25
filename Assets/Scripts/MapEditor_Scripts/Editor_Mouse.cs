using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

using UnityEngine.InputSystem;
public class Editor_Mouse : MonoBehaviour
{
    public static Editor_Mouse instance;
    public UnityEvent RightClick = new UnityEvent();
    public UnityEvent LeftClick = new UnityEvent();
    public UnityEvent MiddleClick = new UnityEvent();
    public UnityEvent MouseUpdate = new UnityEvent();
    public UnityEvent LeftClickHold = new UnityEvent();
    public UnityEvent ShiftLeftClick = new UnityEvent();



    [SerializeField] Editor_Mousemode_Cell mousemode_Cell;
    [SerializeField] Editor_MouseMode_Select mousemode_Select;


    public void Awake()
    {
        instance = this;
        SetMouseMode(EditorMouseMode.Build);
    }

    public void Update()
    {
        
    }


    void OnPoint(InputValue value)
    {
        
        MouseUpdate.Invoke();
    }

    void OnClick(InputValue value)
        {
        LeftClick.Invoke();
            print("click input");
        

    }

    void OnRightClick(InputValue value)
    {
        RightClick.Invoke();
        print("right click input");

    }

    void OnLeftClickDrag(InputValue value)
    {
        print("left drag input = " + value.Get<float>());

    }

    void OnShiftLeftClick(InputValue value)
    {
        ShiftLeftClick.Invoke();
    }



    public void SetMouseMode (EditorMouseMode mouseMode)
    {

        switch (mouseMode)
        {

            case EditorMouseMode.Select:
                mousemode_Select.RegisterModeEvents();
                break;

            case EditorMouseMode.Build:
                mousemode_Cell.RegisterModeEvents();
                break;

        }

    }
}
