using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
[CreateAssetMenu(fileName = "MapProgressData", menuName = "SaveObjects/MapProgressData", order = 1)]
public class MapProgressData : ScriptableObject
{
    string m_progressPath = "";

    public List<MapProgress> MapProgressList;



    public void CheckForDirectory()
    {
        Debug.Log("progress data retrieval running");
        m_progressPath = Application.persistentDataPath + "/GameData";

        if (Directory.Exists(m_progressPath) == false)
        {
            Directory.CreateDirectory(m_progressPath);
        }


        if (File.Exists(m_progressPath + "/LevelProgress") == false)
        {
            File.Create(m_progressPath + "/LevelProgress");
        }
        else
        {
            LoadProgressFromFile();
        }


      //  if(MapProgressList.Count < 0)
       // {
            
       // }
    }


    public void SaveProgress()
    {
     //  var file = File.Open(m_progressPath + "/LevelProgress", FileMode.Open);
      //  BinaryFormatter bf = new BinaryFormatter();

        FileEditor.SaveFile<List<MapProgress>>(m_progressPath + "/LevelProgress", MapProgressList);
    }


    public void LoadProgressFromFile()
    {
        //  var file = File.Open(m_progressPath + "/LevelProgress", FileMode.Open);
        // BinaryFormatter bf = new BinaryFormatter();
        FileEditor.RetrieveFileData<List<MapProgress>>(m_progressPath + "/LevelProgress", out MapProgressList);
            //(List<MapProgress>)bf.Deserialize(file);
            // file.Close();
    }









    public void CompleteNight(MapSaveFile saveFile)
    {
        if (saveFile != null){ 
        string mapID = saveFile.MapName + File.GetCreationTime(saveFile.Directory);

        var IdMatch = MapProgressList.FirstOrDefault(m => m.mapID == mapID);
        if (IdMatch != null)
        {
            IdMatch.AdvanceNight();
        }
        else
        {
            MapProgressList.Add(new MapProgress(saveFile));
        }

        SaveProgress();
    }
    }

}

[Serializable]
public class MapProgressSaveData
{


}



[Serializable]
public class MapProgress
{
    public string mapID = "";
    public int CurrentNight = 1;

    public MapProgress(MapSaveFile saveFile)
    {
        mapID = saveFile.MapName + File.GetCreationTime(saveFile.Directory);
        CurrentNight = 1;

    }

    public void AdvanceNight()
    {      
        if(CurrentNight < 7)
        {
            CurrentNight += 1;
        }
    }

}
