using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
public class CatalogueSwatchGenerator : MonoBehaviour
{

    public DecorObjectCatalogue catalogue;
    public float DefaultCameraDistance;
    public Vector3 DefaultCameraPosition;
    public GameObject ListButton;
    public Transform ListContents;
    public GameObject TargetObject;
    // Start is called before the first frame update
    void Start()
    {
        PopulateList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void SpawnObject(Transform trigger)
    {
        int ID = trigger.GetSiblingIndex();
       var newobj = Instantiate(catalogue.MapObjects[ID].Object, Vector3.zero,Quaternion.Euler(new Vector3(-90,180,0)),null);

        if(TargetObject != null)
        {
            Destroy(TargetObject);
        }

        TargetObject = newobj;
    }


    public void PopulateList()
    {
        for (int i = 0; i < catalogue.MapObjects.Count; i++)
        {
            var button = Instantiate(ListButton, ListContents).GetComponent<Button>();
            button.GetComponentInChildren<Text>().text = catalogue.MapObjects[i].name;
            button.onClick.AddListener(delegate { SpawnObject(button.transform); });

        }

    }


    public void Savecameraastexture()
    {
     //   var icon = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

     
    //    icon.ReadPixels(new Rect(Screen.width /2, Screen.height /2, Screen.width / 2, Screen.height / 2), 0, 0, false);
     //   icon.Apply();


      //  byte[] bytes = icon.EncodeToPNG();


        byte[] bytes = toTexture2D(rt).EncodeToPNG();

        var dirPath = Application.dataPath + "/RawIcon/";

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        File.WriteAllBytes(dirPath + UnityEngine.Random.Range(10000,99999) + ".png", bytes);
        // UnityEditor.AssetDatabase.CreateAsset(icon.enc, "Assets/RawIcons/");


    }


    public RenderTexture rt;
    // Use this for initialization
    public void SaveTexture()
    {
        byte[] bytes = toTexture2D(rt).EncodeToPNG();
        System.IO.File.WriteAllBytes("C:/Users/egsha/SavedScreen.png", bytes);
    }


    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        Destroy(tex);//prevents memory leak
        return tex;
    }








}
