using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class World_Interactable : MonoBehaviour, IInteractable
{
    [Tooltip("Enable to allow onLook to be called every frame the object targeted.")]
    [SerializeField] string _interactionName;

    [Tooltip("Enable to allow onLook to be called every frame the object targeted.")]
    public bool onLookToggle = false;
    public bool interactable = true;

    [Space]
    [Header ("Events")]
    public UnityEvent onInteract;
    public UnityEvent onLook;
    public UnityEvent onLookEnd;
    public UnityEvent onLookStart;

    public void OnInteract()
    {
        if(interactable == true)
        {
            onInteract.Invoke();
        }
    }


    public void OnLook()
    {
        if (onLookToggle == true)
        {
            onLook.Invoke();
        }
        
    }

    public void OnLookEnd()
    {
        onLookEnd.Invoke();
    }

    public void OnLookInteract()
    {
       
    }

    public void OnLookStart(out string name)
    {
        onLookStart.Invoke();
        name = _interactionName;
    }

}
