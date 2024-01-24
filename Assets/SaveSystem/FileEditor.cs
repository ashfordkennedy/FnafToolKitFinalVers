using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


/// <summary>
/// Generic file handler for less repetetive saves
/// </summary>
public class FileEditor
{


    public static void RetrieveFileData<T>(string path,out T value)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);
        if (file.Length > 0)
        {
        
            value = (T)bf.Deserialize(file);
        }
        else
        {
           
            value = default (T);
        }
        file.Close();
    }



    public static void SaveFile<T>(string path, T data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, data);
        file.Close();
    }



}
