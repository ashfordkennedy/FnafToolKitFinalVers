using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using ObjectActionEvents;

public enum EditorMouseMode {Default,Build,Erase,NewRoom,Select, Wall, Window, Door, Off, DoorCenter, DoorLeft, DoorRight,waypointSelect,ObjectSelect,ActionSelect }
public class EditorController : MonoBehaviour
{
    
    public static EditorController Instance;
    public List<WallMeshSet> WallSets;
    public List<BuildSetMaterials> Floors;
    public List<RoomController> Rooms;
    [Space]
    [Header("Map Object Settings")]
    public DecorObjectCatalogue Catalogue;

    [Header("Map Decor List")]
    public List<DecorObject> MapDecor;
    [Space]

    [Header("ListModifiers")]
    public int SelectedIndex = 0;
    public int IndexDestination = 0;
    [Space]
    public int MapGridScale = 10;
    public bool EditModeActive = false;
    public RenderTexture PreviewCaptureTexture;
    public EditorMouseMode Editor_MouseMode = EditorMouseMode.Select;
    public RoomController SelectedRoom = null;

    [SerializeField] SaveDataScriptableObject SaveData;

    public RoomCell[,] CellRegistry = new RoomCell[46, 46];
    public GameObject NewRoomPrefab;


    public List<ObjectActionIndex> ObjectActions = new List<ObjectActionIndex>
    {   
        new ObjectActionIndex("SetGlobalPower","Set Global Power", ObjectActionType.SetFloat),
        new ObjectActionIndex("SetLightOn","Light On/Off", ObjectActionType.SetBool)
        };    

    private void Awake()
    {
        Instance = this;
        Debug.LogAssertion("check for catalogue loading");
        //Catalogue.WriteDictionary();
    }

    private void Start()
    {
        if (SaveData.LoadedFile != null) {
            if (SaveData.LoadedFile.MapName != "")
            {
                StartCoroutine(RepopulateMap());
            }
        }
        else
        {
            LoadingScreen.LoadScreen.LoadScreenToggle(false);
        }
    }




    public void RenameSelectedRoom(InputField IField)
    {
        SelectedRoom.name = IField.text;
        SelectedRoom.Name = IField.text;
        IField.placeholder.GetComponent<Text>().text = SelectedRoom.name;

        if (RoomListMenu.instance.MenuOpen == true)
        {
            RoomListMenu.instance.UpdateListContents();
        }

    }


    public void RegisterNewRoom(RoomController NewRoom)
    {
        Rooms.Add(NewRoom);
        NewRoom.name = "Room " + Rooms.Count;
        NewRoom.Name = "Room " + Rooms.Count;
        NewRoom.gameObject.name = "Room " + Rooms.Count;
        RoomListMenu.instance.UpdateListContents();
        // RoomSettingsUI.Instance.OpenRoomSettings(true);
    }

    public RoomController RegisterNewRoom()
    {


        return null;
    }

    public void DeRegisterRoom(RoomController Room)
    {
        Rooms.Remove(Room);
        Rooms.TrimExcess();
        RoomListMenu.instance.UpdateListContents();
    }

