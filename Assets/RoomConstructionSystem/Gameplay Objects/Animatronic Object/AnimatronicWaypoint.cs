using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AnimatronicWaypoint : DecorObject
{
    public bool Occupied = false;
    public TextMesh text = null;

    public DecorObject TestReference;

    public ObjectActionSet OnOccupied = new ObjectActionSet("OnOccupied");
    public ObjectActionSet OnUnoccupied = new ObjectActionSet("OnUnoccupied");

    public UnityEvent OnOccupiedEvent = new UnityEvent();
    public UnityEvent OnUnocupiedEvent = new UnityEvent();

    public override void EditorSelect()
    {
        base.EditorSelect();
        WaypointSettingsPanel.Instance.SetTarget(this);
        WaypointSettingsPanel.Instance.OpenMenu();

    }

    public override void EditorDeselect()
    {
        base.EditorDeselect();
        
        WaypointSettingsPanel.Instance.CloseMenu();
        WaypointSettingsPanel.Instance.SetTarget(null);

    }


    public override void DestroyObject()
    {

        base.DestroyObject();
        WaypointManager.instance.DeregisterWaypoint(this);
    }

    public void UpdateName(string newName)
    {
        text.text = newName;
        
    }

    public override void ObjectSetup()
    {
        base.ObjectSetup();
        WaypointManager.instance.RegisterWaypoint(this);
        print("waypoint registered");
    }


    public override SavedObject CompileObjectData()
    {
        var id = EditorController.Instance.MapDecor.IndexOf(TestReference);
        SavedObject savedObject = new SavedObject(InternalName, SwatchID, new WaypointSaveData(ObjectSaveDataType.Waypoint,text.text,id,OnOccupied,OnUnoccupied),new SavedTransform(this.transform.position, this.transform.rotation.eulerAngles, this.transform.localScale));
        print(this.gameObject.transform.rotation);
        print("savedWAYPOINT");
        return savedObject;
    }

    public void RestoreWaypointData(WaypointSaveData waypointData)
    {
        print("waypointDataRestoring");        
       // WaypointManager.instance.RegisterWaypoint(this);
        UpdateName(waypointData.Name);
        print("waypoint target" + waypointData.targetItemID);
        if (waypointData.targetItemID > 0)
        {
            this.TestReference = EditorController.Instance.MapDecor[waypointData.targetItemID];
        }
        OnOccupied = new ObjectActionSet(waypointData.OnOccupied);
        OnUnoccupied = new ObjectActionSet(waypointData.OnUnccupied);
        print(waypointData.OnOccupied.ActionSetName);
        print(waypointData.OnOccupied.objectActions.Count);
        print("waypointDataComplete");
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void NightStartSetup()
    {
        OnOccupied.GenerateUnityEvent(OnOccupiedEvent);
        OnUnoccupied.GenerateUnityEvent(OnUnocupiedEvent);
        print("Night start up called on waypoint");
    }

    void DeactivateWaypoint()
    {
        this.gameObject.SetActive(false);
    }
}
