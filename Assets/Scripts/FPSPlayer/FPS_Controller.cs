using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class FPS_Controller : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] LookController lookController;
    Vector3 input;
    Vector3 movement;


    [SerializeField] Camera _targetCamera;
    [SerializeField] bool isMoving = false;
    [SerializeField] bool wasMoving = false;
    [SerializeField] bool isWalking = false;


    [SerializeField] float Speed = 5f;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float runStepLengthen = 0.7f;
    [SerializeField] float jumpSpeed = 10;
    [SerializeField] float groundForce = 10f;
    [SerializeField] float gravityMultiplier = 2f;
    [SerializeField] bool jumping = false;
    [SerializeField] bool _previouslyGrounded;
    [SerializeField] bool _grounded = true;
    [SerializeField] float _StepInterval = 0.3f;
    [SerializeField] float _StepCycle = 0f;
    [SerializeField] float _nextStep = 0f;

    [SerializeField] private HeadBob _headBob = new HeadBob();
    [SerializeField] private JumpBob _jumpBob = new JumpBob();
    [SerializeField] private FovKick _fovKick = new FovKick();
    Vector3 newCameraPosition;
    Vector3 originalCameraPosition;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] movementSounds;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip landSound;
    // Start is called before the first frame update
    void Start()
    {
        lookController.Initialise(this.transform, _targetCamera.transform);
        _headBob.Setup(_targetCamera, _StepInterval);
        _fovKick.Setup(_targetCamera);
        _nextStep = _StepCycle / 2;
        originalCameraPosition = _targetCamera.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_previouslyGrounded && characterController.isGrounded)
        {
            print("starting bob cycle");
            StartCoroutine(_jumpBob.DoBobCycle());
            PlaySound(landSound);
            jumping = false;

            
        }
        _previouslyGrounded = characterController.isGrounded;
    }




    void OnJump(InputValue value)
    {
        if (characterController.isGrounded == true)
        {
            print("jumping");
            jumping = true;
          //  _rigidbody.AddForce(_rigidbody.velocity + (Vector3.up * jumpSpeed), ForceMode.Impulse);
          //  StartCoroutine(JumpLoop());
        }

        if(characterController.isGrounded && jumping == false)
        {

        }


    }


    private void FixedUpdate()
    {
        Move(input);
    }

    void OnLook(InputValue value)
    {
        var _lookMovement = value.Get<Vector2>();
        lookController.LookRotation(this.transform, _targetCamera.transform, _lookMovement);
        lookController.InteractionRaycast(_targetCamera);
    }


    void OnMove(InputValue value)
    {
        //Debug.Log("Movement from keyboard detected!!");
        var tempInput = value.Get<Vector2>();
        input = new Vector3(tempInput.x, tempInput.y, 0);
        wasMoving = isWalking;
        isWalking = (input != Vector3.zero ? true : false);

       StopAllCoroutines();
        StartCoroutine(isWalking ? _fovKick.FovKickUp() : _fovKick.FovKickDown());
    }

    void OnRun(InputValue value)
    {
       var tempValue = value.Get<float>();
        print(tempValue);
        Speed = (tempValue == 0 ? walkSpeed : runSpeed);
    }


    private void Move(Vector3 input)
    {

        // object forward
        Vector3 correctedForward = transform.forward * input.y + transform.right * input.x;

        // get surface normal
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo, characterController.height / 2, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        correctedForward = Vector3.ProjectOnPlane(correctedForward, hitInfo.normal).normalized;
        movement.x = correctedForward.x * Speed;
        movement.z = correctedForward.z * Speed;


        // account for jumping
        if(characterController.isGrounded == true)
        {
            movement.y = -groundForce;

            if (jumping)
            {
                movement.y = jumpSpeed;
                PlaySound(jumpSound);
                jumping = false;
            }

        }

        else
        {
            movement += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
        }

       



        characterController.Move(movement * Time.fixedDeltaTime);
        if(movement != Vector3.zero)
        {
          
        }


        //step cycle
       // ProgressStepCycle(Speed);
        updateCameraPosition(Speed);


    }


    void updateCameraPosition(float speed)
    {
        Vector3 newPosition = originalCameraPosition;

        if(characterController.velocity.magnitude > 0 && characterController.isGrounded == true)
        {
            newPosition = _headBob.GenerateOffset(speed);
            print(newPosition);
            _targetCamera.transform.localPosition = newPosition;
        }

        else
        {
            newPosition.y = originalCameraPosition.y - _jumpBob.Offset();
            _targetCamera.transform.localPosition = newPosition;
        }
       

    }


    void PlayWalkSound()
    {
        int id = Random.Range(0, movementSounds.Length - 1);
        PlaySound(movementSounds[id]);

    }


    void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }



    void ProgressStepCycle(float speed)
    {
        if(characterController.velocity.magnitude > 0 &&( input.x != 0 || input.y != 0))
        {

            _StepCycle += (characterController.velocity.magnitude + (speed * (isWalking ? 1f : runStepLengthen))) * Time.fixedDeltaTime;
            
        }


    }
}