    /*
    /// <summary>
    /// Change Mouse Actions. Uses an int to change enum value (UI Event Compatibility)
    /// </summary>
    /// <param name="mouseMode"></param>
    public void ChangeMouseMode(int mouseMode)
    {
        if (ObjectTransformController.ObjectTransformGizmo.isActiveAndEnabled == true)
        {
            ObjectTransformController.ObjectTransformGizmo.StartCoroutine("DisplayTransformUI", false);
        }
        Editor_MouseMode = (EditorMouseMode)mouseMode;
        print("edtior mousemode now set to " + (EditorMouseMode)mouseMode);

        RoomEditorMouse.Instance.DeselectLastObject();

        RoomEditorMouse.Instance.CellBuildingCursor.gameObject.SetActive(false);

        ///performs specific closing actions, may not be needed
        switch (Editor_MouseMode)
        {
            case EditorMouseMode.Select:
                Editor_MouseMode = EditorMouseMode.Select;
                break;



            case EditorMouseMode.Build:
                RoomEditorMouse.Instance.CellBuildingCursor.gameObject.SetActive(true);
                Editor_MouseMode = EditorMouseMode.Build;

                break;

            case EditorMouseMode.Erase:
                RoomEditorMouse.Instance.CellBuildingCursor.gameObject.SetActive(true);
                Editor_MouseMode = EditorMouseMode.Erase;

                break;

            case EditorMouseMode.NewRoom:
                SelectedRoom = null;
                RoomEditorMouse.Instance.CellBuildingCursor.gameObject.SetActive(true);
                RoomSettingsUI.Instance.CloseMenu();
                Editor_MouseMode = EditorMouseMode.Build;

                break;

            case EditorMouseMode.Door:
                Editor_MouseMode = EditorMouseMode.Door;
                break;


            case EditorMouseMode.DoorLeft:
                Editor_MouseMode = EditorMouseMode.DoorLeft;
                break;

            case EditorMouseMode.DoorRight:
                Editor_MouseMode = EditorMouseMode.DoorRight;
                break;


        }


    }
    
    */

    public void PlaceObjectSetup(int ID)
    {
        ObjectPlacementWidgit.PlacementWidgit.CreateBlueprint(Catalogue.MapObjects[ID].Object);
        //   RoomEditorMouse.Instance.enabled = false;    
        print("Placed object set up has been called");

    }



    public void NewRoomSelectHandler(RoomController NextRoom)
    {
        // close selection panel
       // ObjectTransformController.ObjectTransformGizmo.OpenTransformController(false, null);


        if (SelectedRoom == NextRoom)
        {
            print("no need to reprocess");
        }

        else if (SelectedRoom != null)
        {
            SelectedRoom.EditorDeselect();
            SelectedRoom = NextRoom;
            SelectedRoom.EditorSelect();
        }

        else
        {
            SelectedRoom = NextRoom;
            SelectedRoom.EditorSelect();

        }

    }


    /// <summary>
    /// recreates the map based on save data.
    /// </summary>
    /// <returns></returns>
    public IEnumerator RepopulateMap()
    {
        var FileData = SaveData.LoadedFile;

        for (int i = 0; i < FileData.LevelEnvironmentData.Roomdata.Count; i++)
        {
            RecreateRoom(FileData.LevelEnvironmentData.Roomdata[i]);
        }


        for (int i = 0; i < FileData.DecorList.Count; i++)
        {
            RecreateObjects(FileData.DecorList[i]);
            print("recreating object " + FileData.DecorList[i].InternalName);
        }

        for (int i = 0; i < FileData.DecorList.Count; i++)
        {
            print("recreating settings for " + FileData.DecorList[i].InternalName);
            try
            {
                RecreateObjectSettings(i, FileData.DecorList[i]);
            }
            catch (Exception)
            {
                print("Error restoring settings for object " + FileData.DecorList[i].InternalName);
                throw;
            }
           
            
        }


        //re-add night settings
        NightManager.instance.nightSettings = FileData.NightSettings;
        NightSettingsPanel.instance.UpdateDisplay();


        //drop the load screen. she ain't needed
        LoadingScreen.LoadScreen.LoadScreenToggle(false);







        yield return null;
    }

