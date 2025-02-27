using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ScreenTouchInteractable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private enum ScreenInteractableMode {camera,PanLeft,PanRight }
    [SerializeField] private ScreenInteractableMode _interactableMode;



    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (_interactableMode)
        {
            case ScreenInteractableMode.camera:

                break;

            case ScreenInteractableMode.PanLeft:
                StartCoroutine(PanLoop(-1));
               // PlayerController_Classic.instance.RotatePlayer(_leftRotationSpeed);
                break;

            case ScreenInteractableMode.PanRight:
                StartCoroutine(PanLoop(1));
               // PlayerController_Classic.instance.RotatePlayer(_rightRotationSpeed);
                break;



        }
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
    }

    IEnumerator PanLoop(float rotation)
    {
        while (1 != 2)
        {
            PlayerController_Classic.instance.RotatePlayer(rotation);
            yield return null;
        }

        yield return null;
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
