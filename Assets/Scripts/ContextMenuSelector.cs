using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ContextMenuSelector : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler ,IPointerUpHandler
{
    [SerializeField] RectTransform rectTransform;
    private Vector2 _origin;
    [SerializeField] public int selectedPoint;
    [SerializeField] Image _background;
    [SerializeField] List<Image> icons = new List<Image>();
    private bool _menuSelected;
    private float _sign = 1;
    private float _offset = 0;

    public void GenerateSelectionPoint()
    {
        // get direction
       var mouse = Input.mousePosition;
       var mouseDirection = new Vector2(mouse.x, mouse.y);
        Vector2 CurrentPoint = ( mouseDirection - _origin);

        // display fix
        _sign = (CurrentPoint.x >= 0) ?  1 : -1 ;
        _offset = (_sign >= 0) ? 0 : 360;


        //corrected angle
        var angle = Vector2.Angle(Vector2.up, CurrentPoint) * _sign + _offset;

    
        // round angle
       var angleFactor = 45f;
       var roundedAngle = (Mathf.Floor(angle / angleFactor) * angleFactor);


        if (icons[selectedPoint].color != Color.clear)
        {
            icons[selectedPoint].color = Color.white;
        }
        // generateID
        selectedPoint = (int)roundedAngle / 45;

        if (icons[selectedPoint].color != Color.clear)
        {            
            icons[selectedPoint].color = Color.cyan;
        }
      //  print(roundedAngle + " - angle : offset - " + _offset + " | " + _selectedPoint);

    }




    public void OnPointerEnter(PointerEventData eventData)
    {
      

    }


    
    public void OnPointerExit(PointerEventData eventData)
    {
        _menuSelected = false;
        print("Exiting Context menu, play the damn close method will ya");
        this.gameObject.SetActive(false);
    }
    


    public void OnMouseOver()
    {
      
        print("The mouse is over the selector");
    }


    public void OpenMenu(Vector3 position, List<Sprite> newIcons)
    {
        var fillamount = Mathf.InverseLerp(0, 8, newIcons.Count);
        _background.fillAmount = fillamount;
        for (int i = newIcons.Count; i < icons.Count; i++)
        {
            icons[i].color = Color.clear;
        }
        for (int i = 0; i < newIcons.Count; i++)
        {
            icons[i].color = Color.white;
            icons[i].sprite = newIcons[i];
        }

        _menuSelected = true;
        this.gameObject.SetActive(true);
        rectTransform.position = position;
        _origin = position;
        print("pointer entered and centered at " + _origin);
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