    /// <summary>
    /// Instantiates Saved objects back into the environment
    /// </summary>
    /// <param name="newObj"></param>
    void RecreateObjects(SavedObject newObj)
    {
        int objectID = -1;
        GameObject prefab = Catalogue.GetObjectPrefab(newObj.InternalName);
        CatalogueObject catalogueObject = Catalogue.GetCatalogueData(newObj.InternalName);
        if (prefab != null)
        {

            var Pos = newObj.positionData;
            var position = new Vector3(Pos.X, Pos.Y, Pos.Z);
            var rotation = new Vector3(Pos.XRot, Pos.YRot, Pos.ZRot);
            var scale = new Vector3(Pos.XScale, Pos.YScale, Pos.ZScale);

            DecorObject obj = Instantiate(prefab, null, true).GetComponent<DecorObject>();
            obj.transform.position = position;
            obj.transform.rotation = Quaternion.Euler(rotation);
            obj.transform.localScale = scale;

            // give object its swatch back

            if (newObj.Swatch != 0)
            {
                print("loading swatch");

               obj.SwatchSwap(catalogueObject.Swatches[newObj.Swatch].meshes, catalogueObject.Swatches[newObj.Swatch].materials, newObj.Swatch);
            }

            obj.ObjectSetup();
        }
    }


    void RecreateObjectSettings(int ObjIndex, SavedObject savedObject)
    {
        DecorObject obj = MapDecor[ObjIndex];

        obj.RestoreObjectData(savedObject.ObjectData);



        try
        {
            switch (savedObject.ObjectData.DataType)
            {

                case ObjectSaveDataType.Waypoint:
                    print("waypoint seting up");


                    var W = obj as AnimatronicWaypoint;

                    WaypointSaveData WSD = savedObject.ObjectData as WaypointSaveData;
                    W.RestoreWaypointData(WSD);
                    break;


                case ObjectSaveDataType.ClassicStart:
                    var Cs = obj as DecorClassicStart;
                    ClassicStartData CSD = savedObject.ObjectData as ClassicStartData;
                    Cs.RestoreObjectData(CSD);
                    break;

                case ObjectSaveDataType.ButtonSwitch:
                    var button = obj as DecorButton;
                    ButtonSaveData BSD = savedObject.ObjectData as ButtonSaveData;
                    button.RestoreObjectSave(BSD);
                    break;

            }
        }
        catch (Exception)
        {
            print("failed to restore object settings");
            throw null;
        }
            
        }


  





    /// <summary>
    /// recreates room data and populates map with cells from saved data
    /// </summary>
    /// <param name="NewRoom"></param>
    void RecreateRoom(RoomData NewRoom)
    {

        var NewCtrl = new GameObject().gameObject.AddComponent<RoomController>();
        NewCtrl.gameObject.transform.position = new Vector3(0, 0, 0);


        var newRoomRenderer = new GameObject("CombinedRoom");       
        NewCtrl.CombinedRoomRenderer = newRoomRenderer.gameObject.AddComponent<MeshRenderer>();
        newRoomRenderer.transform.SetParent(NewCtrl.transform);

        var newfilter = newRoomRenderer.gameObject.AddComponent<MeshFilter>();
        newfilter.gameObject.layer = 13;
        newfilter.gameObject.AddComponent<MeshCollider>();
        NewCtrl.CombinedRoomMesh = newfilter;


        NewCtrl.FloorSet = NewRoom.FloorSet;
        NewCtrl.WallSet = NewRoom.WallSet;
        NewCtrl.WallSkin = NewRoom.WallSkin;
        var newContainer = new GameObject("EditCell Container");
        newContainer.transform.SetParent(NewCtrl.transform);
        NewCtrl.EditModeContainer = newContainer;

        
        EditorController.Instance.RegisterNewRoom(NewCtrl);
        NewCtrl.SetRoomName(NewRoom.Name);

        EditorController.Instance.SelectedRoom = NewCtrl;


        for (int i = 0; i < NewRoom.RoomCells.Count; i++)
        {
            RecreateCell(NewRoom.RoomCells[i], NewCtrl, NewRoom);
        }
        NewCtrl.RoomWallCheck();

    }

