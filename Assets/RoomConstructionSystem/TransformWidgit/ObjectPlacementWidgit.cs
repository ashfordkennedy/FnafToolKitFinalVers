using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ObjectPlacementWidgit : MonoBehaviour
{
    public static ObjectPlacementWidgit PlacementWidgit;
    public Camera EditorCamera;

    public LayerMask BuildCellLayerMask;
    public LayerMask WallPlacementMask;

    public bool targetRegistered = false;
    private Vector3 returnPosition = new Vector3();
    // Start is called before the first frame update

    private void Awake()
    {
        PlacementWidgit = this;
        this.enabled = false;
    }

    void Start()
    {
        
    }
    void OnRotateLeft()
    {
        this.transform.GetChild(0).transform.eulerAngles = new Vector3(this.transform.GetChild(0).transform.eulerAngles.x, this.transform.GetChild(0).transform.eulerAngles.y - 45, this.transform.GetChild(0).transform.eulerAngles.z);
    }

    void OnRotateRight()
    {
        this.transform.GetChild(0).transform.eulerAngles = new Vector3(this.transform.GetChild(0).transform.eulerAngles.x, this.transform.GetChild(0).transform.eulerAngles.y + 45, this.transform.GetChild(0).transform.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Delete) == true)
        {
            if (this.transform.childCount > 0)
            {
                this.transform.GetChild(0).SendMessage("DestroyObject", null, SendMessageOptions.DontRequireReceiver);
               // Destroy(this.transform.GetChild(0).gameObject);
            }


            ActivateWidgit(false);
        }




        /*
        if (Input.GetKeyDown(KeyCode.LeftBracket)&& this.transform.childCount > 0)
        {
            
        }

        if (Input.GetKeyDown(KeyCode.RightBracket) && this.transform.childCount > 0)
        {
            
        }
        */


        if (MouseOverUICheck() == false && Input.GetKey(KeyCode.LeftControl) != true)
        {


            RaycastHit hit;
            Ray ray = EditorCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, BuildCellLayerMask))
            {
                Transform objectHit = hit.transform;

                var Gridsnap = 0.01f;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    Gridsnap = EditorController.Instance.MapGridScale / 2;

                }
                else if (Input.GetKey(KeyCode.LeftAlt))
                {
                    Gridsnap = EditorController.Instance.MapGridScale / 10;
                }

                var testsnap = GridSnap.SnapPosition(hit.point, Gridsnap);

                var BuildPrintPosition = SnapPosition(hit.point, Gridsnap);
                this.transform.position = (BuildPrintPosition + new Vector3(0, 0.1f, 0));



                if (Input.GetMouseButtonDown(0))
                {

                    this.transform.GetChild(0).gameObject.layer = 12;
                    this.transform.GetChild(0).gameObject.GetComponent<DecorObject>().ObjectSetup();
                    this.transform.DetachChildren();

                    ActivateWidgit(false);
                }
            }
        }

        else if (MouseOverUICheck() == false && Input.GetKey(KeyCode.LeftControl) == true)
        {
            RaycastHit hit;
            Ray ray = EditorCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, WallPlacementMask))
            {
               

                var Gridsnap = 0.01f;
     
                var BuildPrintPosition = GridSnap.FreeSnapPosition<Vector3>(hit.point, Gridsnap);
                this.transform.position = (BuildPrintPosition + new Vector3(0, 0.1f, 0));



                if (Input.GetMouseButtonDown(0))
                {

                    this.transform.GetChild(0).gameObject.layer = 12;
                    this.transform.GetChild(0).gameObject.GetComponent<DecorObject>().ObjectSetup();
                    this.transform.DetachChildren();

                    ActivateWidgit(false);
                }
            }


        }




    }


    // destroys the object currently selected and instantiates the new object onto the widgit

    public void CreateBlueprint(GameObject NewBlueprint)
    {
        if (this.transform.childCount > 0 && targetRegistered == false)
        {
            this.transform.GetChild(0).SendMessage("DestroyObject",null,SendMessageOptions.DontRequireReceiver);
            Destroy(this.transform.GetChild(0).gameObject);
        }
        else if(this.transform.childCount > 0)
        {
            var child = this.transform.GetChild(0);
            child.transform.SetParent(null);
            child.transform.position = returnPosition;

        }


      GameObject newobject = Instantiate(NewBlueprint, this.transform);
        newobject.transform.localPosition = Vector3.zero;
        newobject.transform.rotation = this.transform.rotation;
       newobject.layer = 2;
        ActivateWidgit(true);
    }

    public void FreePlacementMode(GameObject target)
    {
        returnPosition = target.transform.position;
        target.transform.SetParent(this.transform);
        target.transform.localPosition = Vector3.zero;
        target.layer = 2;
        ActivateWidgit(true);
      //  ObjectTransformController.ObjectTransformGizmo.OpenTransformController(false, null);
    }



    public void ActivateWidgit(bool SetActive)
    {
        switch (SetActive)
        {
            case true:
                this.enabled = true;
                EditorController.Instance.Editor_MouseMode = EditorMouseMode.Off;
                break;





            case false:

                EditorController.Instance.Editor_MouseMode = EditorMouseMode.Select;
             //   RoomEditorMouse.Instance.enabled = true;
                this.enabled = false;
                break;


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



bool MouseOverUICheck()
{
    return EventSystem.current.IsPointerOverGameObject();

}


}
