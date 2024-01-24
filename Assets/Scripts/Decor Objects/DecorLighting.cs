using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using ObjectActionEvents;
public class DecorLighting : DecorObject
{

    //Event slider system
    public float _intensity { get => lightData[l].intensity; set => SetIntensity(value,l);}
    public float _volume { get => lightData[l].volumetricDimmer; set => lightData[l].volumetricDimmer = value; }
    public float _range { get => lightData[l].range; set => lightData[l].range = value; }
    public Color _color { get => lightData[l].color; set => lightData[l].color = value; }

    public float _OuterAngle { get => lightData[l].lightAngle; set => lightData[l].SetSpotAngle(value); }
    public float _InnerAngle { get => lightData[l].innerSpotPercent; set => lightData[l].innerSpotPercent = value; }
    public float _radius { get => lightData[l].shapeRadius; set => lightData[l].shapeRadius = value; }



    /// <summary>
    /// properties used to modify the light default values. default values are used on map load so that previous nights setting changes are reverted. 
    /// They are also used by the light setting panel in concert with the event slider system.
    /// </summary>
    #region map_editor_System
    public float defaultIntensity { get => lightDefaults[l].intensity; set => lightDefaults[l].intensity = value; }
    public float defaultVolume { get => lightDefaults[l].volume; set => lightDefaults[l].volume = value; }
    public float defaultRange { get => lightDefaults[l].range; set => lightDefaults[l].range = value; }
    public Color defaultColor { get => lightDefaults[l].color.ToColor(); set => lightDefaults[l].color = SavableColour.ToSavableColour(value); }

    public float defaultOuterAngle { get => lightDefaults[l].outerAngle; set => lightDefaults[l].outerAngle = value; }
    public float defaultinnerAngle { get => lightDefaults[l].innerAngle; set => lightDefaults[l].innerAngle = value; }
    public float defaultRadius { get => lightDefaults[l].radius; set => lightDefaults[l].radius = value; }

    public int LightCount { get => lightData.Length; }

    public string GetSubLightName(int x) => lightData[x].gameObject.name;
    #endregion

    public DecorLightStats[] lightDefaults;
    public HDAdditionalLightData[] lightData;


    

    public static int l = 0;

    public DecorExt_Power powerSettings = new DecorExt_Power();


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


    public override void EditorSelect()
    {
        base.EditorSelect();
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
       lightData = this.GetComponentsInChildren<HDAdditionalLightData>();
      
    }

    public override SavedObject CompileObjectData()
    {
        Debug.LogWarning("please refactor light saving methods");
       var lightData = new LightData(lightDefaults, new PowerData(powerSettings));
        SavedLight Data = new SavedLight(InternalName,SwatchID, new LightSaveData(ObjectSaveDataType.Light,lightData), new SavedTransform(this.transform));
        return Data;
    }


    public void RestoreLightSave(LightData savedata)
    {
      // LightSaveData Savedata = new LightSaveData(ObjectSaveDataType.Light, new LightData(0, 0, new Color(0, 0, 0)));

      //  LightSaveData Savedata = savedata as LightSaveData;
        
        print("processing light data");

        lightDefaults = savedata.lightStats;



        for (int i = 0; i < lightDefaults.Length; i++)
        {
            var light = lightData[i];
            var data = lightDefaults[i];
            light.intensity = data.intensity;
            light.volumetricDimmer = data.volume;
            light.range = data.range;

            light.SetSpotAngle(data.outerAngle, data.innerAngle);
            light.range = data.range;
            light.SetColor(data.color.ToColor());
        }

        powerSettings.LoadSettings(savedata.powerData);
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
        LightToggle(powerSettings._activeOnStart);
        powerSettings.SetPowerValue(powerSettings._powerDrain);

        if(powerSettings._activeOnStart == true)
        {
            // start power drain
        }
    }

    
    public void SetIntensity(float intensity, int Id = 0)
    {
        lightData[Id].intensity = intensity;

        if (lightData[Id].intensity <= 0)
        {
            powerSettings.powerOff.Invoke();
        }
        else if (powerSettings._active == false)
        {
            powerSettings.powerOn.Invoke();
        }
            ;

    }
    

    public void SetVolume(float volume, int Id = 0)
    {
        _volume = volume;
        lightData[Id].volumetricDimmer = volume;
        
    }

    public void SetRange(float range, int Id = 0)
    {
        _range = range;
        lightData[Id].range = range;
    }

    public void LightToggle(bool active, int Id = 0)
    {
        powerSettings.SetActive(active);
        lightData[Id].gameObject.GetComponent<Light>().enabled = active;
    }



    public void SetColour(Color value)
    {
        _color = value;
        lightData[l].color = value;
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
    public DecorLightStats[] lightStats;
    public PowerData powerData;



    public LightData(DecorLightStats[] lightStats, PowerData powerData)
    {
        this.lightStats = lightStats;
        this.powerData = powerData;
    }

   

}


[System.Serializable]
public class DecorLightStats
{
    public float intensity = 0f;
    public float range = 0f;
    public float volume = 0f;
    public SavableColour color = new SavableColour(Color.white);

    public float outerAngle = 0f;
    public float innerAngle = 0f;
    public float radius = 0f;


    public DecorLightStats(HDAdditionalLightData light)
    {
        this.intensity = light.intensity;
        this.range = light.range;
        this.volume = light.volumetricDimmer;
        this.color = new SavableColour(light.color);
        this.outerAngle = light.lightAngle;
        this.innerAngle = light.innerSpotPercent;
        this.radius = light.shapeRadius;

    }


}