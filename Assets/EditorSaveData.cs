using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EditorSaveData : MonoBehaviour
{
    public Texture2D texture;
    public Byte[] FileImage;

    public EditorSaveData(Byte[] fileImage)
    {

        this.FileImage = fileImage;


    }




}
