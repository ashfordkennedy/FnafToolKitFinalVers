using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CameraMenu : EditorMenuAbstract
{
    public static CameraMenu instance;
    [SerializeField] private Decor_Camera targetCamera;
    [SerializeField] private TMP_InputField _nameField;
    [SerializeField] RenderTexture previewTexture;


    private void Awake()
    {
        instance = this;
    }

    public void SetTarget(Decor_Camera target)
    {
        targetCamera = target;
    }

    void RepaintMenu()
    {
        _nameField.SetTextWithoutNotify(targetCamera.cameraName);

    }

    public void ToggleCameraPreview(Toggle toggle){
    targetCamera.camera.enabled = toggle.isOn;
        if (targetCamera.camera.enabled)
        {
            targetCamera.SetRenderTexture(previewTexture);
        }
        else
        {

        }
    
    }

    public void ToggleCameraPreview(bool enable)
    {
        targetCamera.camera.enabled = enable;
    }


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
