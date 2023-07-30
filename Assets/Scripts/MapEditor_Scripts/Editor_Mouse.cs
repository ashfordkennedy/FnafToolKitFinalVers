using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;
using System.Threading.Tasks;
public class Editor_Mouse : MonoBehaviour
{
    public static Editor_Mouse instance;

    [SerializeField] Camera _targetCamera;

    public static UnityEvent LeftClick = new UnityEvent();
    public static UnityEvent RightClick = new UnityEvent();
    public static UnityEvent ShiftClick = new UnityEvent();
    public static UnityEvent MouseUpdate = new UnityEvent();

    public UnityEvent LeftClickHold = new UnityEvent();
    public UnityEvent ShiftLeftClick = new UnityEvent();



    [SerializeField] Editor_Mousemode_Cell mousemode_Cell;
    [SerializeField] Editor_MouseMode_Select mousemode_Select;


    public static UnityEvent Drag = new UnityEvent();
    public static UnityEvent StopDrag = new UnityEvent();

    public static UnityEvent UnassignMouseMode = new UnityEvent();

    [SerializeField] public static GameObject MouseTarget;
    [SerializeField] public static Vector3 MouseHitPos;
    [SerializeField] public static Vector2 MousePos;

    public float handOffset = 5f;
    private bool dragging = false;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDrag(InputValue value)
    {

        switch (value.Get<float>())
        {

            case 1:
                print("dragging");
                dragging = true;
                DragProcessing();
                break;


            case 0:
                dragging = false;
                StopDrag.Invoke();
                print("drag end");
                break;
        }

    }

    private async void DragProcessing()
    {

        while (dragging != false)
        {
            Drag.Invoke();
            await Task.Delay(1000);
            await Task.Yield();
        }

        return;
    }

    private void OnDoubleLeftClick(InputValue value)
    {
        print("double clicked " + value.Get<float>());
    }

    void OnMouseMove(InputValue value)
    {
        // print("moving mouse update " + Time.time);

        RaycastHit hit;
        MousePos = value.Get<Vector2>();
        Ray ray = _targetCamera.ScreenPointToRay(MousePos);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {

        }
    }

    void OnLeftClick(InputValue value)
    {
        LeftClick.Invoke();
        print("Left click working");
    }



    void OnRightClick(InputValue value)
    {
        RightClick.Invoke();
        print("Right click working");
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
