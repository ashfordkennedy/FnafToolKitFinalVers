using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decor_Camera : DecorObject
{
    public string cameraName { get; private set; } = "";
    public Camera camera;





    public override void EditorSelect(Material SelectMaterial)
    {
        base.EditorSelect(SelectMaterial);


        if (CameraMenu.instance.MenuOpen == false)
        {
            CameraMenu.instance.SetTarget(this);
            CameraMenu.instance.OpenMenu();
        }
        {
            CameraMenu.instance.CloseMenu();
            CameraMenu.instance.SetTarget(this);
            CameraMenu.instance.OpenMenu();

        }
    }


    /// <summary>
    /// called by save handler
    /// </summary>
    /// <param name="cameraData"></param>
    public void RestoreCameraData(CameraData cameraData)
    {
        cameraName = cameraData.cameraName;
    }



    public void SetCameraRotation(Vector3 rotation)
    {
        camera.transform.localRotation = Quaternion.Euler(rotation);
    }

    public void SetCameraPosition(Vector3 position)
    {
        camera.transform.localPosition = position;

    }


    /// <summary>
    /// Enables the camera for rendering. Set to null to disable
    /// Used by camera manager to render to monitor.
    /// </summary>
    /// <param name="renderTexture"></param>
    public void SetRenderTexture(RenderTexture renderTexture)
    {
        camera.targetTexture = renderTexture;
        if(camera.targetTexture != null)
        {
            camera.enabled = true;
        }
        else
        {
            camera.enabled = false;
        }
    }


}


[System.Serializable]
public class CameraData
{
    public string cameraName = "";
    public SavedTransform CameraPos;

    public CameraData(Decor_Camera camera, Transform CameraTransform)
    {
        this.cameraName = camera.cameraName;
        this.CameraPos = new SavedTransform(CameraTransform);
    }

}
