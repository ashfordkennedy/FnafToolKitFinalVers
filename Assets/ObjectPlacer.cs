using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum AxisMode { X, Y, Z }
public class ObjectPlacer : EditorMenuAbstract
{
    public static ObjectPlacer instance;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Camera EditorCamera;
    [SerializeField] Transform placementContainer;
    [SerializeField] Transform transformGizmo;
    public bool _freePlacement = false;
    private bool _Snap;
    private float clickHeld = 0;
    private float lastClicked = 5f;
    public float SnapSize = 0.01f;
    public float rotationDegrees { get; private set; }
    [SerializeField]public Material _selectedMaterial;

    [Header("LayerMasks")]
    public LayerMask objectMask;
    public LayerMask BuildCellLayerMask;
    public LayerMask WallPlacementMask;
    public LayerMask GizmoMask;

    private Vector3 returnPosition = new Vector3();
    private Quaternion returnRotation = new Quaternion();
    private void Awake()
    {
        instance = this;
        rotationDegrees = 1f;
        lastClicked = Time.time;
    
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #region Menu_Methods
    public override void OpenMenu()
    {
        base.OpenMenu();
        playerInput.enabled = true;
        transformGizmo.gameObject.SetActive(true);
        TransformMenu.instance.OpenMenu();

    }

     public void SelectObject(DecorObject target)
    {
       // target.EditorSelect(_selectedMaterial);
        _freePlacement = false;
        PlaceObjects();
        transformGizmo.position = target.gameObject.transform.position;
        placementContainer.position = target.gameObject.transform.position;
        AddToSelected(target);
        TransformMenu.instance.SetSelectCounter();
    }

    public override void CloseMenu()
    {
        base.CloseMenu();
        playerInput.enabled = false;
        transformGizmo.gameObject.SetActive(false);
        PlaceObjects();
        if (TransformMenu.instance.MenuOpen == true)
        {
            TransformMenu.instance.CloseMenu();
        }
    }
    #endregion


    /// <summary>
    /// add the target to the selected objects. Called when instantiating new objects.
    /// </summary>
    /// <param name="target"></param>
    public void AddToSelected(DecorObject target)
    {
        target.EditorSelect(_selectedMaterial);
        if (target.transform.IsChildOf(placementContainer))
            {
                target.transform.SetParent(null);
            }
            else
            {
                target.transform.SetParent(placementContainer);
            //target.transform.localPosition = new Vector3(0, target.transform.localPosition.y, 0);
             
        // Untag me later you stupid cunt 
          target.gameObject.layer = 2;
            }

        TransformMenu.instance.SetSelectCounter();
    }


    public void CloneObject()
    {
        GameObject cloneObject = placementContainer.GetChild(0).gameObject;
        PlaceObjects();
        SpawnNewObject(cloneObject,true);

    }



    /// <summary>
    /// called by the object menus to instantiate the selected object into the scene.
    /// </summary>
    /// <param name="target"></param>
    public void SpawnNewObject(GameObject target)
    {
        Debug.LogWarning("This spawn method has not yet been configured to handle spawning while already active");
        GameObject newobject = Instantiate(target, placementContainer);
        newobject.transform.localPosition = Vector3.zero;
        newobject.transform.rotation = this.transform.rotation;
        newobject.layer = 2;
        TransformMenu.instance.SetSelectCounter();
        if (MenuOpen == false)
        {
            OpenMenu();
        }
    }


    /// <summary>
    /// called by the object menus to instantiate the selected object into the scene.
    /// </summary>
    /// <param name="target"></param>
    public void SpawnNewObject(GameObject target = null, bool enableFreePlacement = false)
    {
        _freePlacement = enableFreePlacement;
        Debug.LogWarning("This spawn method has not yet been configured to handle spawning while already active");
        GameObject newobject = Instantiate(target, placementContainer);
        newobject.transform.localPosition = Vector3.zero;
        newobject.transform.rotation = this.transform.rotation;
        newobject.layer = 2;
        TransformMenu.instance.SetSelectCounter();
        if (MenuOpen == false)
        {
            OpenMenu();
        }
    }


    /// <summary>
    /// detaches objects from placement container and makes them selectable again
    /// </summary>
    public void PlaceObjects()
    {
        print("place object called");
        while ( placementContainer.childCount > 0)
        {
            var child = placementContainer.GetChild(0);
            child.SetParent(null);
            child.gameObject.layer = 12;

            DecorObject decor = child.GetComponent<DecorObject>();
            decor.ObjectSetup();
            decor.EditorDeselect();
        }
    }

    public void PlaceObject(DecorObject target)
    {
        print("place object called");  
            target.transform.SetParent(null);
            target.gameObject.layer = 12;
           
            target.ObjectSetup();
            target.EditorDeselect(); 
        if(placementContainer.childCount == 0)
        {
            CloseMenu();
        }
        else
        {
            TransformMenu.instance.SetSelectCounter();
        }
    }



    public void DeleteObjects()
    {
        foreach (Transform child in placementContainer)
        {
            print("called destroy on child " + child.name);
            child.GetComponent<DecorObject>().DestroyObject();
            
            
        }
        TransformMenu.instance.CloseMenu();
    }

    public void SetFreePlacement()
    {
        _freePlacement = !_freePlacement;
    }

    public void SetRotationDegrees(float value)
    {
        rotationDegrees = value;
    }

    public void SetSnapValue(float value)
    {
       SnapSize = value;
    }


    #region Inputs
    void OnRotateLeft()
    {
        placementContainer.eulerAngles = new Vector3(placementContainer.eulerAngles.x, placementContainer.eulerAngles.y - rotationDegrees, placementContainer.eulerAngles.z);
        TransformMenu.instance.SetFields(placementContainer);
    }

    void OnRotateRight()
    {
       placementContainer.eulerAngles = new Vector3(placementContainer.eulerAngles.x, placementContainer.eulerAngles.y + rotationDegrees, placementContainer.eulerAngles.z);
        TransformMenu.instance.SetFields(placementContainer);
    }

    void OnDeleteObject(){
        if (placementContainer.childCount > 0){
            DeleteObjects();
           // placementContainer.GetChild(0).SendMessage("DestroyObject", null, SendMessageOptions.DontRequireReceiver);      
        }
   
    }

    void OnGridSnap(InputValue value)
    {
        switch (value.Get<float>())
        {
            case 1:
                _Snap = false;
                break;

            case 2:
                _Snap = true;
                break;
        }
    }

    
    void OnClick(InputValue value)
    {
        print("On click has been pressed, over ui result = " + MouseOverUICheck());
        
        if (placementContainer.childCount != 0 && _freePlacement == true && MouseOverUICheck() == false)
        {
            print("On click has been pressed");
           // PlaceObjects();
            TransformMenu.instance.SetFields(placementContainer);
            CloseMenu();
        }

        else
        {

        }
        
    }
    

    void OnMouseMove(InputValue value)
    {
       if (MouseOverUICheck() == false && _freePlacement == true)
        {
       // print("mouse move triggered");
            RaycastHit hit;
            Ray ray = EditorCamera.ScreenPointToRay(Input.mousePosition);

            switch (_Snap)
            {
                case false:
                    //raycast to floor
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, BuildCellLayerMask))
                    {
                        Transform objectHit = hit.transform;
                   // print("raycast hitting floor");
                    var BuildPrintPosition = GridSnap.SnapPosition(hit.point, SnapSize);
                       
                        //offset upwards to prevent floor trapping.
                       placementContainer.position = (BuildPrintPosition + new Vector3(0, 0.1f, 0));
                    }
                        break;

                case true:
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, WallPlacementMask))
                    {
                        var BuildPrintPosition = GridSnap.FreeSnapPosition<Vector3>(hit.point, SnapSize);
                        placementContainer.position = (BuildPrintPosition + new Vector3(0, 0.1f, 0));
                    }
                        break;
                    
            }
            transformGizmo.position = placementContainer.position;
            TransformMenu.instance.SetFields(placementContainer);
      //  print("switch cleared");
         }
    }



    /// <summary>
    /// select other objects
    /// </summary>
    void OnSelectObject()
    {
        RaycastHit hit;
        Ray ray = EditorCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, objectMask) && Time.time > lastClicked + 0.2f)
        {
            lastClicked = Time.time;
            Transform objectHit = hit.transform;
            var target = objectHit.transform.GetComponent<DecorObject>();

            switch (target.selected)
            {
                case true:
                PlaceObject(target);
                    break;
                case false:
                AddToSelected(target);
                    break;
            }
           
        }
            
    }

    void OnClickHold(InputValue value)
    {
        clickHeld = value.Get<float>();
        print(clickHeld);

        if (clickHeld == 1)
        {
            //find drag targetRaycastHit hit;
            RaycastHit hit;
            Ray ray = EditorCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, GizmoMask))
            {
                AxisMode axis = AxisMode.X;
                print("yall hit a gizmo it's working!");
                switch (hit.transform.name)
                {
                    case"X":
                        axis = AxisMode.X;
                        break;
                    case "Y":
                        axis = AxisMode.Y;
                        break;
                    case "Z":
                        axis = AxisMode.Z;
                        break;
                }



                StartCoroutine(ClickHold(axis));


            }
        }
    }


    IEnumerator ClickHold(AxisMode axisMode)
    {
        print("click held");
        while (clickHeld != 0)
        {
            //drag loop 

            RaycastHit hit;
            Ray ray = EditorCamera.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out hit, Mathf.Infinity, BuildCellLayerMask))
            {
                Transform objectHit = hit.transform;
                var Newpos = new Vector3();
                switch (axisMode)
                {

                    case AxisMode.X:
                        float XOffset = (hit.point.x);
                        Mathf.Clamp(XOffset, 25, 1015);
                        Newpos = RoundPosition(new Vector3(XOffset, placementContainer.position.y, placementContainer.position.z), 0.01f);
                        break;

                    case AxisMode.Y:

                        break;
                  
                    case AxisMode.Z:
                        float zOffset = (hit.point.z);
                        Mathf.Clamp(zOffset, 32, 1015);
                        Newpos = RoundPosition(new Vector3(placementContainer.position.x, placementContainer.position.y, zOffset), 0.01f);
                        break;


                }

                transformGizmo.position = Newpos;
                placementContainer.position = Newpos;
            }

            yield return null;
        }
            print("click released");
            yield return null;
    }
    
        #endregion




        Vector3 RoundPosition(Vector3 input, float factor = 0.01f)
        {
            if (factor <= 0f)
                throw new UnityException("factor argument must be above 0");

            float x = Mathf.Round(input.x / factor) * factor;
            //float y = Mathf.Round(input.y / factor) * factor;
            float y = Mathf.Round(input.y / factor) * factor;
            float z = Mathf.Round(input.z / factor) * factor;

            //  Debug.Log(new Vector3(x, y, z));
            return new Vector3(x, y, z);
        }
    }
