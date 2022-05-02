using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Classic : MonoBehaviour
{
    public static PlayerController_Classic instance;
    private Vector3 _rotationLowerBound;
    private Vector3 _rotationUpperBound;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        SetRotationBounds(-90, 90);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRotationBounds(float lowerBound, float upperBound)
    {
        var vector = this.transform.rotation.eulerAngles;
        _rotationLowerBound = new Vector3(vector.x, vector.y - lowerBound, vector.z);
        _rotationUpperBound = new Vector3(vector.x, vector.y + upperBound, vector.z);
    }


    public void RotatePlayer(float amount)
    {
        var rotation = amount;
       // if (transform.eulerAngles.y + amount >= _rotationLowerBound.y){ && transform.eulerAngles.y + amount <= _rotationUpperBound.y) {
            print("rotating player now");
            transform.Rotate(0, rotation, 0);
       // }
    }
}
