using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ButtonState { On, Off, Locked };
public class ButtonMenu : EditorMenuAbstract
{
    public static ButtonMenu instance;
    private DecorButton target;
    private int SelectedButton = 0;
    [SerializeField] TMP_Text buttonHeaderText;

    [SerializeField] GameObject[] EventButtons;

    [SerializeField] Image OnColour;
    [SerializeField] Image OffColour;
    [SerializeField] Image LockedColour;
    [SerializeField] TMP_InputField tagField;

    private void OnEnable()
    {
        instance = this;
    }

    public override void OpenMenu()
    {
        DisableButtons();
        EnableButtons();
        SelectedButton = 0;
        LoadButtonValues(0);
        base.OpenMenu();

    }

    private void LoadButtonValues(int buttonIndex)
    {
        SwitchButton targetButton = target.buttonData[buttonIndex];

        OnColour.color = targetButton.OnColor;
        OffColour.color = targetButton.OffColor;
        LockedColour.color = targetButton.LockedColor;
        tagField.text = targetButton.GetTag();
    }

   

    /// <summary>
    /// Enables the required edit event buttons based off of target data
    /// </summary>
    private void EnableButtons()
    {
        for (int i = 0; i < target.buttonData.Count; i++)
        {
            EventButtons[i].SetActive(true);
        }
    }

    private void DisableButtons()
    {
        for (int i = 0; i < EventButtons.Length; i++)
        {
            EventButtons[i].SetActive(false);
        }
    }

    public override void CloseMenu()
    {
        
        base.CloseMenu();
        ColourSelector.instance.CloseMenu();
    }

    public void SetTarget(DecorButton newTarget)
    {
        target = newTarget;
    }

    public void DisplayNextButton()
    {
        int max = target.buttonData.Count -1;
        if (SelectedButton == max)
        {
            SelectedButton = 0;
        }
        else
        {
            SelectedButton++;
        }
        buttonHeaderText.text = "Button " + SelectedButton;
        LoadButtonValues(SelectedButton);
    }

    public void DisplayPreviousButton()
    {
        int max = target.buttonData.Count;
        if (SelectedButton == 0)
        {
            SelectedButton = max;
        }
        else
        {
            SelectedButton--;
        }
        buttonHeaderText.text = "Button " + SelectedButton;
        LoadButtonValues(SelectedButton);
    }

    public void EditButtonOnEvent()
    {
        ObjectActionSet targetSet = target.buttonData[SelectedButton].ButtonOnActionSet;
        ObjectActionsMenu.instance.SetTargetActionSet(targetSet);
        ObjectActionsMenu.instance.OpenMenu();
    }

    public void EditButtonOffEvent()
    {
        ObjectActionSet targetSet = target.buttonData[SelectedButton].ButtonOffActionSet;
        ObjectActionsMenu.instance.SetTargetActionSet(targetSet);
        ObjectActionsMenu.instance.OpenMenu();
    }

    public void EditButtonEnableEvent()
    {
        ObjectActionSet targetSet = target.buttonData[SelectedButton].ButtonEnabledActionSet;
        ObjectActionsMenu.instance.SetTargetActionSet(targetSet);
        ObjectActionsMenu.instance.OpenMenu();
    }

    public void EditButtonDisableEvent()
    {
        ObjectActionSet targetSet = target.buttonData[SelectedButton].ButtonDisabledActionSet;
        ObjectActionsMenu.instance.SetTargetActionSet(targetSet);
        ObjectActionsMenu.instance.OpenMenu();
    }


    public void SetButtonTag(TMP_InputField input)
    {
       target.buttonData[SelectedButton].SetButtonTag(input.text);
    }

    public void SetButtonOnColor(Image image)
    {       
        ColourSelector.instance.CloseMenu();
        ColourSelector.instance.updateEvent.AddListener(delegate { RecieveColorSelectorColour(ButtonState.On, image); });
        ColourSelector.instance.OpenMenu();
    }

    public void SetButtonOffColor(Image image)
    {
        ColourSelector.instance.CloseMenu();
        ColourSelector.instance.updateEvent.AddListener(delegate { RecieveColorSelectorColour(ButtonState.Off, image); });
        ColourSelector.instance.OpenMenu();
    }

    public void SetButtonLockedColor(Image image)
    {
        ColourSelector.instance.CloseMenu();
        ColourSelector.instance.updateEvent.AddListener(delegate { RecieveColorSelectorColour(ButtonState.Locked, image); });
        ColourSelector.instance.OpenMenu();
    }



    /// <summary>
    /// retrieve color selector choice and display
    /// </summary>
    /// <param name="buttonState"></param>
    /// <param name="button"></param>
    public void RecieveColorSelectorColour(ButtonState buttonState, Image button)
    {
        Color newColor = ColourSelector.instance.tempColour;
        newColor = new Color(newColor.r * 6, newColor.g * 6, newColor.b * 6);

        button.color = newColor;
        var buttonData = target.buttonData[SelectedButton];
        switch (buttonState)
        {
            case ButtonState.Off:
                buttonData.OffColor = newColor;
                break;

            case ButtonState.On:
                buttonData.OnColor = newColor;
                break;

            case ButtonState.Locked:
                buttonData.LockedColor = newColor;
                break;
        }

        target.buttonData[SelectedButton].SetButtonColor(newColor, ButtonState.Off);
    }
}
