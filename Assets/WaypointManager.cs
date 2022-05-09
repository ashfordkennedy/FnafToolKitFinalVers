using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager instance;
    public List<AnimatronicWaypoint> waypoints;
    public Dictionary<AnimatronicWaypoint, int> waypointDictionary = new Dictionary<AnimatronicWaypoint, int>();

    private void Awake()
    {
        instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// adds the waypoint to the registry
    /// </summary>
    /// <param name="waypoint"></param>
    public void RegisterWaypoint(AnimatronicWaypoint waypoint)
    {
        if (waypoints.Contains(waypoint) == false)
        {
            int newIndex = waypoints.Count + 1;
            waypoints.Add(waypoint);
            waypoint.name = "Waypoint " + newIndex;
            RefreshDictionary();
        }
        else
        {
            print("Waypoint Exists. Ignoring register request");
        }
    }


    /// <summary>
    /// Delete the waypoints data and refresh the dictionary
    /// </summary>
    /// <param name="waypoint"></param>
    public void DeregisterWaypoint(AnimatronicWaypoint waypoint)
    {
        int index = -1;
        if(waypointDictionary.TryGetValue(waypoint,out index) == true)
        {
            waypoints.RemoveAt(index);
        }     
        RefreshDictionary();
    }


    // generate IDs for waypoints
    public void RefreshDictionary()
    {
        waypointDictionary.Clear();
        for (int i = 0; i < waypoints.Count; i++)
        {
            waypointDictionary.Add(waypoints[i], i);
        }

    }
}



[System.Serializable]
public class WaypointData
{
    public AnimatronicWaypoint reference;
    public string name = "Waypoint";

    public WaypointData(string name, AnimatronicWaypoint reference)
    {
        this.name = name;
        this.reference = reference;
    }
    


}