using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
/// <summary>
/// created 1/06/22 
/// james jordan
/// edited from unity standard assets
/// </summary> 
public class FovKick
{
    public Camera targetCamera;
    public float originalFov;
    public float FOVIncrease = 10f;
    public float IncreaseTime = 1f;
    public float DecreaseTime = 1f;
    public AnimationCurve increaseCurve;


    public void Setup(Camera camera)
    {
        targetCamera = camera;
        originalFov = camera.fieldOfView;

    }

    public IEnumerator FovKickUp()
    {
        Debug.Log("FovKickUpWorking");
        float newFov = originalFov + FOVIncrease;
        float startTime = Time.time;
        float finishTime = Time.time + IncreaseTime;

        while (Time.time < finishTime)
        {
            float t = Mathf.InverseLerp(startTime, finishTime, Time.time);
           // targetCamera.fieldOfView = Mathf.Lerp(originalFov, newFov, t);
            targetCamera.fieldOfView = originalFov + (increaseCurve.Evaluate(t / IncreaseTime) * FOVIncrease);
            //    originalFov + (increaseCurve.Evaluate(t) * FOVIncrease);
            yield return new WaitForEndOfFrame();
        }


        /*
        Debug.Log("FovKickUpWOrking");
        float t = Mathf.Abs(targetCamera.fieldOfView - originalFov) / FOVIncrease;
        while(t >0)
        {
            targetCamera.fieldOfView = originalFov + (increaseCurve.Evaluate(t / IncreaseTime) * FOVIncrease);
            t -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        */
    }

    public IEnumerator FovKickDown()
    {
        Debug.Log("FovKickDownWorking");
        float currentFov = targetCamera.fieldOfView;
        float startTime = Time.time;
        float finishTime = Time.time + DecreaseTime;

        while(Time.time < finishTime)
        {
            float t = Mathf.InverseLerp(startTime, finishTime, Time.time);
            //targetCamera.fieldOfView = Mathf.Lerp(currentFov, originalFov, t);
            targetCamera.fieldOfView = currentFov - (increaseCurve.Evaluate(t / DecreaseTime) * FOVIncrease);
            yield return new WaitForEndOfFrame();
        }


        /*
       
        float t = Mathf.Abs(targetCamera.fieldOfView - originalFov) / FOVIncrease;
        while (t > 0)
        {         
            targetCamera.fieldOfView = originalFov + (increaseCurve.Evaluate(t / DecreaseTime) * FOVIncrease);
            t -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        */
      //  targetCamera.fieldOfView = originalFov;

    }
}
