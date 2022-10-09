using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ButtonMenu : EditorMenuAbstract
{
    private DecorButton target;
    private int SelectedButton;
    [SerializeField] TMP_Text buttonHeaderText;

    [SerializeField] GameObject[] EventButtons;



    public override void OpenMenu()
    {
        DisableButtons();
        EnableButtons();

        base.OpenMenu();

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
    }

    public void SetTarget(DecorButton newTarget)
    {
        target = newTarget;
    }

    public void DisplayNextButton()
    {
        int max = target.buttonData.Count;
        if (SelectedButton == max)
        {
            SelectedButton = 0;
        }
        else
        {
            SelectedButton++;
        }
        buttonHeaderText.text = "Button " + SelectedButton + "Events";
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
}
