using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ChaseAnimatronic : MonoBehaviour
{

    [Header("Waypoints")]
    public List<AnimatronicWaypoint> Waypoints;
    public int CurrentWaypoint = 0;
    public int TargetWaypoint;
    private NavMeshAgent NavAgent;

    public void Awake()
    {
        NavAgent = GetComponent<NavMeshAgent>();
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
