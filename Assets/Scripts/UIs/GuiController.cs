using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiController : MonoBehaviour
{
    public static GuiController instance;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject EndOfNightCanvas;
    [SerializeField] GameObject SecurityCameraCanvas;
    [SerializeField] GameObject OfficeHudCanvas;

    public void StartClock()
    {
       
        NightClock.instance.ToggleClock(true);

    }

    public void ToggleSecurityCamera()
    {
        SecurityCameraCanvas.SetActive(!SecurityCameraCanvas.activeInHierarchy);

    }

    public void ToggleEndOfNight()
    {
        EndOfNightCanvas.SetActive(!EndOfNightCanvas.activeInHierarchy);
    }

    public void EnableEndOfNightGui(bool enable)
    {
        EndOfNightCanvas.SetActive(enable);
    }

    public void EnableOfficeHudGui(bool enable)
    {
        OfficeHudCanvas.SetActive(enable);
    }



    public void InitialiseNightGui()
    {
        canvasGroup.interactable = true;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

    }

    public void DisableNightGui()
    {
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

    }

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
