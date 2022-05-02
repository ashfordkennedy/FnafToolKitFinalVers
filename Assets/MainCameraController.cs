using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{

    [SerializeField] int MaxRotation = 60;
    [SerializeField] int MinRotation = -60;
    [SerializeField] int CamRotation = 0;
    [SerializeField] Camera MainCamera;
    public bool Pan = false;



    IEnumerator PanLoop(float PanAmnt, int CamAmnt)
    {

        if (Pan == true && CamRotation + CamAmnt <= MaxRotation && CamRotation + CamAmnt != MinRotation)
        {  //&& gameObject.transform.rotation.y <   -125
            MainCamera.gameObject.transform.Rotate(0, PanAmnt, 0);
            CamRotation += CamAmnt;
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(PanLoop(PanAmnt, CamAmnt));
        }

        yield return null;

    }




    public void CameraPan(float scrollAmnt, int camAmnt)
    {
       // MainCamera.gameObject.transform.Rotate(new Vector3(MainCamera.gameObject.transform.rotation.x, scrollAmnt, MainCamera.gameObject.transform.rotation.z));
        Pan = true;
        StartCoroutine(PanLoop(scrollAmnt, camAmnt));
    }

    public void CameraPanStop()
    {
        Pan = false;
    }
}
