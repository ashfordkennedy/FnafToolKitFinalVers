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
    public List<UnityEvent> ButtonOnEvents;
    public List<UnityEvent> ButtonOffEvents;
    public bool panelEnabled = true;
    public bool startEnabled = true;


    /// <summary>
    ///
    /// </summary>
    /// <param name="ButtonID"></param>
    public void ToggleButton(int ButtonID)
    {

        var index = buttonRenderer.materials;
        switch (buttonData[ButtonID].IsEnabled)
        {
            case false:
                
                index[buttonData[ButtonID].MatIndexTarget] = buttonData[ButtonID].OnMaterial;
                ButtonOnEvents[ButtonID].Invoke();

                break;

            case true:
                
                index[buttonData[ButtonID].MatIndexTarget] = defaultMaterial;
                ButtonOffEvents[ButtonID].Invoke();
                break;
        }

        buttonRenderer.materials = index;
        buttonData[ButtonID].IsEnabled = !buttonData[ButtonID].IsEnabled;
    }

    

   

    public DecorLighting lights;
    private void Start()
    {
        
    }


    public override void NightStartSetup()
    {
      //  ButtonOnActionSet.GenerateUnityEvent(ButtonOnEvent);
      //  ButtonOffActionSet.GenerateUnityEvent(ButtonOnEvent);
    }

   

    private static List<ObjectActionIndex> _objectActions = new List<ObjectActionIndex>
        {
        new ObjectActionIndex("None","No Actions Available",ObjectActionType.none),

    };



    public override SavedObject CompileObjectData()
    {

        var buttons = new ButtonSaveData(ObjectSaveDataType.ButtonSwitch, buttonData);



       var savedObject = new SavedObject(this.InternalName,this.SwatchID,buttons,new SavedTransform(this.transform));


        return savedObject;
    }











}

public class ButtonSaveData : ObjectSaveData
{
    public List<SwitchButton> ButtonData;
    public List<SavableObjectActionSet> ButtonOnActionSet;
    public List<SavableObjectActionSet> ButtonOffActionSet;
    public ButtonSaveData(ObjectSaveDataType dataType, List<SwitchButton> buttons) : base(dataType)
    {
        this.DataType = ObjectSaveDataType.ButtonSwitch;
        this.ButtonData = buttons;
      
    }

}


[System.Serializable]
public class SwitchButton
{
    public bool IsEnabled = false;
    public int MatIndexTarget = 0;
    public Material OnMaterial;

    public ObjectActionSet ButtonOnActionSet = new ObjectActionSet("ButtonOn");
    public ObjectActionSet ButtonOffActionSet = new ObjectActionSet("ButtonOff");
}








public interface IPlayModeInteractable
{
    void LeftClick();
    void RightClick();
}