using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum ContextMenuMode { select, light, Animatronic,wall}

/// <summary>
/// the context menu is designed to allow quick access and switching of mouse modes & the selection of edit options on objects
/// </summary>
public class ContextMenu : MonoBehaviour
{
    public static ContextMenu instance;
    public GameObject target;
    public RectTransform menuRoot;
    [SerializeField]  List<ContextMenuActionSet> _menuSets;
    [SerializeField] private ContextMenuSelector _menuSelector;
    int targetMenuSet = 0;
    private DecorObject decorTarget = null;

    #region MenuOptionIDs
    /*
     [Set 1: standard]
    select - standard Selector
    eraseCell - erase cell mode
    newCell - new cell mode
    newRoom - new room
    roomList - room list menu

    ---------------------------------
    [Set 2: wall tool]
    wallDefault - default wall
    wallDoor - door wall
    wallLeftDoor - left door wall

    */
    #endregion






   public void OnRightClick(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            print("Input detected by context menu, opening");

            DecorObject Dtarget = null;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider != null)
            {
                target = hit.collider.gameObject;
                print("Object hit by right click = " + hit.collider.name + " tagged - " + hit.collider.tag + " layer - " + hit.collider.gameObject.layer);
                targetMenuSet = 0;
                switch (hit.collider.gameObject.layer)
                {

                    default:
                        targetMenuSet = 0;
                        break;
                    // build grid open normally
                    case 11:
                        targetMenuSet = 0;
                        break;


                    // wall
                    case 9:
                        targetMenuSet = 1;
                        break;

                    // decor
                    case 12:
                        

                        if (target.TryGetComponent<DecorObject>(out Dtarget) == false)
                        {
                            Dtarget = target.GetComponentInParent<DecorObject>();
                        }                            
                       
                        /*
                        if (target == null)
                        {
                           
                        }
                        */
                        targetMenuSet = DecernObjectType(Dtarget);
                        
                        print("you hit a decor object ya knob");
                        break;

                }


                _menuSelector.OpenMenu(Input.mousePosition, _menuSets[targetMenuSet].OptionIcons);
            }
            else
            {
                _menuSelector.OpenMenu(Input.mousePosition, _menuSets[0].OptionIcons);
            }
        }


        else if (context.canceled)
        {
            if (_menuSelector.selectedPoint < _menuSets[targetMenuSet].menuOptions.Count ) {
                string option = _menuSets[targetMenuSet].menuOptions[_menuSelector.selectedPoint];
                SelectOption(option);
            }
            _menuSelector.CloseMenu();
            print("Input detected by context menu, closing");

        }







          
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectOption(string option)
    {
      
            print(option + "Has been selected");
            switch (option)
            {
                case "select":
                RoomEditorMouse.Instance.ChangeMouseMode(3);
                    break;

                case "eraseCell":
                RoomEditorMouse.Instance.ChangeMouseMode(1);
                    break;


                case "newCell":
                RoomEditorMouse.Instance.ChangeMouseMode(0);
                    break;


                case "newRoom":
                RoomEditorMouse.Instance.ChangeMouseMode(2);
                    break;

                case "roomList":
                    // toggle regular select and open the ui
                    RoomListMenu.instance.ToggleMenu();
                RoomEditorMouse.Instance.ChangeMouseMode(3);
                    break;


                case "wallDefault":
                target.GetComponent<WallComponent>().SetWallType(WallType.Blank);
                break;

                case "wallDoor":
              target.GetComponent<WallComponent>().SetWallType(WallType.Door);
                break;


            case "wallLeftDoor":
                target.GetComponent<WallComponent>().SetWallType(WallType.DoorLeft);
                break;


            case "wallCenterDoor":
                target.GetComponent<WallComponent>().SetWallType(WallType.DoorCenter);
                break;

            case "wallRightDoor":
                target.GetComponent<WallComponent>().SetWallType(WallType.DoorRight);
                break;

            case "":

                break;

            case "Transform":

                // ObjectTransformController.ObjectTransformGizmo.OpenTransformController(!ObjectTransformController.ObjectTransformGizmo.menuOpen, decorTarget);
                ObjectPlacer.instance.OpenMenu();
                ObjectPlacer.instance.SelectObject(target.GetComponent<DecorObject>());
                break;

            case "Swatch":

                if (SwatchUI.Instance.MenuOpen == false || SwatchUI.Instance.target != decorTarget)
                {
                    print(decorTarget.InternalName);
                    SwatchUI.Instance.OpenSwatchPanel(decorTarget.InternalName, decorTarget);
                }
                else
                {
                    SwatchUI.Instance.CloseMenu();
                }
                break;

            case "Waypoint":
                if(WaypointSettingsPanel.Instance.MenuOpen == false || WaypointSettingsPanel.Instance.target != (AnimatronicWaypoint)decorTarget)
                {
                    WaypointSettingsPanel.Instance.OpenMenu((AnimatronicWaypoint)decorTarget);
                }
                else
                {
                    WaypointSettingsPanel.Instance.CloseMenu();
                }
                break;


            case "Light":
                var Ltarget = (DecorLighting)decorTarget;
                if (LightSettingUI.Instance.MenuOpen == false || LightSettingUI.Instance.TargetLight != Ltarget)
                {
                    LightSettingUI.Instance.ToggleLightUI(true, Ltarget);
                  //  SwatchUI.Instance.OpenSwatchPanel(targ.InternalName, targ);
                }
                else
                {
                    SwatchUI.Instance.CloseMenu();
                }
                break;

            case "Animatronic":
                var Atarget = (EditorAnimatronic)decorTarget;
                if (AnimatronicMenu.instance.MenuOpen == false || AnimatronicMenu.instance.targetAnimatronic != Atarget)
                {
                    AnimatronicMenu.instance.OpenMenu(Atarget);
                    //  SwatchUI.Instance.OpenSwatchPanel(targ.InternalName, targ);
                }
                else
                {
                    AnimatronicMenu.instance.CloseMenu();
                }
                break;

        }
        


    }

    private int DecernObjectType(DecorObject target)
    {
        decorTarget = target;
        int MenuSet = 2;
        switch (target.decorType)
        {
            // basic decor
            case DecorObjectType.Basic:
                
                MenuSet = 2;
                break;

            case DecorObjectType.Light:
                MenuSet = 3;
                break;

            case DecorObjectType.Door:

                break;

          
            case DecorObjectType.Waypoint:
                MenuSet = 4;
                break;

            case DecorObjectType.Animatronic:
                MenuSet = 5;
                break;

        }

        return MenuSet;
    }
}

[System.Serializable]
public class ContextMenuActionSet
{
    public string internalName;
  
    public List<Sprite> OptionIcons = new List<Sprite>();
    [Space(20)]

    public List<string> menuOptions = new List<string>();
}



[System.Serializable]
public class ContextMenuOption
{
    /// <summary>
    /// The Internal name is used as the send message value when triggering the MenuOption
    /// </summary>
    public string internalName;
    public string name;
}
