using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using ObjectActionEvents;
using TMPro;
public class DecorButton : DecorObject
{
    [SerializeField] private MeshRenderer buttonRenderer;
    [SerializeField] private Material defaultMaterial;

    [SerializeField] private DecorButton _linkedButton = null;
    public List<SwitchButton> buttonData = new List<SwitchButton>();
    public List<UnityEvent> ButtonOnEvents = new List<UnityEvent>();
    public List<UnityEvent> ButtonOffEvents = new List<UnityEvent>();
    public bool panelEnabled = true;
    public bool startEnabled = true;

    /// <summary>
    ///
    /// </summary>
    /// <param name="ButtonID"></param>
    public void ToggleButton(int ButtonID)
    {

        var index = meshRenderer.materials;
        switch (buttonData[ButtonID].IsEnabled)
        {
            case false:
                
                index[buttonData[ButtonID].MaterialIndexTarget] = buttonData[ButtonID].OnMaterial;
                ButtonOnEvents[ButtonID].Invoke();

                break;

            case true:
                
                index[buttonData[ButtonID].MaterialIndexTarget] = defaultMaterial;
                ButtonOffEvents[ButtonID].Invoke();
                break;
        }

        buttonRenderer.materials = index;
        buttonData[ButtonID].IsEnabled = !buttonData[ButtonID].IsEnabled;

        if(_linkedButton != null)
        {
            _linkedButton.MirrorButtonToggle(ButtonID);
        }
    }

    public void MirrorButtonToggle(int ButtonID)
    {
        var index = meshRenderer.materials;
        switch (buttonData[ButtonID].IsEnabled)
        {
            case false:

                index[buttonData[ButtonID].MaterialIndexTarget] = buttonData[ButtonID].OnMaterial;

                break;

            case true:

                index[buttonData[ButtonID].MaterialIndexTarget] = defaultMaterial;
                break;
        }
        buttonRenderer.materials = index;
        buttonData[ButtonID].IsEnabled = !buttonData[ButtonID].IsEnabled;
    }

    
    private void Start()
    {
        
    }

    public override void ObjectSetup()
    {
        base.ObjectSetup();
        for (int i = 0; i < buttonData.Count; i++)
        {
            buttonData[i].SetDefaultColor();
        }
    }


    public override void NightStartSetup()
    {
        for (int i = 0; i < buttonData.Count; i++)
        {
            buttonData[i].WipeEvents();
            buttonData[i].GenerateEvents();
        }
        }

    /// <summary>
    /// safely nullifies the other objects reference to this object. Called by the other button
    /// </summary>
    public void DeregisterLinkedObects()
    {
        _linkedButton = null;
    }


    /// <summary>
    /// Destroy the object but also call the unlink function to stop null reference exception
    /// </summary>
    public override void DestroyObject()
    {
        _linkedButton.DeregisterLinkedObects();
        base.DestroyObject();
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


    public void RestoreObjectSave(ButtonSaveData saveData)
    {
        for (int i = 0; i < saveData.buttonEnabled.Count; i++)
        {
            buttonData[i].IsEnabled = saveData.buttonEnabled[i];
            buttonData[i].SetButtonColor(saveData.OffColors[i].ToColor(), ButtonState.Off);
            buttonData[i].SetButtonColor(saveData.OnColors[i].ToColor(), ButtonState.On);
            buttonData[i].SetButtonColor(saveData.LockedColors[i].ToColor(), ButtonState.Locked);

            buttonData[i].SetButtonTag(saveData.nameTags[i]);


           // print(($"button data {i} exists, no excuse for an error here, {buttonData.Count} button data fields existing here"));


            buttonData[i].ButtonDisabledActionSet = new ObjectActionSet(saveData.buttonDisabledActionSet[i]);
            buttonData[i].ButtonEnabledActionSet = new ObjectActionSet(saveData.buttonEnabledActionSet[i]);
            buttonData[i].ButtonOnActionSet = new ObjectActionSet(saveData.buttonOnActionSet[i]);
            buttonData[i].ButtonOffActionSet = new ObjectActionSet(saveData.buttonOffActionSet[i]);
            
        }
    }

   






}

/// <summary>
/// button save data stores switch button values in lists to be recreated later. (Stores only relevant information so no savable switchButton class was needed)
/// </summary>
 [System.Serializable]
public class ButtonSaveData : ObjectSaveData
{
    
    public List<SavableObjectActionSet> buttonOnActionSet = new List<SavableObjectActionSet>();
    public List<SavableObjectActionSet> buttonOffActionSet = new List<SavableObjectActionSet>();
    public List<SavableObjectActionSet> buttonEnabledActionSet = new List<SavableObjectActionSet>();
    public List<SavableObjectActionSet> buttonDisabledActionSet = new List<SavableObjectActionSet>();
    public List<string> nameTags = new List<string>();
    public List<SavableColour> OnColors = new List<SavableColour>();
    public List<SavableColour> OffColors = new List<SavableColour>();
    public List<SavableColour> LockedColors = new List<SavableColour>();
    public List<bool> buttonEnabled = new List<bool>();
    

