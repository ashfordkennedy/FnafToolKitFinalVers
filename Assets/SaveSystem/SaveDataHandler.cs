using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
public enum FileType {Local,Community}
public class SaveDataHandler : MonoBehaviour
{
    public static SaveDataHandler SaveHandler;

   [SerializeField]public string IconBuffer = "";
    [SerializeField] public string OverwritePath = "";
    
    public FileType LoadMode = FileType.Local;


    [SerializeField]public SaveDataScriptableObject SaveData = null;
    
  
    public string demosavename = "";
    public MapSaveFile testsavefull;
    public LevelConstructionData testsave;




    public void Awake()
    {
       // if (SaveHandler == null)
       // {
            SaveHandler = this;
            SaveData.GenerateData();
       
         //   DontDestroyOnLoad(this.gameObject);
       // }

       // else
       // {
        //    Destroy(this.gameObject);
       // }
    }



    /// <summary>
    /// Performs SaveLevel(), but allows for the generation of the files Icon string before saving.
    /// </summary>
    /// <param name="SaveFileName"></param>
    /// <returns></returns>
    public IEnumerator SaveLevelProcess(string SaveFileName, string creatorName, string bio,bool editable, SaveMode saveMode)
    {
        if (IconBuffer == "")
        {
            var icon = new Texture2D(256, 256, TextureFormat.RGB24, false);
            UIController.instance.HideCanvas(true);
            StartCoroutine("RenderIcon", icon);
            yield return new WaitUntil(() => IconBuffer != "");
        }
        UIController.instance.HideCanvas(false);
        SaveLevel(SaveFileName, creatorName, bio, editable, saveMode);


        yield return null;
    }




    public void SaveLevel(string SaveFileName,string creatorName, string bio, bool editable, SaveMode saveMode)
    {
        #region map_data
        LevelConstructionData NewLayoutData = new LevelConstructionData(new List<RoomData>());

     

        // room packager
        for (int i = 0; i < EditorController.Instance.Rooms.Count; i++)
        {
            RoomController room = EditorController.Instance.Rooms[i];
            
            List<CellSaveData> NewRoomCells = new List<CellSaveData>();


            // Generate Cell info
            for (int x = 0; x <room.RoomCells.GetLength(0); x++)
            {
                for (int y = 0; y < room.RoomCells.GetLength(1); y++)
                {
                   var Cell = room.RoomCells[x, y];

                    if (Cell != null)
                    {


                        var newCell = new CellSaveData(new RoomCordinates(x,y),Cell.Walls[0].Wall_State, Cell.Walls[0].Wall_Type, Cell.Walls[1].Wall_State, Cell.Walls[1].Wall_Type, Cell.Walls[2].Wall_State, Cell.Walls[2].Wall_Type, Cell.Walls[3].Wall_State, Cell.Walls[3].Wall_Type,
                            Cell.InnerCornerRenderers[0].gameObject.activeInHierarchy, Cell.InnerCornerRenderers[1].gameObject.activeInHierarchy, Cell.InnerCornerRenderers[2].gameObject.activeInHierarchy, Cell.InnerCornerRenderers[3].gameObject.activeInHierarchy);

                        NewRoomCells.Add(newCell);

                    }
        
                }

            }

                    // Final constructor
                    RoomData NewRoomData = new RoomData(room.Name,NewRoomCells,room.FloorSet,room.WallSet,room.WallSkin);
            print("room name is " + room.Name);
            NewLayoutData.Roomdata.Add(NewRoomData);
        }



        List<SavedObject> decorObjs = new List<SavedObject>();

        // generate map decor list
        if (EditorController.Instance.MapDecor.Count > 0)
        {
            for (int i = 0; i < EditorController.Instance.MapDecor.Count; i++)
            {
                decorObjs.Add(EditorController.Instance.MapDecor[i].CompileObjectData());


            }
        }

    #endregion




        //test function, don't bother with it unless you need to query saving
        testsave = NewLayoutData;

        var nightSettings = NightManager.instance.nightSettings;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = null;
        string directory = "";
        if (saveMode == SaveMode.newSave) {
            if (File.Exists(SaveData.SavedMapDirectory + "/" + SaveFileName + ".FVLev") == false)
            {
                //creates save file if cant be found;
                file = File.Create(SaveData.SavedMapDirectory + "/" + SaveFileName + ".FVLev");

            }

            //grab maps night settings for constructor
            
        }
        else
        {
            // nothing needed as its running off of a preestablished file
            /*
            int id = SaveMenu.instance.SelectedFileId;
            if (File.Exists(SaveData.SelectedFile.Directory) == false)
            {
                //creates save file if cant be found;
                file = File.Create(SaveData.SavedMapDirectory + "/" + SaveFileName + ".FVLev");

            }
            else
            {
                file = file. SaveData.SelectedFile.Directory
            }
            file = File.Create(SaveData.SelectedFile.Directory);
            */
            directory = OverwritePath;
            file = File.Open(directory, FileMode.Open);
            
        }
        MapSaveFile NewSave = new MapSaveFile(IconBuffer,SaveFileName,creatorName,bio, NewLayoutData, decorObjs,nightSettings,editable, directory);
        Debug.Log("Saved game");


        

        bf.Serialize(file, NewSave);       
        file.Close();
        if (saveMode == SaveMode.Overwrite)
        {
            File.Move(file.Name, SaveFileName);
        }
        IconBuffer = "";
    }


