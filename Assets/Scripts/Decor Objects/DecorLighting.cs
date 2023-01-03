using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using ObjectActionEvents;
public class DecorLighting : DecorObject
{

    public float _intensity {get; private set;} = 15f;
    public float _volume { get; private set; } = 5f;
    public float _range { get; private set; } = 65f;
    public Color _color { get; private set; }
    private HDAdditionalLightData lightData;
    [SerializeField] private Light light;

    public bool _active { get; private set; } = true;
    public bool _activeOnStart { get; private set; } = true;
    public float _powerDrain { get; private set; } = 0f;


    [SerializeField] static DecorLighting()
    {
        ObjectActions = new List<ObjectActionIndex>
    {   
        new ObjectActionIndex("SetLightIntensity","Set Intensity",ObjectActionType.SetFloat),
        new ObjectActionIndex("SetLightRange","Set Range",ObjectActionType.SetFloat),
        new ObjectActionIndex("SetLightVolume","Set Volume",ObjectActionType.SetFloat),
        new ObjectActionIndex("SetLightOn","Light On/Off",ObjectActionType.SetBool)
        };
    }

    public override List<ObjectActionIndex> GetObjectActions()
    {
        return ObjectActions;
    }


    public override void EditorSelect(Material SelectMaterial)
    {
        base.EditorSelect(SelectMaterial);
       // LightSettingUI.Instance.SetTargetLight(this);
      // LightSettingUI.Instance.ToggleLightUI(true);
    }

    public override void EditorDeselect()
    {
        base.EditorDeselect();
       // LightSettingUI.Instance.SetTargetLight(null);
       // LightSettingUI.Instance.ToggleLightUI(false);
    }


    public override void ObjectSetup()
    {
       base.ObjectSetup();
       lightData = this.GetComponentInChildren<HDAdditionalLightData>();
    }

    public override SavedObject CompileObjectData()
    {
        var lightData = new LightData(_intensity, _volume, _color,_range,_powerDrain,_activeOnStart,_active);
        SavedLight Data = new SavedLight(InternalName,SwatchID, new LightSaveData(ObjectSaveDataType.Light,lightData), new SavedTransform(this.transform));
        return Data;
    }


    public void RestoreLightSave(LightData savedata)
    {
      // LightSaveData Savedata = new LightSaveData(ObjectSaveDataType.Light, new LightData(0, 0, new Color(0, 0, 0)));

      //  LightSaveData Savedata = savedata as LightSaveData;
        
        print("processing light data");

        SetIntensity(savedata.Intensity);
        SetVolume(savedata.Volume);
        SetRange(savedata.Range);

        Color lightcolour = savedata.Colour.ToColor();
        SetColour(lightcolour);
        LightToggle(savedata.active);
        SetOnStartActive(savedata.activeOnStart);
        SetPowerValue(savedata.Power);
       
    }


    public override void DestroyObject()
    {
        if(LightSettingUI.Instance.MenuOpen == true)
        {
            LightSettingUI.Instance.CloseMenu();
        }
        base.DestroyObject();
    }


    public override void NightStartSetup()
    {
        print("Night startup called");
        SetIntensity(_intensity);
        SetVolume(_volume);
        SetRange(_range);
        SetColour(_color);
        LightToggle(_activeOnStart);       
        SetPowerValue(_powerDrain);

        if(_activeOnStart == true)
        {

        }
    }


    public void SetIntensity(float intensity)
    {
        _intensity = intensity;
        lightData.intensity = intensity;
        light.intensity = intensity;

    }

    public void SetVolume(float volume)
    {
        _volume = volume;
        lightData.volumetricDimmer = volume;
        
    }

    public void SetRange(float range)
    {
        _range = range;
        lightData.range = range;
        light.range = range;
    }

    public void LightToggle(bool active)
    {
        _active = active;
        light.enabled = active;
    }

    public void SetOnStartActive(bool active)
    {
        _activeOnStart = active;
    }
    public void SetPowerValue(float value)
    {
        _powerDrain = value;
    }
    public void SetColour(Color value)
    {
        _color = value;
        lightData.color = value;
        light.color = value;
    }


    public void ActionEvent_SetLight(ChangeLightSettingsAction targetAction)
    {
        SetVolume(targetAction.volume);
        SetIntensity(targetAction.intensity);
        SetColour(targetAction.newColor);
        SetRange(targetAction.range);
        LightToggle(targetAction.OnOff);
    }
}



[System.Serializable]
public class SavableColour
{
    float R;
    float G;
    float B;

    public SavableColour()
    {
        R = 0;
        G = 0;
        B = 0;
    }

    public SavableColour(float r, float g, float b)
    {
        this.R = r;
        this.G = g;
        this.B = b;

    }

    public SavableColour(Color color)
    {
        this.R = color.r;
        this.G = color.g;
        this.B = color.b;
    }


    public Color ToColor()
    {
        return new Color(R, G, B);
    }

    public static SavableColour ToSavableColour(Color color)
    {
       var coltest = new SavableColour(color);
        Debug.Log("saved color is " + coltest.R + " " + coltest.G + " " + coltest.B);
        return new SavableColour(color.r, color.g, color.b);
    }

}


[System.Serializable]
public class LightData
{
    public float Intensity = 1f;
    public float Volume = 1f;
    public SavableColour Colour;
    public float Range = 10;
    public float Power = 0f;
    public bool activeOnStart = true;
    public bool active = true;



    public LightData(float intensity, float volume, Color colour, float range, float power, bool activeonstart, bool active)
    {
        this.Intensity = intensity;
        this.Volume = volume;
        this.Colour = new SavableColour(colour);
        this.Range = range;
        this.active = active;
        this.activeOnStart = activeonstart;
        this.Power = power;
    }

    public string Print()
    {
        return $"Light data|| intensity = {this.Intensity} || volume = {this.Volume}";
    }

}