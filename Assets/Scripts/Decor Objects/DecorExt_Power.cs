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




}