    public IEnumerator RenderIcon(Texture2D icon)
    {
        yield return new WaitForEndOfFrame();
        RenderTexture.active = Camera.main.targetTexture;

        Rect Snapshot = new Rect((Screen.width / 2) - 128, (Screen.height / 2) - 128,256,256);
        
        icon.ReadPixels(Snapshot, 0, 0);
        icon.Apply();
        yield return new WaitForEndOfFrame();
        IconBuffer = ImageConversion.textureToString(icon);
        Camera.main.targetTexture = null;
        yield return null;

    }
    


    

    public static FilePreview RetreiveFileInfo(FileType MapSource, int Index)
    {
        FilePreview Info = null;
        switch (MapSource)
        {
            case FileType.Community:

                Info = SaveHandler.SaveData.CommunityMaps[Index];
              
                break;

            case FileType.Local:

                Info = SaveHandler.SaveData.LocalMaps[Index];
                break;

        }

        return Info;

    }



   













}

[Serializable]
public class MapSaveFile
{
    public string PreviewImage;
    public string MapName;
    public string MapCreator;
    public string Bio;
    public bool Editable;
    public string Directory;
    public LevelConstructionData LevelEnvironmentData;
    public List<SavedObject> DecorList;
    public List<NightSettings> NightSettings;
    public PlayerStats playerStats;

    public MapSaveFile(string MapImage, string mapName, string Creator, string Bio, LevelConstructionData ConstructData, List<SavedObject> decorList, List<NightSettings> NightSettings, bool editable, string Directory)
    {
        this.PreviewImage = MapImage;
        this.MapName = mapName;
        this.MapCreator = Creator;
        this.LevelEnvironmentData = ConstructData;
        this.DecorList = decorList;
        this.NightSettings = NightSettings;
        this.Bio = Bio;
        this.Editable = editable;
        this.Directory = Directory;

    }
}



[Serializable]
public class PlayerStats
{
    public float upperBound = 45f;
    public float lowerBound = 45f;
}



[Serializable]
public class LevelConstructionData
{
    public List<RoomData> Roomdata;

    public LevelConstructionData(List<RoomData> roomdata)
    {
        this.Roomdata = roomdata;

    }
}


/// <summary>
/// Container for each rooms relevant data. Only usable information such as wall state and stripped down cell data are needed
/// </summary>
[Serializable]
public class RoomData
{
    public string Name;
    public List<CellSaveData> RoomCells;
    public int WallSet = 0;
    public int WallSkin = 0;
    public int FloorSet = 0;
  


