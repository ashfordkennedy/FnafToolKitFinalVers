using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using ObjectActionEvents;
public class DecorButton : DecorObject
{
    [SerializeField] private MeshRenderer buttonRenderer;
    [SerializeField] private Material defaultMaterial;
     
    public List<SwitchButton> buttonData;

    public void ToggleButton(int ButtonID)
    {

        var index = buttonRenderer.materials;
        switch (buttonData[ButtonID].IsEnabled)
        {
            case false:
                
                index[buttonData[ButtonID].MatIndexTarget] = buttonData[ButtonID].OnMaterial;
                
               
                break;

            case true:
                
                index[buttonData[ButtonID].MatIndexTarget] = defaultMaterial;
                
                break;
        }

        buttonRenderer.materials = index;
        buttonData[ButtonID].ButtonEvent.Invoke();
        buttonData[ButtonID].IsEnabled = !buttonData[ButtonID].IsEnabled;
    }

    public ObjectActionSet ButtonActionSet;
    public UnityEvent ButtonOnEvent;

    public DecorLighting lights;
    private void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ButtonActionSet.objectActions.Add(new ChangeLightSettingsAction(lights, Color.red, 10, 30, 100, true));
            ButtonActionSet.GenerateUnityEvent(ButtonOnEvent);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ButtonOnEvent.Invoke();
        }
    }

    private static List<ObjectActionIndex> _objectActions = new List<ObjectActionIndex>
        {
        new ObjectActionIndex("None","No Actions Available",ObjectActionType.none),

    };











}

[System.Serializable]
public class SwitchButton
{
   public bool IsEnabled = false;
    public int MatIndexTarget = 0;
    public UnityEvent ButtonEvent;
    public Material OnMaterial;
}





public interface IPlayModeInteractable
{
    void LeftClick();
    void RightClick();
}