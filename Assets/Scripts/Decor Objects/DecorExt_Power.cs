using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
[System.Serializable]
public class DecorExt_Power
{
    public bool _active { get; private set; } = true;
    public bool _activeOnStart { get; private set; } = true;
    public float _powerDrain { get; private set; } = 0f;

    public UnityEvent powerOn = new UnityEvent();
    public UnityEvent powerOff = new UnityEvent();


    public void SetActive(bool active)
    {
        _active = active;
    }

    public void SetOnStartActive(bool active)
    {
        _activeOnStart = active;
    }
    public void SetPowerValue(float value)
    {
        _powerDrain = value;
    }



    public  DecorExt_Power(bool active = true, bool activeOnStart = true, float powerDrain = 1f)
    {
        _active = active;
        _activeOnStart = activeOnStart;
        _powerDrain = powerDrain;
    }

    public void LoadSettings(PowerData powerData)
    {
        _active = powerData._active;
        _activeOnStart = powerData._activeOnStart;
        _powerDrain = powerData._powerDrain;

    }
}


[System.Serializable]
    public class PowerData
{
    public bool _active = true;
    public bool _activeOnStart = true;
    public float _powerDrain = 0f;


    public PowerData(DecorExt_Power powerExt)
    {
        _active = powerExt._active;
        _activeOnStart = powerExt._activeOnStart;
        _powerDrain = powerExt._powerDrain;

    }


}

