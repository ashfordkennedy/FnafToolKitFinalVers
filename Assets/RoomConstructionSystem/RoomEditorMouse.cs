using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Processing for mouse Interactions with the world
/// </summary>
public class RoomEditorMouse : MonoBehaviour
{
    public EditorMouseMode mouseMode = EditorMouseMode.Select;
    [SerializeField] private List<Texture2D> _mouseIcons = new List<Texture2D>();

    public static RoomEditorMouse Instance;
    
    public GameObject RoomCellPrefab;
    public GameObject CellBuildingCursor;

    private float lastClicked = 3;

    // Private camera Values
    private float h;
    private float v;

    [Space]
    [Header("Layer Masks")]
    public LayerMask BuildCellLayerMask;
    public LayerMask FloorLayerMask;
    public LayerMask WallLayerMask;

    [Space]
    [Header("Camera Values")]
    public Transform cameraContainer;
    public Camera EditorCamera;
    [SerializeField] float CameraTurnSpeed;
    [SerializeField] float CameraMoveSpeed;
    [SerializeField] float CameraScrollSpeed;
    [Space]
    [Header("Build Settings")]
    public bool CanBuild = false;
    public Color BuildableColour;
    public Color BlockedColour;
    public Material BuildBlockMat;
    public Material SelectionMaterial;
    public ParticleSystem BuildParticles;

    public GameObject LastSelectedObject;


    public ObjectTransformController TransformController;
    [SerializeField] ToggleGroup buildToolToggleGroup;


    /// <summary>
    /// tempTarget is storage for objects containing a class you wish to set a target of
    /// </summary>
    public GameObject tempTarget = null;


    [SerializeField]private GameObject _selectTarget = null;
    public GameObject SelectTarget
    {
       get {return _selectTarget; }
        set { _selectTarget = value;
            Debug.Log("setting selected");
        }
    }






    public void Awake()
    {
        Instance = this;
        lastClicked = Time.time;
    }



    public void ToggleActive(bool SetActive = false)
    {
        this.enabled = SetActive;

    }


    public void OnRotateCamera(InputValue value)
    {
        Vector2 axis = value.Get<Vector2>();
        print("rotating, value is " + axis);

            if (axis.x != 0 || axis.y != 0)
            {          


                var vertical = (Vector3.forward * axis.y);
                vertical.y = 0;

                var horizontal = (Vector3.right * axis.x);
                horizontal.y = 0;

                cameraContainer.Translate(vertical * CameraMoveSpeed, Space.Self);

                cameraContainer.Translate(horizontal * CameraMoveSpeed, Space.Self);
                Vector3 optimisedPos = cameraContainer.transform.position;
                optimisedPos.y = 0;
                cameraContainer.transform.position = optimisedPos;
            }
        
        
    }

