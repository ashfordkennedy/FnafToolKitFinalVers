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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
