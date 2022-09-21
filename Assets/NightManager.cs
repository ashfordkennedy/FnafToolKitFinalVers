using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Rendering;
public class NightManager : MonoBehaviour
{

    public static NightManager instance;

    public List<NightSettings> nightSettings;

    [SerializeField] private VolumeProfile backupVolume;
    [SerializeField] private VolumeProfile sceneVolume;

    public int CurrentNight = 0;
    public float HourLength = 90;


#region powerSettings
    public float powerLoss = 0;
    public float currentPower = 50;
    public float totalPower = 50;
    public float powerPercent = 100;
    public UnityEvent noPower = new UnityEvent();

    [SerializeField] private Text _powerText;
    [SerializeField] private Image _PowerBar;
#endregion


    void OnEnable()
    {
        if (instance == null)
        {
            instance = this;

            nightSettings = new List<NightSettings>{
                new NightSettings(),
                new NightSettings(),
                new NightSettings(),
                new NightSettings(),
                new NightSettings(),
                new NightSettings(),
                new NightSettings(),
};
        }




    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Stops all processing of night logic
    /// </summary>
     internal void CancelNight()
    {
        StopCoroutine(PowerProcessing());
        print("ended night");
        ResetMap();
    }



    /// <summary>
    /// Resets the environment and placement of objects to their start positions
    /// </summary>
    public void ResetMap()
    {


    }

    /// <summary>
    /// Starts the night
    /// </summary>
    public void BeginNight()
    {    

        powerLoss = 0f;
        UpdatePowerDisplay();
        StartCoroutine(PowerProcessing());

    }

    /// <summary>
    /// ends the night if the player makes it to the final hour
    /// </summary>
    public void EndNight()
    {
        StopCoroutine(PowerProcessing());
        print("ended night");
        var mapFile = SaveDataHandler.SaveHandler.SaveData.SelectedFile;
        if(mapFile != null)
        {
            SaveDataHandler.SaveHandler.SaveData.mapProgressData.CompleteNight(mapFile);
        }
    }


    public void RegisterPowerLoss(float value)
    {
        print("powerloss detected");
        powerLoss += value;
    }

    public void RemovePowerLoss(float value)
    {
       powerLoss -= value;
    }

    private IEnumerator PowerProcessing()
    {
        while (currentPower > 0)
        {
            yield return new WaitForSeconds(5f);
            currentPower -= powerLoss;
            UpdatePowerDisplay();
        }

        PowerOff();

        yield return null;
    }


    /// <summary>
    /// triggers noPower event, causing objects subscribed to run their behaviours
    /// </summary>
    private void PowerOff()
    {
        noPower.Invoke();
    }

    private void UpdatePowerDisplay()
    {
       float powerPercentage = Mathf.RoundToInt(Mathf.InverseLerp(0, totalPower, currentPower) * 100);
       powerPercent = powerPercentage;
       _powerText.text = "POWER LEFT: " + powerPercentage + "%";
    }
}


[Serializable]
public class NightSettings
{
    public float hourLength = 30f;
    public int startHour = 12;
    public int endHour = 6;
    public float basePowerLoss = 0.05f;
    public float totalPower = 5f;
    public SaveableColour fogColor = new SaveableColour(0,0,0);
    public float fogIntensity = -0.15f;


    public NightSettings(int starthour = 12, int endhour = 6, float basepowerloss = 0.05f, float totalpower = 5f, float hourLength = 30f, Color fogColor = new Color(), float fogIntensity = -0.15f)
    {
        this.startHour = starthour;
        this.endHour = endhour;
        this.basePowerLoss = basepowerloss;
        this.totalPower = totalpower;
        this.hourLength = hourLength;
        this.fogColor = new SaveableColour(fogColor);
        this.fogIntensity = fogIntensity;
    }


    public NightSettings()
    {
        this.startHour = 12;
        this.endHour = 6;
        this.basePowerLoss = 0.05f;
        this.totalPower = 5f;

    }
}
