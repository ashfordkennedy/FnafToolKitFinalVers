using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// the jumpscare works by moving the selected animatronic to the jumpscare layer to render seperately from the scene.
/// this ensures it is never clipped out and always renders correctly.
/// </summary>
public class JumpScareManager : MonoBehaviour
{

    [SerializeField] Camera TextureCamera;
    [SerializeField] GameObject jumpscareUIImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TriggerJumpscare(Animatronic animatronic)
    {
       //setup

        //play


        //finish

    }

    
}
