using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decor_Camera : DecorObject
{
    public string cameraName { get; private set; } = "";
    [SerializeField] Camera _camera;





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
        _camera.transform.localRotation = Quaternion.Euler(rotation);
    }

    public void SetCameraPosition(Vector3 position)
    {
        _camera.transform.localPosition = position;

    }


    /// <summary>
    /// Enables the camera for rendering. Set to null to disable
    /// Used by camera manager to render to monitor.
    /// </summary>
    /// <param name="renderTexture"></param>
    public void SetRenderTexture(RenderTexture renderTexture)
    {
        _camera.targetTexture = renderTexture;
        if(_camera.targetTexture != null)
        {
            _camera.enabled = true;
        }
        else
        {
            _camera.enabled = false;
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