    public ButtonSaveData(ObjectSaveDataType dataType, List<SwitchButton> buttons) : base(dataType)
    {
        this.DataType = ObjectSaveDataType.ButtonSwitch;

        for (int i = 0; i < buttons.Count; i++)
        {

            var onActionSet = new SavableObjectActionSet(buttons[i].ButtonOnActionSet);
            buttonOnActionSet.Add(onActionSet);

            var offActionSet = new SavableObjectActionSet(buttons[i].ButtonOffActionSet);
            buttonOffActionSet.Add(offActionSet);

            var enabledActionSet = new SavableObjectActionSet(buttons[i].ButtonEnabledActionSet);
            buttonEnabledActionSet.Add(enabledActionSet);

            var disabledActionSet = new SavableObjectActionSet(buttons[i].ButtonDisabledActionSet);
            buttonDisabledActionSet.Add(disabledActionSet);

            nameTags.Add(buttons[i].GetTag());

            OnColors.Add(new SavableColour(buttons[i].OnColor));
            OffColors.Add(new SavableColour(buttons[i].OffColor));
            LockedColors.Add(new SavableColour(buttons[i].LockedColor));
            buttonEnabled.Add(buttons[i].IsEnabled);

        }
      
    }

}


[System.Serializable]
public class SwitchButton
{
    public bool IsEnabled = false;
    public bool IsPressed = false;
    [Tooltip("The material index on the renderer the button targets")]public int MaterialIndexTarget = 0;
    public Material OnMaterial;
    public Color OnColor = Color.cyan;
    public Color OffColor = Color.red;
    public Color LockedColor = Color.yellow;
    [SerializeField] TMP_Text tagText;
    
    [SerializeField] private MeshRenderer buttonRenderer;

    public ObjectActionSet ButtonOnActionSet = new ObjectActionSet("ButtonOn");
    public ObjectActionSet ButtonOffActionSet = new ObjectActionSet("ButtonOff");
    public ObjectActionSet ButtonEnabledActionSet = new ObjectActionSet("ButtonEnabled");
    public ObjectActionSet ButtonDisabledActionSet = new ObjectActionSet("ButtonDisabled");
    public UnityEvent ButtonOnEvent;
    public UnityEvent ButtonOffEvent;
    public UnityEvent ButtonDisabledEvent;
    public UnityEvent ButtonEnabledEvent;

    public string GetTag()
    {
        return tagText.text;
    }

    public void ButtonToggle()
    {
        switch (IsPressed)
        {
            case true:
                ButtonOff();
                break;

            case false:
                ButtonOn();
                break;
        }
    }

    public void GenerateEvents()
    {
        ButtonOnActionSet.GenerateUnityEvent(ButtonOnEvent);
        ButtonOffActionSet.GenerateUnityEvent(ButtonOffEvent);
        ButtonEnabledActionSet.GenerateUnityEvent(ButtonOnEvent);
        ButtonDisabledActionSet.GenerateUnityEvent(ButtonDisabledEvent);
    }

    public void WipeEvents()
    {
        ButtonOnEvent.RemoveAllListeners();
        ButtonOffEvent.RemoveAllListeners();
        ButtonEnabledEvent.RemoveAllListeners();
        ButtonDisabledEvent.RemoveAllListeners();
    }


    public void ButtonOn()
    {
        IsPressed = true;
        SetMaterialColor(OnColor);
    }

    public void ButtonOff()
    {
        IsPressed = false;
        SetMaterialColor(OffColor);
    }

    /// <summary>
    /// alters the material to reflect the new colour
    /// </summary>
    /// <param name="color"></param>
    public void SetMaterialColor(Color color)
    {
       var materials = buttonRenderer.materials;
        materials[MaterialIndexTarget].SetColor("_BaseColor", color);
        buttonRenderer.materials = materials;
        Debug.Log("updating material");
    }

    /// <summary>
    /// Sets the colour values for the button
    /// </summary>
    /// <param name="color"></param>
    /// <param name="buttonState"></param>
    public void SetButtonColor(Color color, ButtonState buttonState)
    {
        switch (buttonState)
        {
            case ButtonState.Locked:
                LockedColor = color;
                break;

            case ButtonState.Off:
                OffColor = color;
                break;

            case ButtonState.On:
                OnColor = color;
                break;           
        }
        SetMaterialColor(OffColor);

    }

    public void SetButtonTag(string text)
    {
        tagText.text = text;
    }

    public void SetDefaultColor()
    {
        SetMaterialColor(OffColor);
    }
}








public interface IPlayModeInteractable
{
    void LeftClick();
    void RightClick();
}