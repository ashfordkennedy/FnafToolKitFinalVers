using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public enum AIMode {Basic, Stalker, Runner,Attacker, Haunter}
public enum AIUpdateMode { None, Hourly, RandomHour}
public enum MovementMode {Warp, Walk }

[RequireComponent(typeof(NavMeshAgent))]
public class Animatronic : MonoBehaviour
{


    public Animator animator;
    private int AILevel = 0;
    private int AIMaxLevel = 20;

    [Header("Waypoints")]
    public List<AnimatronicWaypoint> Waypoints;
    public int CurrentWaypoint = 0;
    public int TargetWaypoint;
    private NavMeshAgent NavAgent;


    [Header("Night Settings")]
    public int[] AIStartLevels;
    public int[] AIIncrements;
    public int[] MaxLevels;
    public AIMode AiMode = AIMode.Basic;

    public void Awake()
    {
        NavAgent = GetComponent<NavMeshAgent>();
    }


    public void OnMouseDown()
    {
        if (EditorController.Instance.gameObject.activeInHierarchy == false)
        {


        }
    }



    public void EditorSelect()
    {
        ObjectTransformController.instance.gameObject.SetActive(true);
       // ObjectTransformController.ObjectTransformGizmo.TargetTransformObject = this;
        ObjectTransformController.instance.StartCoroutine("DisplayTransformUI", true);


    }

    public void EditorDeselect()
    {
        ObjectTransformController.instance.StartCoroutine("DisplayTransformUI", false);


    }




    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.N))
        {
            TargetWaypoint += 1;
            if (TargetWaypoint > (Waypoints.Count - 1))
            {
                TargetWaypoint = 0;

            }
            StopCoroutine("NextWaypoint");
            StartCoroutine("NextWaypoint");

        }
    }

    public void UpdateAILevel()
    {



    }

    public void CompletedWaypoint()
    {
        switch (AiMode)
        {
            case AIMode.Basic:

                break;

            case AIMode.Stalker:


                break;
            case AIMode.Runner:


                break;
            case AIMode.Attacker:


                break;
            case AIMode.Haunter:


                break;

        }
    }


    public IEnumerator NextWaypoint()
    {

        NavAgent.SetDestination(Waypoints[TargetWaypoint].transform.position);
        print(NavAgent.path.corners.ToString());
        NavAgent.isStopped = false;
        animator.SetFloat("speed", 1);
        yield return new WaitUntil(() => NavAgent.remainingDistance <= NavAgent.stoppingDistance);
        animator.SetFloat("speed", 0);
        NavAgent.isStopped = true;
        yield return null;
    }


    public IEnumerator RouteToWaypoint()
    {
        /*
        int Points = Waypoints[TargetWaypoint].RoutingPoints.Count;
        int CurrentPoint = 0;
        while (CurrentPoint < Points + 1)
        {
            NavAgent.destination = Waypoints[TargetWaypoint].RoutingPoints[CurrentPoint].transform.position;

            yield return new WaitUntil(() => NavAgent.remainingDistance == 0.05f);
            CurrentPoint += 1;

        }
        */
        yield return null;
    }
}



    /// <summary>
    /// Contains information based on 
    /// </summary>
    [Serializable]
public class WaypointInfo
{
    public int d;


}


    [Serializable]
    public class Pos
    {
        public float X = 0f;
        public float Y = 0f;
        public float Z = 0f;

        public Pos(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;

        }
    }