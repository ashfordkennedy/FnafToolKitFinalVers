using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;


[CreateAssetMenu (fileName ="SaveFileData",menuName ="SaveObjects/SaveFileData",order = 0)]
public class SaveDataScriptableObject : ScriptableObject
{
    public string CommunityMapDirectory = "";
    public string SavedMapDirectory = "";

    [Header("FilePreviewLists")]
    public List<FilePreview> CommunityMaps;
    public List<FilePreview> LocalMaps;
    public MapSaveFile SelectedFile;

    public MapSaveFile LoadedFile = null;

    [SerializeField] public MapProgressData mapProgressData;
    /// <summary>
    /// Sets up scriptableObject for use
    /// </summary>
    public void GenerateData()
    {
        CommunityMapDirectory = Application.persistentDataPath + "/CommunityMaps";
        SavedMapDirectory = Application.persistentDataPath + "/LocalMaps";

        Directory.CreateDirectory(CommunityMapDirectory);
        Directory.CreateDirectory(SavedMapDirectory);

        PopulateMapLists();
        mapProgressData.CheckForDirectory();
    }


    /// <summary>
    /// Finds, filters and creates file Info listings, UI List population is handled seperately
    /// </summary>
    public void PopulateMapLists()
    {

        // these values are recycled for each loop
        var info = new DirectoryInfo(SavedMapDirectory);
        var MapsRaw = info.GetFiles();

        LocalMaps.Clear();
        CommunityMaps.Clear();


        if (MapsRaw.Length > 0)
        {

            for (int i = 0; i < MapsRaw.Length; i++)
            {

                if (MapsRaw[i].Extension == ".FVLev" && MapsRaw.Length > 0)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(SavedMapDirectory + "/" + MapsRaw[i].Name, FileMode.Open);
                    MapSaveFile MapFile = (MapSaveFile)bf.Deserialize(file);

                    MapFile.Directory = SavedMapDirectory + "/" + MapsRaw[i].Name + ".FVLev";
                    FileInfo FD = MapsRaw[i];


                    LocalMaps.Add(new FilePreview(FD.CreationTime.ToShortDateString(), FD.LastWriteTime.ToShortDateString(), MapFile.PreviewImage, MapFile.MapName, "" + MapFile.MapCreator, MapsRaw[i].Directory.FullName + "/" + MapsRaw[i].Name));
                    file.Close();


                }
            }

            info = new DirectoryInfo(CommunityMapDirectory);
            MapsRaw = info.GetFiles();

            for (int i = 0; i < MapsRaw.Length; i++)
            {

                if (MapsRaw[i].Extension == ".FVLev" && MapsRaw.Length > 0)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(CommunityMapDirectory + "/" + MapsRaw[i].Name, FileMode.Open);
                    MapSaveFile MapFile = (MapSaveFile)bf.Deserialize(file);

                    MapFile.Directory = CommunityMapDirectory + "/" + MapsRaw[i].Name + ".FVLev";
                    FileInfo FD = MapsRaw[i];


                    LocalMaps.Add(new FilePreview(FD.CreationTime.ToShortDateString(), FD.LastWriteTime.ToShortDateString(), MapFile.PreviewImage, MapFile.MapName, "" + MapFile.MapCreator, MapsRaw[i].Directory.FullName + "/" + MapsRaw[i].Name));
                    file.Close();


                }
            }



        }





    }

    public MapSaveFile RetriveFullMapFile(string Directory)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Directory, FileMode.Open);
        MapSaveFile MapFile = (MapSaveFile)bf.Deserialize(file);
        MapFile.Directory = Directory;
        file.Close();
        return MapFile;
    }

}
