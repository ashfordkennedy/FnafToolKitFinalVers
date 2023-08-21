using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;
using System.Threading.Tasks;
using System.Threading;
public class Editor_Mouse : MonoBehaviour
{
    public static Editor_Mouse instance;

    public static DecorObject highlightedObject;

    [SerializeField] Camera _targetCamera;


    //left click
    public static UnityEvent LeftClickUp = new UnityEvent();
    public static UnityEvent LeftClick = new UnityEvent();
    public static UnityEvent LeftClickDown = new UnityEvent();


    // middle click
    public static UnityEvent MiddleClickUp = new UnityEvent();
    public static UnityEvent MiddleClick = new UnityEvent();
    public static UnityEvent MiddleClickDown = new UnityEvent();

    // right click
    public static UnityEvent RightClickUp = new UnityEvent();
    public static UnityEvent RightClick = new UnityEvent();
    public static UnityEvent RightClickDown = new UnityEvent();



    public static UnityEvent ShiftClick = new UnityEvent();



    // shift click
    public UnityEvent ShiftLeftClick = new UnityEvent();


    public static UnityEvent ShiftRightClickUp = new UnityEvent();
    public static UnityEvent ShiftRightClick = new UnityEvent();
    public static UnityEvent ShiftRightClickDown = new UnityEvent();

    //Updates
    public static UnityEvent LeftClickHold = new UnityEvent();
    public static UnityEvent RightClickHold = new UnityEvent();

    /// <summary>
    /// called while mouse is moving
    /// </summary>
    public static UnityEvent MouseUpdate = new UnityEvent();


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
    private float ShiftHeld = 0;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Editor_MouseMode_Default.instance.EnableMouseMode();

        ShiftRightClickDown.AddListener(ContextMenu.instance.OnShiftRightClick);
        ShiftRightClickUp.AddListener(ContextMenu.instance.OnShiftRightClickUp);
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

   


    void OnRightClick(InputValue value)
    {

        if (ShiftHeld != 1)
        {
            RightClick.Invoke();
            print("Right click working");
            switch (value.Get<float>())
            {
                case 1:
                    RightClickDown.Invoke();
                    break;

                case 0:
                    RightClickUp.Invoke();
                    break;
            }
        }


        else
        {
            ShiftRightClick.Invoke();
            switch (value.Get<float>())
            {
                case 1:
                    ShiftRightClickDown.Invoke();
                    break;

                case 0:
                    ShiftRightClickUp.Invoke();
                    break;
            }
        }
    }

    void OnPoint(InputValue value)
    {
        
        MouseUpdate.Invoke();
    }

    void OnClick(InputValue value)
        {


        switch (ShiftHeld)
        {
            case 0:
                LeftClick.Invoke();

                switch (value.Get<float>())
                {
                    case 1:
                        LeftClickDown.Invoke();
                        break;

                    case 0:
                        LeftClickUp.Invoke();
                        break;
                }


                break;

            case 1:
                ShiftLeftClick.Invoke();
                break;




        }

    }

    void OnMiddleClick(InputValue value)
    {
        MiddleClick.Invoke();
        switch (value.Get<float>())
        {
            case 1:
                MiddleClickDown.Invoke();
                break;

            case 0:
                MiddleClickUp.Invoke();
                break;
        }
    }

    float LeftClickDrag = 0;
    async void OnLeftClickDrag(InputValue value)
    {

        if(ShiftHeld == 0){
            print("left drag input = " + value.Get<float>());
            LeftClickDrag = value.Get<float>();



            while (LeftClickDrag != 0)
            {
                LeftClickHold.Invoke();
                await Task.Delay(100);
                await Task.Yield();
            }
        }
    }

    void OnShiftLeftClick(InputValue value)
    {
        ShiftLeftClick.Invoke();
        print("shiftclick called");
    }

    void OnShift(InputValue value)
    {
        ShiftHeld = value.Get<float>();
        print("shift called " + value.Get<float>());
    }




    public void SetMouseMode (EditorMouseMode mouseMode)
    {
        UnassignMouseMode.Invoke();
        switch (mouseMode)
        {

            case EditorMouseMode.Select:
                mousemode_Select.EnableMouseMode();
                break;

            case EditorMouseMode.Build:
                mousemode_Cell.EnableMouseMode();
                break;

        }

    }
}