    void RecreateCell(CellSaveData cell, RoomController RCtrl, RoomData roomData)
    {

        RoomCell NewCell = Instantiate(RoomEditorMouse.Instance.RoomCellPrefab).GetComponent<RoomCell>();

        //  NewCell.gameObject.transform.position = cell.ID;

      var newPos = new Vector3(cell.ID.X * EditorController.Instance.MapGridScale, 0, cell.ID.Y * EditorController.Instance.MapGridScale);

        NewCell.transform.position = newPos;
        NewCell.Room_Ctrl = EditorController.Instance.SelectedRoom;
        NewCell.transform.SetParent(EditorController.Instance.SelectedRoom.EditModeContainer.transform, true);

        var RoomCenter = EditorController.Instance.SelectedRoom.transform.localPosition;
        int Xdistance = (int)NewCell.transform.localPosition.x / EditorController.Instance.MapGridScale;
        int Zdistance = (int)NewCell.transform.localPosition.z / EditorController.Instance.MapGridScale;

        NewCell.CellID = new Vector2Int(Xdistance, Zdistance);
        RCtrl.RegisterCell(new Vector2Int(cell.ID.X, cell.ID.Y), NewCell,false);
        NewCell.ReconstructCell(cell, NewCell.Room_Ctrl.WallSet,NewCell.Room_Ctrl.WallSkin,roomData.FloorSet);
      //  NewCell.CellInitialize();
    }




    public void RegisterDecorObject(DecorObject newObject)
    {
        if (MapDecor.Contains(newObject) == false){
            MapDecor.Add(newObject);
        }

    }




    public void DeRegisterDecorObject(DecorObject targetObject)
    {

        var id = MapDecor.IndexOf(targetObject);

        if(id != -1)
        {
            var obj = MapDecor[id].gameObject;
            MapDecor.RemoveAt(id);
            Destroy(obj);
        }

        /* God your an idiot for using a loop. how old is this mess?
        for (int i = 0; i <= MapDecor.Count; i++)
        {

            if(MapDecor[i] == targetObject)
            {
                MapDecor.RemoveAt(i);
                break;
            }
        }
        */

    }

    public void RegisterCell(Vector2Int pos, RoomCell cell)
    {
        CellRegistry[pos.x, pos.y] = cell;

    }

    public void DeregisterCell(Vector2Int pos)
    {
        CellRegistry[pos.x, pos.y] = null;
        CameraMapGenerator.instance.EraseCell(pos);
    }

    public void CenterToRoom(Transform target)
    {
        Rooms[target.GetSiblingIndex()].EditorHighlight();
    }
    
    public void DestroyRoom(Transform target)
    {
        Rooms[target.GetSiblingIndex()].DestroyRoom();
    }




    public void CloseAllMenus()
    {
        
        RoomListMenu.instance.CloseMenu();
        RoomSettingsUI.Instance.CloseMenu();
        LightSettingUI.Instance.ToggleLightUI(false);
        SwatchUI.Instance.CloseMenu();
        NightSettingsPanel.instance.CloseMenu();
        WaypointSettingsPanel.Instance.CloseMenu();
        RoomEditorMouse.Instance.ToggleActive(false);
      //  ObjectTransformController.
    }

    public void OptimiseRooms()
    {
        for (int i = 0; i < Rooms.Count; i++)
        {
            Rooms[i].SwapRoomEditMode(false);
        }
    }

    public void CreateRoom(RoomCell NewCell)
    {
        RoomController NewCtrl = Instantiate(NewRoomPrefab).GetComponent<RoomController>();
        SelectedRoom = NewCtrl;
        NewCell.transform.SetParent(NewCtrl.EditModeContainer.transform, true);
        
        NewRoomSelectHandler(NewCtrl);
        RegisterNewRoom(NewCtrl);
        RoomSettingsUI.Instance.OpenMenu();
    }

}




public enum EditorTab {Decor,GamePlay,Animatronic}
[Serializable]
public class MapObject
{
    public string name = "Object";
    public string InternalName = "";
    public EditorTab editorTab = EditorTab.Decor;
    public List<ObjectSwatch> Swatches;
    [TextArea] public string Description = "";
    public int Price;
    public GameObject Object;
    public Sprite Menusprite;
    public int ObjectId;
    public DecorType DecorTabType;

}


