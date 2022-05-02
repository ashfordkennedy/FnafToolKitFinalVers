using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// PlayModeController controls the various scripts that run during nightmode. such as NightManager and the AI controllers.
/// </summary>
public class PlayModeController : MonoBehaviour
{
    public static PlayModeController instance;
    [SerializeField] PlayerInput playerInput;


    private void Awake()
    {
        instance = this;
    }


    


    public void StartPlaymode()
    {
        UIController.instance.HideCanvas(true);
        GuiController.instance.InitialiseNightGui();
        GuiController.instance.EnableOfficeHudGui(true);
        NightManager.instance.BeginNight();
        NightClock.instance.SetClockSettings(new NightSettings(24,6,0.05f,5,10));
        NightClock.instance.ToggleClock(true);
    }


    /// <summary>
    /// Debug method, cancles the night and resumes editor
    /// </summary>
    public void EndPlayMode()
    {
        UIController.instance.HideCanvas(false);
        NightManager.instance.CancelNight();
        NightClock.instance.ToggleClock(false);
        GuiController.instance.EnableOfficeHudGui(false);
    }

   





    // Update is called once per frame
    void Update()
    {
        
    }
}
