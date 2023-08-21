using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ObjectActionEvents;
public enum DecorObjectType { Basic, Door, Light, Animatronic, Waypoint, Camera, ClassicStart, Button };

/// <summary>
/// Decor object is a base class for all objects
/// </summary>

public class DecorObject : MonoBehaviour
{
    public bool canInteract => EditorController.Instance.EditModeActive;
    public bool selected = false;
    public DecorObjectType decorType = DecorObjectType.Basic;
    public string InternalName;
    public int SwatchID;
    public Renderer meshRenderer;
    public static List<Dropdown.OptionData> WaypointConditions = new List<Dropdown.OptionData>
    {   new Dropdown.OptionData("None"),
    };

    public static List<ObjectActionIndex> ObjectActions = new List<ObjectActionIndex>
    {   new ObjectActionIndex("None","None",ObjectActionType.none),
        new ObjectActionIndex("SetLightIntensity","Set Intensity",ObjectActionType.SetFloat),
        new ObjectActionIndex("SetLightOn","Light On/Off",ObjectActionType.SetBool)
    };


    public virtual List<Dropdown.OptionData> GetWaypointConditionOptions()
    {
        return WaypointConditions;
    }

    public virtual List<ObjectActionIndex> GetObjectActions()
    {

        return ObjectActions;
    }

    public virtual waypointConditionSetting GetConditionEnum(string condition)
    {
        waypointConditionSetting value = waypointConditionSetting.none;

        switch (condition)
        {
            case "None":
                value = waypointConditionSetting.none;
                break;

            case "":

                break;
        }


        return value;
    }


    public virtual void EditorSelect()
    {
        if (selected == false)
        {
            selected = true;
            print("OBJECT SELECTED");
        }
    }

    public virtual void EditorDeselect()
    {

        if (selected == true) {
            selected = false;
            print("OBJECT DESELECTED");
        }
    }


    public virtual void SwatchSwap(Mesh[] mesh,Material[] mats,int swatchid)
    {
        var filter = this.gameObject.GetComponent<MeshFilter>();
        var rend = this.gameObject.GetComponent<MeshRenderer>();
        SwatchID = swatchid;
        filter.mesh = mesh[0];
        rend.materials = mats;
    }
        
   
    /// <summary>
    /// method called before the start of each night in-game
    /// </summary>
    public virtual void NightStartSetup()
    {

    }



    public virtual void ObjectSetup()
    {
        print("object registered " + InternalName);
        EditorController.Instance.RegisterDecorObject(this);
       
    }




    public virtual SavedObject CompileObjectData()
    {
       SavedObject Data = new SavedObject(InternalName,SwatchID, new ObjectSaveData(ObjectSaveDataType.none), new SavedTransform(this.transform.position, this.transform.rotation.eulerAngles, this.transform.localScale));


        return Data;
    }


    public virtual void DestroyObject()
    {
       // ObjectPlacementWidgit.PlacementWidgit.ActivateWidgit(false);
       
        SwatchUI.Instance.CloseMenu();
        EditorController.Instance.DeRegisterDecorObject(this);

    }



    public void OnMouseEnter()
    {
        Editor_Mouse.highlightedObject = this;
        this.gameObject.layer = 16;
        print("highlighted");
    }

    public void OnMouseExit()
    {
        if(Editor_Mouse.highlightedObject == this)
        {
            this.gameObject.layer = 12;
            Editor_Mouse.highlightedObject = null;
        }
    }




}

public enum ObjectSaveDataType {none,Light,Waypoint,Animatronic, ClassicStart, ButtonSwitch }


[System.Serializable]
public class ObjectSaveData
{
    public ObjectSaveDataType DataType = ObjectSaveDataType.none;

    public ObjectSaveData(ObjectSaveDataType dataType)
    {
        this.DataType = dataType;
    }

}


[System.Serializable]
public class LightSaveData : ObjectSaveData
{
    public LightData lightData;


    public LightSaveData(ObjectSaveDataType dataType, LightData lightData): base(dataType){
        this.lightData = lightData;
        this.DataType = dataType;
        }
}

/// <summary>
/// class containing animatronic stats and waypoint data
/// </summary>
[System.Serializable]
public class AnimatronicSaveData : ObjectSaveData
{
    public AnimatronicData animatronicData;


    public AnimatronicSaveData(ObjectSaveDataType dataType, AnimatronicData animatronicData) : base(dataType)
    {
        this.animatronicData = animatronicData;
        this.DataType = dataType;
    }
}


/// <summary>
/// class for waypoint obects in the map, not animatronic waypoints
/// </summary>
[System.Serializable]
public class WaypointSaveData : ObjectSaveData
{
    public string Name;
    public int targetItemID;
    public int waypointID;
    public SavableObjectActionSet OnOccupied;
    public SavableObjectActionSet OnUnccupied;

    public WaypointSaveData(ObjectSaveDataType dataType, string Name,int targetItemID, ObjectActionSet OccupiedEvent, ObjectActionSet UnoccupiedEvent):base (dataType)
    {
        this.Name = Name;
        this.DataType = dataType;
        this.targetItemID = targetItemID;
        this.OnOccupied = new SavableObjectActionSet(OccupiedEvent);
        this.OnUnccupied = new SavableObjectActionSet(UnoccupiedEvent);
    }
}