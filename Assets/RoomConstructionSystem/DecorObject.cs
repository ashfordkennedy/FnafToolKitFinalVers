using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum DecorObjectType { Basic, Door, Light, Animatronic, Waypoint, Camera };

/// <summary>
/// Decor object is a base class for all objects
/// </summary>

public class DecorObject : MonoBehaviour
{
    public DecorObjectType decorType = DecorObjectType.Basic;
    public string InternalName;
    public int SwatchID;
    public static List<Dropdown.OptionData> conditions = new List<Dropdown.OptionData>
    {   new Dropdown.OptionData("None"),
    };

    public virtual List<Dropdown.OptionData> GetConditionOptions()
    {
        return conditions;
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
        // ObjectTransformController.ObjectTransformGizmo.gameObject.SetActive(true);
        // ObjectTransformController.ObjectTransformGizmo.TargetTransformObject = this.gameObject;

        /* enable new object placer here
        if(ObjectTransformController.ObjectTransformGizmo.TargetTransformObject == this.gameObject)
        {
            ObjectTransformController.ObjectTransformGizmo.OpenTransformController(false, null);
        }
        else
        {
            SwatchUI.Instance.OpenSwatchPanel(InternalName, this);
            ObjectTransformController.ObjectTransformGizmo.OpenTransformController(true, this);

        }

        */


       // ObjectTransformController.ObjectTransformGizmo.StartCoroutine("DisplayTransformUI",true);
       // ObjectSetup();



        //  this.transform.position = GridSnap.SnapPosition(this.transform.position, 1);
    }

    public virtual void EditorDeselect()
    {
      //  ObjectTransformController.ObjectTransformGizmo.OpenTransformController(false, null);
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
        print("objectsetup " + InternalName);
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
}

public enum ObjectSaveDataType {none,Light,Waypoint,Animatronic }


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
    

    public WaypointSaveData(ObjectSaveDataType dataType, string Name,int targetItemID):base (dataType)
    {
        this.Name = Name;
        this.DataType = dataType;
        this.targetItemID = targetItemID;
    }
}