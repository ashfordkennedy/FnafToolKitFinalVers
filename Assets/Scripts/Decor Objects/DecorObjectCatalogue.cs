using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Flags]
public enum DecorTheme { }

[Flags]
public enum DecorType {Door = 1,Window = 2,Seating = 4,Surface = 8,Wall = 16,Light = 32}


[CreateAssetMenu(fileName = "ObjectCatalogue", menuName = "ScriptableObjects/ObjectCatalogue")]
public class DecorObjectCatalogue : ScriptableObject
{

    public static DecorObjectCatalogue instance;
    [Header("Map Object Settings")]
    public List<CatalogueObject> MapObjects;
    public Dictionary<string, int> ObjectDictionary = new Dictionary<string, int>();

    public void WriteDictionary()
    {
        instance = this;
        if (ObjectDictionary.Count > 0)
        {
            ObjectDictionary.Clear();
        }


        for (int i = 0; i < MapObjects.Count; i++)
        {          
            ObjectDictionary.Add(MapObjects[i].InternalName, i);

        }
        Debug.Log("dictionary written");
    }




    public string GetDecorObjectName(string internalName)
    {
        int id = -1;
       if(ObjectDictionary.TryGetValue(internalName,out id))
        {
           return MapObjects[id].name;
        }
        else
        {
            return "Decoration";
        }
    }

    public int GetDecorObjectId(CatalogueObject target)
    {
        int id = -1;
        ObjectDictionary.TryGetValue(target.InternalName, out id);     
        return id;
    }






    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