    private void Update()
    {
        ///zooming controls
        if (Input.mouseScrollDelta.y != 0 || Input.GetAxis("ArrowVertical") != 0 && Input.GetKey(KeyCode.RightControl))
        {
            //  EditorCamera.transform.Translate((EditorCamera.transform.position - cameraContainer.position) * Input.mouseScrollDelta.y * Time.deltaTime, Space.Self);
            float value = Input.mouseScrollDelta.normalized.y + Input.GetAxis("ArrowVertical");
            if (Input.mouseScrollDelta.y < 0 || Input.GetAxis("ArrowVertical") < 0)
            {
                EditorCamera.transform.Translate(EditorCamera.transform.forward * CameraScrollSpeed * value * Time.deltaTime, Space.World);

            }
            else if (Vector3.Distance(EditorCamera.transform.position,cameraContainer.transform.position) > 5f)
            {
                EditorCamera.transform.Translate(EditorCamera.transform.forward * CameraScrollSpeed * value * Time.deltaTime, Space.World);
            }
            else
            {
                Debug.LogWarning("camera limit met");
            }
           
           // var newpos = EditorCamera.transform.position;
           // var y = Mathf.Clamp(newpos.y, 2, 50);
           // newpos.y = y;
          //  EditorCamera.transform.position = newpos;
            


            //EditorCamera.transform.position = Vector3.MoveTowards(EditorCamera.transform.position, cameraContainer.position, CameraScrollSpeed * Input.mouseScrollDelta.y);
        }

        // Camera movement
        if (Input.GetMouseButton(2) && MouseOverUICheck() == false || Input.GetKey(KeyCode.RightShift) && MouseOverUICheck() == false)
        {
            v += Input.GetAxis("Mouse Y") * CameraTurnSpeed * Time.deltaTime;
            h += Input.GetAxis("Mouse X") * CameraTurnSpeed * Time.deltaTime;

            v = Mathf.Clamp(v, -35f, 48f);
           
            while (h < 0f)
            {
                h += 360f;
            }
            while (h >= 360f)
            {
                h -= 360f;
            }  
            


            cameraContainer.transform.eulerAngles = new Vector3(v, h, 0f);


            // cameraContainer.transform.eulerAngles = new Vector3(v, h, 0f);          
            //EditorCamera.transform.eulerAngles = new Vector3(h, v, 0f);

           var rot = new Vector2(Input.GetAxis("Horizontal") + Input.GetAxis("ArrowHorizontal"), Input.GetAxis("Vertical") + Input.GetAxis("ArrowVertical"));


            if (rot != Vector2.zero)
            {
                // var CurentPos = EditorCamera.transform.position;
                // EditorCamera.transform.position = new Vector3(CurentPos.x + Input.GetAxis("Horizontal"), CurentPos.y + Input.GetAxis("Vertical"));

                var vertical = (Vector3.forward * rot.y);
                vertical.y = 0;

                var horizontal = (Vector3.right * rot.x);
                horizontal.y = 0;

                cameraContainer.Translate( vertical * CameraMoveSpeed, Space.Self);

                cameraContainer.Translate(horizontal * CameraMoveSpeed, Space.Self);
                Vector3 optimisedPos = cameraContainer.transform.position;
                optimisedPos.y = 0;
                cameraContainer.transform.position = optimisedPos;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenuController.instance.ToggleMenu();
        }



        if (MouseOverUICheck() == false)
        {


            switch (mouseMode)
            {

                

                case EditorMouseMode.Select:
                    SelectCellHandler();
                    break;


                case EditorMouseMode.Build:
                    BuildCellHandler();

                    break;

                case EditorMouseMode.Erase:
                    EraseCellHandler();

                    break;



                    ///these guys arent needed now. remove whenever
                case EditorMouseMode.Door:
                    WallTypeClickHandler(WallType.Door);
                   
                    break;

                case EditorMouseMode.Wall:
                    WallTypeClickHandler(WallType.Blank);
                 
                    break;

                case EditorMouseMode.Window:
                    WallTypeClickHandler(WallType.Window);
                   
                    break;

                case EditorMouseMode.DoorCenter:
                    WallTypeClickHandler(WallType.DoorCenter);
                    
                    break;

                case EditorMouseMode.DoorLeft:
                    
                    WallTypeClickHandler(WallType.DoorLeft);
                    break;

                case EditorMouseMode.DoorRight:
                   
                    WallTypeClickHandler(WallType.DoorRight);
                    break;

                case EditorMouseMode.waypointSelect:
                    SelectWaypointHandler();

                    break;

                case EditorMouseMode.ObjectSelect:
                    SelectTargetObjectHandler();

                    break;

                case EditorMouseMode.ActionSelect:
                    ActionSelectHandler();
                    break;


            }

        }
    }

    private void ActionSelectHandler()
    {
        RaycastHit hit;
        Ray ray = EditorCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && Input.GetMouseButtonDown(0))
        {

            DecorObject newTarget = null;
            switch (hit.transform.gameObject.layer)
            {
                // decor Object
                case 12:
                    if ((newTarget = hit.transform.gameObject.GetComponentInParent<DecorObject>()) != null)
                    {
                        ObjectActionsMenu.instance.SetActionSelectTarget(newTarget);
                        ChangeMouseMode(3);
                    }

                    break;
            }
        }
    }
    

