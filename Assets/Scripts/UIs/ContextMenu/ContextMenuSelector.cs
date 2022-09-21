using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// The UI component of the Context Menu. Contains 8 selectables with the selected determined by the angle 
/// of the mouse to the centre of the menu. Menu consists of 8 icons and a single background image who's fill amount is controlled by the 
/// amount of available commands of the target.
/// </summary>
public class ContextMenuSelector : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler ,IPointerUpHandler
{
    [SerializeField] RectTransform rectTransform;
    private Vector2 _origin;
    [SerializeField] public int selectedPoint;
    [SerializeField] Image _background;
    [SerializeField] List<Image> wheelIcons = new List<Image>();
    [SerializeField] TMP_Text ActionNameDisplay;
    private bool _menuSelected;
    private int ActionSetId = 0;



    public void GenerateSelectionPoint()
    {
         float _sign = 1;
         float _offset = 0;



    // get direction
    var mouse = Input.mousePosition;
       var mouseDirection = new Vector2(mouse.x, mouse.y);
        Vector2 CurrentPoint = ( mouseDirection - _origin);

        // display angle correction
        _sign = (CurrentPoint.x >= 0) ?  1 : -1 ;
        _offset = (_sign >= 0) ? 0 : 360;


        //corrected angle
        var angle = Vector2.Angle(Vector2.up, CurrentPoint) * _sign + _offset;

    
        // round angle
       var angleFactor = 45f;
        #region -DeveloperNote-
        /*
           Angle factor is hard coded as 45 degrees to each segment of the menu. 
           When ripping out the system for Deity Project, refactor to accommodate dynamic segment sizes dependent on 
           total of required with a simple 360 / total to return angle factor of segment.
         */
        #endregion

        var roundedAngle = (Mathf.Floor(angle / angleFactor) * angleFactor);


        // Keep unused buttons hidden
        if (wheelIcons[selectedPoint].color != Color.clear)
        {
            wheelIcons[selectedPoint].color = Color.white;
        }


        // generate ID (0 - 7)
        selectedPoint = (int)(roundedAngle / angleFactor);


        var menuData = ContextMenu.instance.menuOptions;
        

        // Highlight button at given ID 
        if (wheelIcons[selectedPoint].color != Color.clear)
        {
            var actionId = ContextMenu.instance.menuSets[ActionSetId].menuOptions[selectedPoint];
            ActionNameDisplay.text = menuData[actionId].name;
            wheelIcons[selectedPoint].color = Color.cyan;
        }
        else
        {
            ActionNameDisplay.text = "";
        }

        // Display action name
        
        



      // DEBUG: DISPLAY WORKING VALUES AND FINAL ID
      //  print(roundedAngle + " - angle : offset - " + _offset + " | " + _selectedPoint);

    }




    public void OnPointerEnter(PointerEventData eventData)
    {
      

    }


    /// <summary>
    /// Disable the context Menu if the mouse leaves the area of the menu
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        _menuSelected = false;
        this.gameObject.SetActive(false);
    }
    


    public void OnMouseOver()
    {
      
        print("The mouse is over the selector");
    }

    /// <summary>
    /// Opens the context menu and
    /// </summary>
    /// <param name="position"></param>
    /// <param name="newIcons"></param>
    public void OpenMenu(Vector3 position, ContextMenuActionSet ActionSet, int SetId)
    {

        var menuData = ContextMenu.instance.menuOptions;
        var options = ActionSet.menuOptions;

        ActionSetId = SetId;

        var fillamount = Mathf.InverseLerp(0, 8, ActionSet.menuOptions.Count);
        _background.fillAmount = fillamount;


        // Set all unused icons to clear
        for (int i = options.Count; i < wheelIcons.Count; i++)
        {
            wheelIcons[i].color = Color.clear;
        }

        //Display correct sprite for actual actions
        for (int i = 0; i < ActionSet.menuOptions.Count; i++)
        {
            int id = ActionSet.menuOptions[i];

            wheelIcons[i].color = Color.white;
            wheelIcons[i].sprite = menuData[id].sprite;
        }

        _menuSelected = true;
        this.gameObject.SetActive(true);
        rectTransform.position = position;
        _origin = position;
       // print("pointer entered and centered at " + _origin);
        StartCoroutine(MousePositionLoop());
    }


    public void CloseMenu()
    {
        AudioManager.Audio_M.PlayUIClick();
        this.gameObject.SetActive(false);
        _menuSelected = false;
    }

    private IEnumerator MousePositionLoop()
    {
        while (_menuSelected != false)
        {
            GenerateSelectionPoint();
           // print("Running that loop message ");
            yield return null;
        }

        yield return null;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