/// <summary>
/// swatches are variants of a single object and used 
/// </summary>
[Serializable] 
public class ObjectSwatch
{
    public Mesh[] mesh;
        public Material[] material;
    public Sprite Swatch;
}


[Serializable]
public class WallSet
{
    public string name;
    [Header("UI info")]
    public Sprite UISprite;
   
    [Header("---Trial Format---")]
    [Header("0 Blank, 1 Window, 2 Door, 3 Vent, 4 DoorLeft, 5 DoorCenter, 6 DoorRight }")]
    [Header("Fill in as appropriate with each variant under the indexs above, both corners do not need window and door pieces}")]
    public Mesh[] StraightWalls;
    public Mesh[] LeftCornerWalls;
    public Mesh[] RightCornerWalls;
    public Mesh[] BothCornerWalls;
    [Header("Inner Corner")]
    public Mesh InnerCorner;


    [Header("Materials")]
    public Material WallMaterials;
    public List<BuildSetMaterials> MaterialSets;
   

}

[Serializable]
public class BuildSetMaterials
{
    public string name;
    public Material material;
    public Sprite Icon;
}





/// <summary>
/// Data type containing an animatronics waypoint and AI settings to be reloaded later. stored inside the animatronic decor save class
/// </summary>
[Serializable]
public class AnimatronicData : ObjectSaveData
{
    public List<int> AiLevel = new List<int>(7);
    public List<int> Aggression = new List<int>(7);
    public List<AnimatronicSavedWaypoint> waypointData = new List<AnimatronicSavedWaypoint>();

  /*
    public void StoreWaypointData(List<TargetWaypointData> targetWaypoints)
    {
        for (int i = 0; i < targetWaypoints.Count; i++)
        {
            targetWaypoints[i].GenerateSavedWaypointData();
        }
    }
    */

    public AnimatronicData(ObjectSaveDataType dataType, EditorAnimatronic animatronic) : base(dataType)
    {
        this.DataType = ObjectSaveDataType.Animatronic;
        this.AiLevel = animatronic.AiLevelData;
        this.Aggression = animatronic.AggressionData;

        for (int i = 0; i < animatronic.waypoints.Count; i++)
        {
            waypointData.Add(new AnimatronicSavedWaypoint(animatronic.waypoints[i]));
        }
    }

    /// <summary>
    /// Converts the saved waypoints into editor compatible waypoints
    /// </summary>
    /// <returns></returns>
    public List<TargetWaypointData> ConvertStoredWaypoints()
    {
      var waypoints = new List<TargetWaypointData>();

        for (int i = 0; i < waypointData.Count; i++)
        {
            var condition = new WaypointCondition(waypointData[i].condition);
            var waypoint = (AnimatronicWaypoint)EditorController.Instance.MapDecor[waypointData[i].waypointDecorIDTarget];
            var waypointdata = new TargetWaypointData(waypoint, waypointData[i].waypointName);
            waypointdata.condition = condition;

            waypoints.Add(waypointdata);
        }
        return waypoints;
    }

}
/// <summary>
/// Class containing the save friendly version of an animatronic waypoint to be stored by AnimatronicData class
/// </summary>
[Serializable]
public class AnimatronicSavedWaypoint
{
    public int waypointDecorIDTarget;
    public string waypointName = "";
    public int waypointID = -1;
    public WaypointConditionSavable condition;


    public AnimatronicSavedWaypoint(TargetWaypointData waypoint)
    {
        this.waypointDecorIDTarget = EditorController.Instance.MapDecor.IndexOf(waypoint.waypoint);
        this.waypointName = waypoint.waypointName;
        this.condition = waypoint.GenerateSavedWaypointData();
    }


}
