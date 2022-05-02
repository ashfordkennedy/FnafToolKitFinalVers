using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum FP_CursorMode {Default,Locked,Examine }
public class CharacterController_Roam : MonoBehaviour
{
    [SerializeField] Camera FirstPersonCamera;
    [SerializeField] AudioListener audioListener;
    [SerializeField] FP_CursorMode FPCursorMode = FP_CursorMode.Default;

    [SerializeField] Texture2D[] CursorTextures;


    // Private camera Values
    private float h;
    private float v;
    private Vector2 movementInput;
    private Vector3 movement;
    private Vector3 look;
    public float DefaultCameraHeight = 11;
    public AnimationCurve curve;
    [SerializeField] Light FlashLight;
    [SerializeField] float speed = 1f;
    [SerializeField] float TurnSpeed = 10f;
    [SerializeField] Rigidbody RB;
    [SerializeField] float YRotMinimum, YRotMaximum;
    [SerializeField] GameObject FPSTarget;
    public void ActivateController()
    {
        FirstPersonCamera.enabled = true;
        audioListener.enabled = true;
        this.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(CursorTextures[(int)FPCursorMode], new Vector2(0.5f, 0.5f), CursorMode.ForceSoftware);
        Cursor.visible = false;
        this.transform.parent.transform.position = FPSTarget.transform.position;
    }

    public void SetStart()
    {
        this.transform.parent.transform.position = FPSTarget.transform.position;
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
        var input = value.Get<Vector2>();
        movement = new Vector3(input.x, 0, input.y);

       

        /*
        var forward = FirstPersonCamera.transform.forward;
        var right = FirstPersonCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        var CorrectedMovement = forward * input.y + right * input.x;
        movement = CorrectedMovement;
        */

    }


    public void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();

      //  look.x = transform.localEulerAngles.x + value.Get<Vector2>().x * TurnSpeed;
     //   look.y += value.Get<Vector2>().x * TurnSpeed;
       
    }

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {

        MovePlayer();

        if (Input.GetKeyDown(KeyCode.F))
        {
            FlashLight.enabled = !FlashLight.enabled;
        }

    }

    public void MovePlayer()
    {
        if (movement != Vector3.zero)
        {
            var forward = FirstPersonCamera.transform.forward;
            var right = FirstPersonCamera.transform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            var CorrectedMovement = forward * movementInput.y + right * movementInput.x;
            movement = CorrectedMovement;



            movement = movement.normalized * speed * Time.deltaTime;
           // RB.MovePosition(RB.gameObject.transform.position + movement);
            RB.AddForce(movement,ForceMode.VelocityChange);


            //head bob
            Vector3 pos = FirstPersonCamera.transform.localPosition;
            float newY = DefaultCameraHeight * (curve.Evaluate(Time.time));

            FirstPersonCamera.transform.localPosition = new Vector3(pos.x, newY, pos.z);
        }


        h += Input.GetAxis("Mouse Y") * TurnSpeed * Time.deltaTime;
       v += Input.GetAxis("Mouse X") * TurnSpeed * Time.deltaTime;

        h = Mathf.Clamp(h, -90f, 90f);

        while (v < 0f)
        {
            v += 360f;
        }
        while (v >= 360f)
        {
            v -= 360f;
        }
        transform.eulerAngles = new Vector3(-h, v, 0f);






        /*

        //   transform.Rotate(new Vector3(-look.y * Time.deltaTime * TurnSpeed, look.x * Time.deltaTime * TurnSpeed, 0) );
        if (look != Vector3.zero)
        {
            transform.rotation =  Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(-look.y * Time.deltaTime * TurnSpeed, look.x * Time.deltaTime * TurnSpeed, 0));
        }
      //  transform.localEulerAngles = new Vector3(-look.y, look.x, 0);
      */
    }



    public void OnLeftClick()
    {
        Debug.Log("left clicked");
        RaycastHit hit;
        Ray ray = FirstPersonCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 10f))
        {
            hit.transform.gameObject.SendMessage("LeftClick",null,SendMessageOptions.DontRequireReceiver);
        }

    }


}
