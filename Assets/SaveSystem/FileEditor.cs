using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class FileEditor : MonoBehaviour
{


    public static void RetrieveFileData<T>(string path,out T value)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);
        if (file.Length > 0)
        {
            print("file contains data");
            value = (T)bf.Deserialize(file);
        }
        else
        {
            print("file empty");
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
