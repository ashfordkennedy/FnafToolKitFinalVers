using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TooltipMenu : EditorMenuAbstract
{

    public static TooltipMenu instance;
    [SerializeField] TMP_Text tooltipText;
    [SerializeField] RectTransform tooltipTransform;

    private void Awake()
    {
        instance = this;
    }

    public void DisplayToolTip(string tipText)
    {
        OpenMenu();      
        tooltipText.text = tipText;
        StartCoroutine(RepositionTooltip());
    }

    public IEnumerator RepositionTooltip()
    {
        while (Time.time != 0)
        {
            tooltipTransform.position = Input.mousePosition;
            FitOnScreen();
            yield return new WaitForSecondsRealtime(0.1f);
           yield return null;
        }

        yield return null;
    }

    public void WipeToolTip()
    {
        StopCoroutine(RepositionTooltip());
        tooltipText.text = "";
       CloseMenu();
    }

    public void FitOnScreen()
    {
        float width = Screen.width;
        float Xpos = Input.mousePosition.x;
        float ScreenPos = Mathf.InverseLerp(0, width, Xpos);
        var newPivot = new Vector2(0,0);

        if(ScreenPos < 0.35f)
        {
            newPivot.x = 0;
        }
        else if (ScreenPos > 0.65f )
        {
            newPivot.x = 1;
        }
        else
        {
            newPivot.x = 0.5f;
        }
       // newPivot.x = (ScreenPos > 0.5f ) ? 1 : 0;
        tooltipTransform.pivot = newPivot;

       // print(Xpos);
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
