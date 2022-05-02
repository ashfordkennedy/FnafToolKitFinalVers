using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class MapToggleBar : MonoBehaviour
{
    [SerializeField] UnityEvent EditorLightSwitch = new UnityEvent();
    [SerializeField] UnityEvent MapLightSwitch = new UnityEvent();
    [SerializeField] UnityEvent GridOnSwitch = new UnityEvent();
    [SerializeField] UnityEvent GridOffSwitch = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void MapLightToggle(Toggle toggle)
    {

        switch (toggle.isOn)
        {

            case false:
                EditorLightSwitch.Invoke();
                break;


            case true:
                MapLightSwitch.Invoke();
                break;
        }
    }

    public void GridDisplayToggle(Toggle toggle)
    {

        switch (toggle.isOn)
        {

            case false:
                GridOffSwitch.Invoke();
                break;


            case true:
                GridOnSwitch.Invoke();
                break;
        }
    }
}
