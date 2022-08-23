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

    public bool panelEnabled = true;
    public bool startEnabled = true;


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

    public ObjectActionSet ButtonOnActionSet = new ObjectActionSet("ButtonOn");
    public ObjectActionSet ButtonOffActionSet = new ObjectActionSet("ButtonOff");

    public UnityEvent ButtonOnEvent = new UnityEvent();
    public UnityEvent ButtonOffEvent = new UnityEvent();

    public DecorLighting lights;
    private void Start()
    {
        
    }


    public override void NightStartSetup()
    {
        ButtonOnActionSet.GenerateUnityEvent(ButtonOnEvent);
        ButtonOffActionSet.GenerateUnityEvent(ButtonOnEvent);
    }

    public void EditButtonOnEvent()
    {
        ObjectActionsMenu.instance.SetTargetActionSet(ButtonOnActionSet);
        ObjectActionsMenu.instance.OpenMenu();
    }

    public void EditButtonOffEvent()
    {
        ObjectActionsMenu.instance.SetTargetActionSet(ButtonOffActionSet);
        ObjectActionsMenu.instance.OpenMenu();
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