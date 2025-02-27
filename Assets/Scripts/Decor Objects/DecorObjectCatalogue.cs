using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
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




    public CatalogueObject GetCatalogueData(string internalName)
    {
        var foundObject = MapObjects.First(n => n.InternalName == internalName);
        return foundObject;

    }



    public string GetDecorObjectName(string internalName)
    {
       var foundObject = MapObjects.First(n => n.InternalName == internalName);


       if(foundObject != null)
        {
           return foundObject.name;
        }
        else
        {
            return "Unnamed_Object";
        }
    }

    public int GetDecorObjectId(CatalogueObject target)
    {
        int id = -1;
        // ObjectDictionary.TryGetValue(target.InternalName, out id);
        Debug.Log("this method is redundant, get rid of all callers");
        return id;
    }

    public GameObject GetObjectPrefab(string internalName)
    {
        var foundObject = MapObjects.First(n => n.InternalName == internalName);

        if(foundObject != null)
        {
            return TryLoadPrefab(internalName);
        }
        return null;
    }

    private GameObject TryLoadPrefab(string internalName)
    {
        try
        {
            GameObject resource = Resources.Load<GameObject>("CatalogueObjects/" + internalName);
            Debug.Log(resource.name);
            Debug.Log("Loaded resource " + internalName);
            return resource;
        }
        catch (System.Exception)
        {
            Debug.Log("Unable to load resource " + internalName);
            return null;
            throw;
        }
    }

    public void UnloadUnusedAssets()
    {
        Resources.UnloadUnusedAssets();
        Debug.Log("Unloaded unused assets");
    }


}
