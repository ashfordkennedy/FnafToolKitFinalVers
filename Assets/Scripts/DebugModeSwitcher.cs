using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class DebugModeSwitcher : MonoBehaviour
{
    enum GameMode {FirstPerson,Editor }
    public UnityEvent ToggleFp;
    public UnityEvent ToggleEM;
    GameMode GM = GameMode.Editor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && Input.GetKey(KeyCode.LeftShift))
        {
            switch (GM)
            {

                case GameMode.FirstPerson:
                    ToggleEM.Invoke();
                    GM = GameMode.FirstPerson;
                    GM = GameMode.Editor;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;

                case GameMode.Editor:
                    ToggleFp.Invoke();
                    GM = GameMode.FirstPerson;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = true;
                    break;
            }


        }
    }
}
