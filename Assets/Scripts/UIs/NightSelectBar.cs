using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


/// <summary>
/// controller to easily enable and disable buttons without storing references.
/// </summary>
public class NightSelectBar : MonoBehaviour
{
    public Button[] buttons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        buttons = new Button[transform.childCount];
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = transform.GetChild(i).GetComponent<Button>();
        }

    }

    public void SetButtonStates(int maxNight)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = (i <= maxNight) ? true: false;
        }


    }  
}
