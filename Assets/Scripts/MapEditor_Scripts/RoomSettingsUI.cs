using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomSettingsUI : EditorMenuAbstract
{
    public static RoomSettingsUI Instance;
    [SerializeField] GameObject RoomDesignContainer;
    [SerializeField] GameObject FloorDesignContainer;
    [SerializeField] GameObject WallSetButtonPrefab;
    [SerializeField] GameObject FloorSetButtonPrefab;
    [SerializeField] InputField RoomNameField;
    [SerializeField] Dropdown WallSetDropdown;
    // Start is called before the first frame update


    private void Awake()
    {
        Instance = this;
        PopulateDropdown();

    }
    void Start()
    {
        // PopulateSelectionTabs();
        
       // OpenRoomSettings(false);
    }


    /// <summary>
    /// Adds the editors found Wallsets as options to the dropdown menu
    /// </summary>
    public void PopulateDropdown()
    {
        for (int i = 0; i < EditorController.Instance.WallSets.Count; i++)
        {
            WallSetDropdown.options.Add(new Dropdown.OptionData(EditorController.Instance.WallSets[i].name));
        }

    }






    /// <summary>
    /// Called on scene load. Generates content based on map editor databases for walls and floors
    /// </summary>
    public void PopulateWallSelectionTabs()
    {
        ClearSelectionTabs();
        if (EditorController.Instance.SelectedRoom != null) { 
            for (int i = 0; i < EditorController.Instance.WallSets[EditorController.Instance.SelectedRoom.WallSet].MaterialSets.Count; i++)
            {
                Button newWall = Instantiate(WallSetButtonPrefab, RoomDesignContainer.transform).GetComponent<Button>();
                newWall.image.sprite = EditorController.Instance.WallSets[EditorController.Instance.SelectedRoom.WallSet].MaterialSets[i].Icon;
                newWall.gameObject.SetActive(true);
            }

            for (int i = 0; i < EditorController.Instance.Floors.Count; i++)
            {
                Button newWall = Instantiate(FloorSetButtonPrefab, FloorDesignContainer.transform).GetComponent<Button>();
                newWall.image.sprite = EditorController.Instance.Floors[i].Icon;
                newWall.gameObject.SetActive(true);
            }
        }
    }

    public void UpdateRoomSettings()
    {
        if (EditorController.Instance.SelectedRoom != null)
        {
            RoomNameField.text = EditorController.Instance.SelectedRoom.name;
        }
    }


    public override void OpenMenu()
    {
        base.OpenMenu();
        UpdateRoomSettings();
        PopulateWallSelectionTabs();
        WallSetDropdown.SetValueWithoutNotify(EditorController.Instance.SelectedRoom.WallSet);
    }


    public override void ToggleMenu()
    {
        base.ToggleMenu();
    }

    public override void CloseMenu()
    {
        base.CloseMenu();
        EditorController.Instance.SelectedRoom = null;
    }


    public void DeleteRoom()
    {
        var openRoom = EditorController.Instance.SelectedRoom;      
        CloseMenu();
        openRoom.DestroyRoom();
        EditorController.Instance.SelectedRoom = null;
    }

    public void OpenRoomSettings(bool Open) 
    {
        UpdateRoomSettings();
        PopulateWallSelectionTabs();
        this.gameObject.SetActive(Open);

        switch (Open)
        {
            case false:
                EditorController.Instance.SelectedRoom = null;

                break;
                

        }

    }

    public void ClearSelectionTabs()
    {
        foreach (Transform child in RoomDesignContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Transform child in FloorDesignContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void RenameRoom()
    {      
         EditorController.Instance.SelectedRoom.SetRoomName(RoomNameField.text);
    }


    public void SetRoomWallMaterial(GameObject Panel)
    {
        EditorController.Instance.SelectedRoom.WallSkin = Panel.transform.GetSiblingIndex();

       // EditorController.Instance.SelectedRoom.SetRoomMaterials();
        EditorController.Instance.SelectedRoom.RoomWallCheck();
    }

    public void SetRoomFloorType(GameObject Panel)
    {
        EditorController.Instance.SelectedRoom.FloorSet = Panel.transform.GetSiblingIndex();
       // EditorController.Instance.SelectedRoom.SetRoomMaterials();
         EditorController.Instance.SelectedRoom.RoomWallCheck();
    }


    public void SetRoomWWallType(Dropdown dropdown)
    {
        EditorController.Instance.SelectedRoom.WallSet = dropdown.value;
        EditorController.Instance.SelectedRoom.WallSkin = 0;
        EditorController.Instance.SelectedRoom.SetRoomMaterials();
        // EditorController.Instance.SelectedRoom.RoomWallCheck();
        PopulateWallSelectionTabs();
    }
}