    void SelectWaypointHandler()
    {
        RaycastHit hit;
        Ray ray = EditorCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && Input.GetMouseButtonDown(0))
        {

            AnimatronicWaypoint target = null;
            switch (hit.transform.gameObject.layer)
            {
                // decor Object
                case 12:
                    if ((target = hit.transform.gameObject.GetComponentInParent<AnimatronicWaypoint>()) != null)
                    {
                       // print(target.name + "successfully hit waypoint object");
                        AnimatronicMenu.instance.AddWaypoint(hit.transform.gameObject.GetComponentInParent<AnimatronicWaypoint>());
                        ChangeMouseMode(3);
                    }
                    break;
            }
        }
    }



    void SelectTargetObjectHandler()
    {
        RaycastHit hit;
        Ray ray = EditorCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && Input.GetMouseButtonDown(0))
        {

           
            switch (hit.transform.gameObject.layer)
            {
               
                // decor Object
                case 12:
                    DecorObject target = hit.transform.gameObject.GetComponentInParent<DecorObject>();
                    AnimatronicWaypoint output;
                    if (target.gameObject.TryGetComponent<AnimatronicWaypoint>(out output)== false)
                    {
                        print(target.name + "successfully hit waypoint object");
                        tempTarget.SendMessage("SetObjectTarget", target, SendMessageOptions.DontRequireReceiver);
                        ChangeMouseMode(3);
                    }
                    break;
            }
        }
    }
















    void SelectCellHandler()
    {
        

        RaycastHit hit;
        Ray ray = EditorCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && Input.GetMouseButtonDown(0))
        {
            //restore if needed
            /*
            DeselectLastObject();
            LastSelectedObject = SelectTarget;

            if(LastSelectedObject != null)
            {
                LastSelectedObject.SendMessage("EditorDeselect", null, SendMessageOptions.DontRequireReceiver);
                LastSelectedObject = null;

            }
          */

            Transform objectHit = hit.transform;

            

            switch (hit.transform.gameObject.layer)
            {
                case 0:
                    hit.transform.SendMessage("EditorSelect", null, SendMessageOptions.DontRequireReceiver);
                    break;


                // FLoors
                case 8:

                    
                   //EditorController.Instance.NewRoomSelectHandler(hit.transform.gameObject.GetComponent<RoomCell>().Room_Ctrl);
                   // Target.Room_Ctrl.EditorHighlight();

                    break;

                //wall
                case 9:
                  //  EditorController.Instance.NewRoomSelectHandler(hit.transform.gameObject.GetComponent<WallComponent>().Room_Cell.Room_Ctrl);
                    break;

                    // decor Object
                case 12:
                  //  hit.transform.SendMessageUpwards("EditorSelect", null, SendMessageOptions.DontRequireReceiver);
                    break;


                    //combined room mesh
                case 13:

                    EditorController.Instance.NewRoomSelectHandler(hit.transform.gameObject.GetComponentInParent<RoomController>());
                    break;
            }


        }
    }




    /// <summary>
    /// Sets selected wall components type to desired state
    /// </summary>
    /// <param name="WallSetting"></param>
    void WallTypeClickHandler(WallType WallSetting = WallType.Blank)
    {
        RaycastHit hit;
        Ray ray = EditorCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, WallLayerMask) && Input.GetMouseButtonDown(0))
        {
            Transform objectHit = hit.transform;
            AudioManager.Audio_M.PlayBuildSound();
            hit.transform.gameObject.GetComponent<WallComponent>().SetWallType(WallSetting);

        }


    }

    void EraseCellHandler()
    {

        RaycastHit hit;
        Ray ray = EditorCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity,FloorLayerMask))
        {
            Transform objectHit = hit.transform;

            var BuildPrintPosition = SnapPosition(hit.point, EditorController.Instance.MapGridScale);
            CellBuildingCursor.transform.position = (BuildPrintPosition + new Vector3(0, 0.1f, 0));

            CanBuild = CellBuildableCheck();


            if (Input.GetMouseButton(0) && CellErasableCheck() == true && lastClicked <= Time.time - 0.05f)
            {
                lastClicked = Time.time;
                //EditorController.Instance.DeRegisterRoom(EditorController.Instance.SelectedRoom);
                print(hit.collider.name + "was marked for erasure" );
                hit.collider.gameObject.GetComponent<RoomCell>().EraseCell();
            }

        }
    }


    /// <summary>
    /// Change Mouse Actions. Uses an int to change enum value (UI Event Compatibility)
    /// </summary>
    /// <param name="newMouseMode"></param>
    public void ChangeMouseMode(int newMouseMode)
    {
      /*  if (ObjectTransformController.ObjectTransformGizmo.isActiveAndEnabled == true)
        {
            ObjectTransformController.ObjectTransformGizmo.StartCoroutine("DisplayTransformUI", false);
        }*/


        if(ObjectPlacer.instance.MenuOpen == true)
        {
            ObjectPlacer.instance.CloseMenu();
        }



        this.mouseMode = (EditorMouseMode)newMouseMode;
       // print("edtior mousemode now set to " + (EditorMouseMode)newMouseMode);

        DeselectLastObject();

        CellBuildingCursor.gameObject.SetActive(false);

      //  Cursor.SetCursor(_mouseIcons[newMouseMode], Vector2.zero, CursorMode.ForceSoftware);
        ///performs specific closing actions, may not be needed
        switch (this.mouseMode)
        {
            case EditorMouseMode.Select:
                this.mouseMode = EditorMouseMode.Select;
                
                break;

            case EditorMouseMode.Build:
                CellBuildingCursor.gameObject.SetActive(true);
                this.mouseMode = EditorMouseMode.Build;

                break;

            case EditorMouseMode.Erase:
                CellBuildingCursor.gameObject.SetActive(true);
                this.mouseMode = EditorMouseMode.Erase;

                break;

            case EditorMouseMode.NewRoom:
                EditorController.Instance.SelectedRoom = null;
                CellBuildingCursor.gameObject.SetActive(true);
                RoomSettingsUI.Instance.CloseMenu();
                this.mouseMode = EditorMouseMode.Build;

                break;

            case EditorMouseMode.Door:
                this.mouseMode = EditorMouseMode.Door;
                break;


            case EditorMouseMode.DoorLeft:
                this.mouseMode = EditorMouseMode.DoorLeft;
                break;

            case EditorMouseMode.DoorRight:
                this.mouseMode = EditorMouseMode.DoorRight;
                break;

            case EditorMouseMode.waypointSelect:
              //  this.mouseMode = EditorMouseMode.waypointSelect;
                break;

        }


    }

    /// <summary>
    /// Handles the updater method for adding Room cells to the currently selected room or creating a brand new room
    /// </summary>
    void BuildCellHandler() {



            RaycastHit hit;
            Ray ray = EditorCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, BuildCellLayerMask))
            {
                Transform objectHit = hit.transform;

                var BuildPrintPosition = SnapPosition(hit.point, EditorController.Instance.MapGridScale);
                CellBuildingCursor.transform.position = (BuildPrintPosition + new Vector3(0, 0.1f, 0));

                CanBuild = CellBuildableCheck();

            switch (objectHit.gameObject.layer)
            {
                //build grid
                case 11:

                    if (Input.GetMouseButton(0) && CanBuild == true && lastClicked <= Time.time - 0.05f)
                    {
                        lastClicked = Time.time;
                        AudioManager.Audio_M.PlayBuildSound();
                        RoomCell NewCell = Instantiate(RoomCellPrefab).GetComponent<RoomCell>();

                        NewCell.gameObject.transform.position = BuildPrintPosition;
                        BuildParticles.transform.position = BuildPrintPosition;
                        BuildParticles.Play();




                        //What the absolute fuck was I thinking, just use the damn prefab. good god, you idiot!!!

                        // build a new room, none are selected. Sets up room controller objects
                        if (EditorController.Instance.SelectedRoom == null)
                        {
                            DeselectLastObject();


                            int Xdistance = (int)NewCell.transform.localPosition.x / EditorController.Instance.MapGridScale;
                            int Zdistance = (int)NewCell.transform.localPosition.z / EditorController.Instance.MapGridScale;
                            // Debug.Log("Distance from origin is x = " + Xdistance + " Z = " + Zdistance);
                            NewCell.CellID = new Vector2Int(Xdistance, Zdistance);

                            EditorController.Instance.CreateRoom(NewCell);
                        }


                        // just instantiates and adds to room
                        else
                        {
                            NewCell.Room_Ctrl = EditorController.Instance.SelectedRoom;
                            NewCell.transform.SetParent(EditorController.Instance.SelectedRoom.EditModeContainer.transform, true);

                            var RoomCenter = EditorController.Instance.SelectedRoom.transform.localPosition;
                            int Xdistance = (int)NewCell.transform.localPosition.x / EditorController.Instance.MapGridScale;
                            int Zdistance = (int)NewCell.transform.localPosition.z / EditorController.Instance.MapGridScale;
                            //  Debug.Log("Distance from origin is x = " + Xdistance + " Z = " + Zdistance);
                            NewCell.CellID = new Vector2Int(Xdistance, Zdistance);

                        }
                        NewCell.CellInitialize();
                    }

                    break;

                    // floor
                case 8:

                    if(Input.GetMouseButton(0) && CanBuild == false && lastClicked <= Time.time - 0.05f)
                    {

                        lastClicked = Time.time;
                        AudioManager.Audio_M.PlayBuildSound();
                        RoomCell NewCell = Instantiate(RoomCellPrefab).GetComponent<RoomCell>();

                        NewCell.gameObject.transform.position = BuildPrintPosition;
                        BuildParticles.transform.position = BuildPrintPosition;
                        BuildParticles.Play();


                    }



                    break;


            }
            



            }
        }


       
        

    /// <summary>
    /// Used to generate Grid safe positions for placement of room cells.
    /// </summary>
    /// <param name="input">Current position.</param>
    /// <param name="factor">Grid unit size.</param>
    /// <returns></returns>
    Vector3 SnapPosition(Vector3 input, float factor = 1f)
    {
        if (factor <= 0f)
            throw new UnityException("factor argument must be above 0");

        float x = Mathf.Round(input.x / factor) * factor;
        //float y = Mathf.Round(input.y / factor) * factor;
        float y = 0;
        float z = Mathf.Round(input.z / factor) * factor;

        //  Debug.Log(new Vector3(x, y, z));
        return new Vector3(x, y, z);
    }



    public void DeselectLastObject()
    {
        if (SelectTarget != null)
        {
            LastSelectedObject = RoomEditorMouse.Instance.SelectTarget;
            LastSelectedObject.SendMessage("EditorDeselect", null, SendMessageOptions.DontRequireReceiver);
          //  ObjectTransformController.ObjectTransformGizmo.OpenTransformController(false, null);
            LastSelectedObject = null;
        }


    }








    bool CellBuildableCheck()
    {
        RaycastHit hits;
        Ray CellRay = new Ray((CellBuildingCursor.transform.position + new Vector3(0,0.1f,0)), Vector3.down);
       // Ray CellRay = EditorCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(CellRay, out hits, Mathf.Infinity, FloorLayerMask))
        {
          //  print(hits.collider.gameObject.name + hits.collider.gameObject.layer);
          //  print("cant build");
            BuildBlockMat.SetColor("_Color", BlockedColour);
            return false;
           
        }
        
        else
        {
          //  print("can build");
            BuildBlockMat.SetColor("_Color", BuildableColour);
            return true;
           
        }
        
    }

    
    public IEnumerator CenterCamera(Vector3 NewPos)
    {
        NewPos.y = EditorCamera.gameObject.transform.parent.position.y;
        var CameraPos = EditorCamera.gameObject.transform.parent.position;
        float LerpAmount = 0.05f;
    
      //  EditorCamera.gameObject.transform.eulerAngles= );

        while (LerpAmount <= 1)
        {          
            EditorCamera.gameObject.transform.parent.position = Vector3.Lerp(CameraPos, NewPos, LerpAmount);
      //  EditorCamera.gameObject.transform.eulerAngles = Vector3.Lerp(EditorCamera.gameObject.transform.eulerAngles, new Vector3(65, 0, 0), LerpAmount);
            LerpAmount += 0.05f;
            yield return new WaitForSeconds(0.01f);         
        }

      //  disable select highlight
        yield return new WaitForSeconds(0.01f);
        if (SelectTarget != null)
        {
            SelectTarget.SendMessage("EditorDeselect", null, SendMessageOptions.DontRequireReceiver);
        }
        // EditorController.Instance.SelectedRoom.

        // yield return new WaitUntil(() => IsMoving == false);
        // yield return new WaitForSeconds(UnityEngine.Random.Range(2, 20));
        yield return null;
    }




    bool CellErasableCheck()
    {
        RaycastHit hits;
        Ray CellRay = new Ray((CellBuildingCursor.transform.position + new Vector3(0, 0.1f, 0)), Vector3.down);
        if (Physics.Raycast(CellRay, out hits, Mathf.Infinity, FloorLayerMask))
        {
            //  print("can build");
            BuildBlockMat.SetColor("_Color", BuildableColour);
            return true;

        }

        else
        {
           

            print(hits.collider.gameObject.name + hits.collider.gameObject.layer);
            //  print("cant build");
            BuildBlockMat.SetColor("_Color", BlockedColour);
            return false;
        }

    }

    bool MouseOverUICheck()
    {
        return EventSystem.current.IsPointerOverGameObject();

    }



}
