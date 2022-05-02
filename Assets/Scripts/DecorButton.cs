using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class DecorButton : MonoBehaviour //DecorObject
{
    [SerializeField] private MeshRenderer buttonRenderer;
    [SerializeField] private Material defaultMaterial;
     
    public List<SwitchButton> buttonData;

    public void ToggleButton(int ButtonID)
    {

        var index = buttonRenderer.materials;
        switch (buttonData[ButtonID].IsEnabled)
        {
            case false:
                
                index[buttonData[ButtonID].MatIndexTarget] = buttonData[ButtonID].OnMaterial;
                
               
                break;

            case true:
                
                index[buttonData[ButtonID].MatIndexTarget] = defaultMaterial;
                
                break;
        }

        buttonRenderer.materials = index;
        buttonData[ButtonID].ButtonEvent.Invoke();
        buttonData[ButtonID].IsEnabled = !buttonData[ButtonID].IsEnabled;
    }

   

}

[System.Serializable]
public class SwitchButton
{
   public bool IsEnabled = false;
    public int MatIndexTarget = 0;
    public UnityEvent ButtonEvent;
    public Material OnMaterial;
}





public interface IPlayModeInteractable
{
    void LeftClick();
    void RightClick();
}