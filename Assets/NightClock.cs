using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NightClock : MonoBehaviour
{
    public static NightClock instance;
    public int currentHour = 12;
    public Text clockDisplay;
    [SerializeField] Text _endOfNightText;

    [SerializeField]private GameObject clockCanvas;
   [SerializeField] private GameObject EndOfNightCanvas;

    private NightSettings nightSettings;
    private void OnEnable()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void SetClockSettings(NightSettings nightSettings)
    {
        this.nightSettings = nightSettings;

    }



    public void ToggleClock(bool enabled = false)
    {
        clockCanvas.SetActive(enabled);

        switch (enabled)
        {
            case false:
                StopCoroutine("ClockProcessing");
                break;

            case true:
                StartCoroutine(ClockProcessing(nightSettings.hourLength, nightSettings.startHour, nightSettings.endHour));
                _endOfNightText.text = "" + nightSettings.endHour + "AM";
                break;
        }

        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            
        }
    }



    private IEnumerator ClockProcessing(float hourDuration,int startHour,int endhour)
    {
        currentHour = startHour;
        UpdateClockDisplay();

        while (currentHour != endhour)
        {

            yield return new WaitForSeconds(hourDuration);
            currentHour += 1;
            UpdateClockDisplay();
        }

        NightManager.instance.EndNight();
        EndOfNightCanvas.SetActive(true);
        yield return null;
    }



    private void UpdateClockDisplay()
    {
        if (currentHour > 24)
        {
            currentHour = 1;
        }

        int hourDisplay = currentHour;
        string hourSuffix = "AM";

        if (currentHour > 12)
        {
            hourDisplay = currentHour - 12;
            if (currentHour < 24)
            {
                hourSuffix = "PM";
            }
        }


       


        clockDisplay.text = "" + hourDisplay + "" + hourSuffix;

    }
}
