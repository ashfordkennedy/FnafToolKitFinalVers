using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ObjectTransformController : MonoBehaviour
{
    public static ObjectTransformController instance;

    public bool menuOpen = false;
    public InputField[] PositionFields;
    public InputField[] RotationFields;
    public DecorObject TargetTransformObject;
    [SerializeField] Camera EditorCamera;
    public Vector3 Offset;
    public Vector3 ScreenPoint;
    public LayerMask BuildGridMask;
    public LayerMask WidgitMask;
    public RectTransform TransformUi;

    bool snap = false;
    private Vector3 tempPos;
    private Vector3 tempRot;
    Transform axisTarget = null;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        this.gameObject.SetActive(false);
        print("TransformGizmoSet");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete) == true)
        {
            DeleteObject();
        }       
    }

    public void PickObject()
    {
        ObjectPlacementWidgit.PlacementWidgit.FreePlacementMode(TargetTransformObject.gameObject);
    }

    public void MoveObject(AxisMode axis = AxisMode.X, float mouseValue = 0f)
    {
        Transform target = TargetTransformObject.transform;
        switch (axis)
        {
            case AxisMode.X:
                target.position = target.position + new Vector3(mouseValue, 0, 0);
                break;
        }



    }








    public void OpenTransformController(bool open, DecorObject target)
    {
        menuOpen = open;
        switch (open)
        {
            case true:
                TargetTransformObject = target;
                this.gameObject.SetActive(true);
                ObjectTransformController.instance.transform.position = target.transform.position;
                UpdateTransformUI();
                TransformUi.gameObject.SetActive(true);
                break;


            case false:
              //  if (this.gameObject.activeInHierarchy == true)
               // {
                    ObjectTransformController.instance.gameObject.SetActive(false);
                    TargetTransformObject = null;
                    TransformUi.gameObject.SetActive(false);
                SwatchUI.Instance.CloseMenu();
                LightSettingUI.Instance.ToggleLightUI(false);
                if (AnimatronicMenu.instance.MenuOpen == true)
                {
                    AnimatronicMenu.instance.CloseMenu();
                }
              //  }
                break;
        }
    }

    public void CloseMenu()
    {
      //  OpenTransformController(false, null);
    }



    public IEnumerator DisplayTransformUI(bool show)
    {

        
        var NewPos = new Vector2(0, 0);
        float LerpAmount = 0.05f;
        switch (show)
        {
            case true:
                UpdateTransformUI();
                this.gameObject.transform.position = TargetTransformObject.transform.position;
                this.gameObject.SetActive(true);
                
                NewPos = new Vector2(0.5f, 0);


                break;

            case false:
                LightSettingUI.Instance.ToggleLightUI(false);
                SwatchUI.Instance.CloseMenu();
                print("closing transform menu");               
                NewPos = new Vector2(0.5f, 1.5f);

                break;

        }
        yield return new WaitForSeconds(0.01f);
        var currentPos = TransformUi.pivot;
        while (LerpAmount <= 1)
        {
            TransformUi.pivot = Vector2.Lerp(currentPos, NewPos, LerpAmount);
            LerpAmount += 0.1f;
            yield return new WaitForSeconds(0.01f);

        }
        TransformUi.pivot = NewPos;

        switch (show)
        {

            case false:
                this.gameObject.SetActive(false);
                print("closing transform menu");
                
                break;

        }
        yield return null;
    }



    public void UpdateTransformUI()
    {
        var pos = TargetTransformObject.transform.position;
        PositionFields[0].text = "" + pos.x;
        PositionFields[1].text = "" + pos.y;
        PositionFields[2].text = "" + pos.z;


        var rot = TargetTransformObject.transform.rotation.eulerAngles;
        RotationFields[0].text = "" + rot.x;
        RotationFields[1].text = "" + rot.y;
        RotationFields[2].text = "" + rot.z;
    }




    public void DragZ()
    {

        RaycastHit hit;
        Ray ray = EditorCamera.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out hit, Mathf.Infinity, BuildGridMask))
        {
            Transform objectHit = hit.transform;


            float ZOffset = (hit.point.z);
            Mathf.Clamp(ZOffset, 32, 1015);
            print(ZOffset);

            var Newpos = RoundPosition(new Vector3(TargetTransformObject.transform.position.x, TargetTransformObject.transform.position.y, ZOffset), 0.01f);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                Newpos = GridSnap.SnapPosition(Newpos,EditorController.Instance.MapGridScale /2);


            }


            this.gameObject.transform.position = Newpos;
            TargetTransformObject.transform.position = Newpos;

        }
    }

    public void DragX()
    {
        RaycastHit hit;
        Ray ray = EditorCamera.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out hit, Mathf.Infinity, BuildGridMask))
        {
            Transform objectHit = hit.transform;


            float XOffset = (hit.point.x);
            Mathf.Clamp(XOffset, 25, 1015);
           // print(XOffset);

            var Newpos = RoundPosition(new Vector3(XOffset, TargetTransformObject.transform.position.y, TargetTransformObject.transform.position.z), 0.01f);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                Newpos = GridSnap.SnapPosition(Newpos, EditorController.Instance.MapGridScale / 2);


            }


            this.gameObject.transform.position = Newpos;
            TargetTransformObject.transform.position = Newpos;
        }
    }

    public void DragY(Vector3 origin)
    {

        var distance = Input.mousePosition - origin * -1;

        distance = Vector3.Normalize(distance);

         float dis = Vector3.Distance(Input.mousePosition, origin);


        // var offset = (dis * distance.y / 10);
        var offset = Mathf.Clamp((Input.mousePosition.y / Screen.height) * 2 - 1, -1.0f, 1.0f);

        float YOffset = (TargetTransformObject.transform.position.y + offset);
           Mathf.Clamp(YOffset, -10, 50);
            print(dis + "is the current distance");

            var Newpos = RoundPosition(new Vector3(TargetTransformObject.transform.position.x, YOffset, TargetTransformObject.transform.position.z), 0.01f);

            this.gameObject.transform.position = Newpos;
            TargetTransformObject.transform.position = Newpos;
        





    }



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


    public void TextEnterPosition()
    {

        if (PositionFields[0].text != "" && PositionFields[1].text != ""  && PositionFields[2].text != "" || PositionFields[0].text != "-" && PositionFields[1].text != "-" && PositionFields[2].text != "-") {
            var Newpos = new Vector3(float.Parse(PositionFields[0].text), float.Parse(PositionFields[1].text), float.Parse(PositionFields[2].text));

            TargetTransformObject.transform.position = Newpos;
            this.gameObject.transform.position = Newpos;
        }
    }



    public void SetRotation()
    {
        if (RotationFields[0].text != "" && RotationFields[1].text != "" && RotationFields[2].text != "" || RotationFields[0].text != "-" && RotationFields[1].text != "-" && RotationFields[2].text != "-")
        {

            var eulers = TargetTransformObject.transform.eulerAngles;

            var neweuler = new Vector3(float.Parse(RotationFields[0].text), float.Parse(RotationFields[1].text), float.Parse(RotationFields[2].text));


         //  this.transform.eulerAngles = neweuler;
            TargetTransformObject.transform.eulerAngles = neweuler;


        }
    }


    //set fields object name to correct axis
    public void SetPosition(InputField input)
    {
        string Axis = input.gameObject.name;
        float value;

        Vector3 pos = TargetTransformObject.transform.position;

        if (float.TryParse(input.text, out value))
        {

            switch (Axis)
            {
                default:
                    Debug.LogWarning("the input field should be named X Y or Z for the axis you wish to manipulate");
                    break;

                case "X":
                    pos.x = Mathf.Clamp(value,0,455);
                    break;

                case "Y":
                    pos.y = Mathf.Clamp(value, 0, 455);
                    break;

                case "Z":
                    pos.z = value;
                    break;

            }
            TargetTransformObject.transform.position = pos;
        }
    }









    //set fields object name to correct axis
    public void SetRotation(InputField input)
    {
        string Axis = input.gameObject.name;
        float value;

        Vector3 eulers = TargetTransformObject.transform.eulerAngles;

        if (float.TryParse(input.text, out value))
        {

            switch (Axis)
            {
                default:
                    Debug.LogWarning("the input field should be named X Y or Z for the axis you wish to manipulate");
                    break;

                case "X":
                    eulers.x = value;
                    break;

                case "Y":
                    eulers.y = value;
                    break;

                case "Z":
                    eulers.z = value;
                    break;

            }
            TargetTransformObject.transform.eulerAngles = eulers;
        }
    }




    public void CloneObject()
    {
        Instantiate(TargetTransformObject, TargetTransformObject.transform.position, TargetTransformObject.transform.rotation);

    }

    public void DeleteObject()
    {
      if(TargetTransformObject != null)
        {
            TargetTransformObject.DestroyObject();
            Destroy(TargetTransformObject.gameObject);
          //  OpenTransformController(false, null);
            //StartCoroutine("DisplayTransformUI", false);
        }
    }
}