    public RoomData(string Name, List<CellSaveData> roomCells, int floorset, int wallset,int wallSkin)
    {
        this.Name = Name;
        this.RoomCells = roomCells;
        this.FloorSet = floorset;
        this.WallSkin = wallSkin;
        this.WallSet = wallset;     
    }
}

[Serializable]
public struct RoomCordinates
{
   public int X;
   public int Y;

    public RoomCordinates(int x,int y)
    {
        this.X = x;
        this.Y = y;
    }
}


[Serializable]
public class CellSaveData
{
    public RoomCordinates ID;
    public WallState NorthWall;
    public WallState EastWall;
    public WallState SouthWall;
    public WallState WestWall;

    public WallType NorthType;
    public WallType EastType;
    public WallType SouthType;
    public WallType WestType;

    public bool Corner0;
    public bool Corner1;
    public bool Corner2;
    public bool Corner3;

    public CellSaveData(RoomCordinates id,WallState north,WallType northtype, WallState east, WallType easttype, WallState south, WallType southtype, WallState west, WallType westtype, bool corner0,  bool corner1, bool corner2, bool corner3)
    {
        this.ID = id;
        this.NorthWall = north;
        this.EastWall = east;
        this.SouthWall = south;
        this.WestWall = west;
        this.Corner0 = corner0;
        this.Corner1 = corner1;
        this.Corner2 = corner2;
        this.Corner3 = corner3;
        this.NorthType = northtype;
        this.SouthType = southtype;
        this.EastType = easttype;
        this.WestType = westtype;

    }
}

[System.Serializable]
public class SavedTransform
{
    public float X;
    public float Y;
    public float Z;
    public float XRot;
    public float YRot;
    public float ZRot;
    public float XScale;
    public float YScale;
    public float ZScale;


    public SavedTransform(Vector3 pos, Vector3 rot, Vector3 scale)
    {
        this.X = pos.x;
        this.Y = pos.y;
        this.Z = pos.z;
        this.XRot = rot.x;
        this.YRot = rot.y;
        this.ZRot = rot.z;
        this.XScale = scale.x;
        this.YScale = scale.y;
        this.ZScale = scale.z;
    }

    public SavedTransform(Transform transform)
    {
        this.X = transform.position.x;
        this.Y = transform.position.y;
        this.Z = transform.position.z;
        this.XRot = transform.rotation.eulerAngles.x;
        this.YRot = transform.rotation.eulerAngles.y;
        this.ZRot = transform.rotation.eulerAngles.z;
        this.XScale = transform.lossyScale.x;
        this.YScale = transform.lossyScale.y;
        this.ZScale = transform.lossyScale.z;

    }

}



/// <summary>
/// base class for a saved decor object, Inherit and expand to store specific data
/// </summary>
[Serializable]
public class SavedObject
{
    public string InternalName = "";
    public int Swatch = 0;
    public ObjectSaveData ObjectData;
    public SavedTransform positionData;
    public SavedObject(string internalName,int swatch, ObjectSaveData objectData, SavedTransform positiondata)
    {
        this.InternalName = internalName;
        this.Swatch = swatch;
        this.ObjectData = objectData;
        this.positionData = positiondata;
    }
}

[Serializable]
public class SavedLight : SavedObject
{



    public SavedLight(string internalName,int swatch, ObjectSaveData objectData, SavedTransform positiondata) : base(internalName,swatch,objectData, positiondata)
    {
        this.InternalName = internalName;
        this.ObjectData = objectData;
        this.Swatch = swatch;
        this.positionData = positiondata;
    }
}

[Serializable]
public class SavedAnimatronic : SavedObject
{

    public SavedAnimatronic(string internalName, int swatch, ObjectSaveData objectData, SavedTransform positiondata) : base(internalName, swatch, objectData, positiondata)
    {
        this.InternalName = internalName;
        this.ObjectData = objectData;
        this.Swatch = swatch;
        this.positionData = positiondata;
    }
}