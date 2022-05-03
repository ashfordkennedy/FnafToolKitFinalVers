using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CameraMenu : EditorMenuAbstract
{
    public static CameraMenu instance;
    private Decor_Camera targetCamera;
    [SerializeField] private TMP_InputField _nameField;
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

    public void ToggleCameraPreview(bool enable){
    targetCamera.gameobject.setActive(enable);    
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
