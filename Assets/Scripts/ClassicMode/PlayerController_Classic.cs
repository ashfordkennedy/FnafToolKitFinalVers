using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classic controller is an object that exists in all Map scenes, Its values are loaded in with the scene and cannot be destroyed. 
/// A playerStart object acts as a proxy, providing a way to alter its position in the editor. The controllers settings can vary between nights.
/// Position is not yet supported.
/// 
/// </summary>
public class PlayerController_Classic : MonoBehaviour
{
    public static PlayerController_Classic instance;
    [SerializeField]private float _rotationLowerBound;
    [SerializeField]private float _rotationUpperBound;

    [SerializeField] private float _currentRotation = 0;
    [SerializeField] private float _rotateSpeedMultiplier = 2;
    [SerializeField] Camera _controllerCamera;


    private Quaternion tempRotation;
    private Vector3 tempPosition;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        SetRotationBounds(90, 90);
    }


    public void BeginNight(Transform startObject, float lowerBound, float upperBound)
    {
        SetTransform(startObject);
        SetRotationBounds(lowerBound, upperBound);


    }



    public void SetTransform(Transform transform)
    {
        this.transform.position = transform.position;
        this.transform.rotation = transform.rotation;
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetRotationBounds(float lowerBound, float upperBound)
    {
        var vector = this.transform.localEulerAngles;
        _rotationLowerBound =  vector.y - lowerBound;
        _rotationUpperBound =  vector.y + upperBound;
    }


    public void RotatePlayer(float amount)
    {
        float speed = amount * _rotateSpeedMultiplier;

        if (_currentRotation > _rotationLowerBound && amount == -1||   _currentRotation < _rotationUpperBound && amount == 1)
        {
            transform.Rotate(new Vector3(0, speed, 0));
            _currentRotation += speed;
        }
    }

    private void SetTempTransform()
    {
        tempRotation = this.transform.rotation;
        tempPosition = this.transform.position;
    }

    private void RestoreTransform()
    {
        this.transform.rotation = tempRotation;
        this.transform.position = tempPosition;
    }
            

    public void BeginPreview(Transform startObject, float lowerBound, float upperBound)
    {
        SetTempTransform();
        SetRotationBounds(lowerBound, upperBound);
        SetTransform(startObject);
        _controllerCamera.gameObject.SetActive(true);

    }

    public void EndPreview()
    {

        RestoreTransform();
        _controllerCamera.gameObject.SetActive(false);
    }
}
