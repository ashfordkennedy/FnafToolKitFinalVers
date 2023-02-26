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
    [SerializeField]public  List<ContextMenuActionSet> menuSets;
    [SerializeField] private ContextMenuSelector _menuSelector;
    int targetMenuSet = 0;
    private DecorObject decorTarget = null;

    [SerializeField] public ContextMenuOption[] menuOptions;
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
                        
                        print("you hit a decor object ya knob. its set id is " + targetMenuSet);
                        break;
                }


                _menuSelector.OpenMenu(Input.mousePosition, menuSets[targetMenuSet], targetMenuSet);
            }
            else
            {
                _menuSelector.OpenMenu(Input.mousePosition, menuSets[0], targetMenuSet);
            }
        }


        else if (context.canceled)
        {
            if (_menuSelector.selectedPoint < menuSets[targetMenuSet].menuOptions.Count ) {
                print("Selecting menu option");
                string option = menuSets[targetMenuSet].internalName;
                // string option = menuSets[targetMenuSet].menuOptions[_menuSelector.selectedPoint];
                int id = menuSets[targetMenuSet].menuOptions[_menuSelector.selectedPoint];
                SelectOption(menuOptions[id].ActionType);
            }


            _menuSelector.CloseMenu();
            print("Input detected by context menu, closing");

        }







          
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    
    public void SelectOption(ContextMenuActions menuOptions)
    {
      
            print(menuOptions + "Has been selected");
            switch (menuOptions)
            {
                case ContextMenuActions.Select:
                RoomEditorMouse.Instance.ChangeMouseMode(3);
                    break;

                case ContextMenuActions.EraseCell:
                RoomEditorMouse.Instance.ChangeMouseMode(1);
                    break;


                case ContextMenuActions.NewCell:
                RoomEditorMouse.Instance.ChangeMouseMode(0);
                    break;


                case ContextMenuActions.NewRoom:
                RoomEditorMouse.Instance.ChangeMouseMode(2);
                    break;

                case ContextMenuActions.RoomList:
                    // toggle regular select and open the ui
                    RoomListMenu.instance.ToggleMenu();
                RoomEditorMouse.Instance.ChangeMouseMode(3);
                    break;

            case ContextMenuActions.SelectRoom:
                var room = target.GetComponentInParent<RoomController>();
                EditorController.Instance.NewRoomSelectHandler(room);

                break;

                case ContextMenuActions.WallDefault:
                target.GetComponent<WallComponent>().SetWallType(WallType.Blank);
                break;

                case ContextMenuActions.WallDoor:
              target.GetComponent<WallComponent>().SetWallType(WallType.Door);
                break;


            case ContextMenuActions.WallLeftDoor:
                target.GetComponent<WallComponent>().SetWallType(WallType.DoorLeft);
                break;


            case ContextMenuActions.WallCenterDoor:
                target.GetComponent<WallComponent>().SetWallType(WallType.DoorCenter);
                break;

            case ContextMenuActions.WallRightDoor:
                target.GetComponent<WallComponent>().SetWallType(WallType.DoorRight);
                break;


            case ContextMenuActions.Transform:

                // ObjectTransformController.ObjectTransformGizmo.OpenTransformController(!ObjectTransformController.ObjectTransformGizmo.menuOpen, decorTarget);
                ObjectPlacer.instance.OpenMenu();
                ObjectPlacer.instance.SelectObject(target.GetComponent<DecorObject>());
                break;

            case ContextMenuActions.Swatch:

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

            case ContextMenuActions.Waypoint:
                if(WaypointSettingsPanel.Instance.MenuOpen == false || WaypointSettingsPanel.Instance.target != (AnimatronicWaypoint)decorTarget)
                {
                    WaypointSettingsPanel.Instance.OpenMenu((AnimatronicWaypoint)decorTarget);
                }
                else
                {
                    WaypointSettingsPanel.Instance.CloseMenu();
                }
                break;


            case ContextMenuActions.Light:
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

            case ContextMenuActions.Animatronic:
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


            case ContextMenuActions.Delete:
                target.GetComponent<DecorObject>().DestroyObject();

                break;

            case ContextMenuActions.ClassicPlayer:
              var CPtarget = target.GetComponent<DecorClassicStart>();
                ClassicControllerMenu.instance.SetTarget(CPtarget);
                ClassicControllerMenu.instance.OpenMenu();

                break;

            case ContextMenuActions.ButtonPanel:
                var ButTarget = target.GetComponent<DecorButton>();
                ButtonMenu.instance.SetTarget(ButTarget);
                ButtonMenu.instance.OpenMenu();
                break;

            case ContextMenuActions.Camera:
                var camTarget = target.GetComponent<Decor_Camera>();
                CameraMenu.instance.SetTarget(camTarget);
                CameraMenu.instance.OpenMenu();
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

            case DecorObjectType.ClassicStart:
                MenuSet = 6;
                break;

            case DecorObjectType.Button:
                MenuSet = 7;
                break;

            case DecorObjectType.Camera:
                MenuSet = 8;
                break;

        }

        return MenuSet;
    }
}

[System.Serializable]
public class ContextMenuActionSet
{
    public string internalName;
    public List<int> menuOptions = new List<int>();
}


public enum ContextMenuActions
{
    Select, EraseCell, NewCell, NewRoom, RoomList, WallDefault,
    WallDoor, WallLeftDoor, WallCenterDoor, WallRightDoor, Transform, Swatch, Waypoint, Light, Animatronic, Delete, ClassicPlayer,
    ButtonPanel, SelectRoom, Camera
        
};

[System.Serializable]
public class ContextMenuOption
{
    /// <summary>
    /// The Internal name is used as the send message value when triggering the MenuOption
    /// </summary>
    public string name;
    public ContextMenuActions ActionType;    
    public Sprite sprite;
}
