using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/// <summary>
/// outdated af script. you dont need her, just nick the mouse mode switching
/// </summary>
public class ToolUI : MonoBehaviour
{
    [SerializeField] ToggleGroup BuildToolToggles;
    [SerializeField] Toggle SelectModeToggle;







    /// <summary>
    /// Registers toggles on the tool bars to the toggle group
    /// </summary>
    public void RegisterToolToggles()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            BuildToolToggles.RegisterToggle(this.gameObject.transform.GetChild(i).GetComponent<Toggle>());
        }

    }







    /// <summary>
    /// performs Changemousemode() and also performs set up
    /// </summary>
    /// <param name="NewMouseMode"></param>
    public void ChangeMouse(int NewMouseMode)
    {
        RoomEditorMouse.Instance.ChangeMouseMode(NewMouseMode);
        LightSettingUI.Instance.ToggleLightUI(false);
        ActiveToggleCheck();

    }



    /// <summary>
    /// used to disable toggles when mouse modes not enabled though the toolbar are in use, eg. Target Select
    /// </summary>
    public void ReactivateToggles()
    {   
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            this.gameObject.transform.GetChild(i).GetComponent<Toggle>().interactable = true; 
        }

        ActiveToggleCheck();
    }

    public void DeactivateToggles()
    {
        BuildToolToggles.SetAllTogglesOff();
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            this.gameObject.transform.GetChild(i).GetComponent<Toggle>().interactable = false;
        }
    }

    /// <summary>
    /// Checks if any tools are selected on the UI, then swaps to select mode 
    /// </summary>
    void ActiveToggleCheck()
    {
        AudioManager.Audio_M.PlayUIClick();
        if (BuildToolToggles.AnyTogglesOn() == false)
        {
            SelectModeToggle.isOn = true;
            BuildToolToggles.NotifyToggleOn(SelectModeToggle);
            RoomEditorMouse.Instance.ChangeMouseMode(3);
        }

    }


}
