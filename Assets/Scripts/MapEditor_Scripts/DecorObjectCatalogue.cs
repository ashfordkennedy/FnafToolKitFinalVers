using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DecorTheme { }
public enum DecorType {Door,Window,Seating,Surface,Wall }


[CreateAssetMenu(fileName = "ObjectCatalogue", menuName = "ScriptableObjects/ObjectCatalogue")]
public class DecorObjectCatalogue : ScriptableObject
{

    [Header("Map Object Settings")]
    public List<MapObject> MapObjects;
    public Dictionary<string, int> ObjectDictionary = new Dictionary<string, int>();



    public void WriteDictionary()
    {
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











    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
