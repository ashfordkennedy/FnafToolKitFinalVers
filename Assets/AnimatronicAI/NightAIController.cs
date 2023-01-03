using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightAIController : MonoBehaviour
{
    public List<EditorAnimatronic> Animatronics;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && Input.GetKey(KeyCode.LeftAlt))
        {
            DisableAI();
            StartAnimatronicAi(0);
        }
    }

    public void StartAnimatronicAi(int NightId)
    {
       // Animatronics[0].NightSetup(NightId);
        StartCoroutine(AnimatronicBrain(Animatronics[0]));


    }

    public void DisableAI()
    {
        StopAllCoroutines();
    }

    public IEnumerator AnimatronicBrain(EditorAnimatronic target)
    {
        while(NightManager.instance.totalPower > 0)
        {


           yield return new WaitForSeconds(target.Aggression);

            //check AI level
            if(AiLevelCheck(target.AiLevel) == true)
            {
                // stash target waypoint ref
                var waypoint = target.GetNextWaypoint();

                //next waypoint exists
                if(waypoint != null)
                {

                    //Condition is met and waypoint clear
                   if(waypoint.condition.CheckCondition() == true && waypoint.waypoint.Occupied == false)
                    {
                        AdvanceAnimatronic(waypoint, target);
                        print("waypoint condition met");
                    }

                   //Failed check (AI setting dependent)
                    else
                    {
                        print("waypoint condition failed");
                        FailedConditionTest(target);
                    }
                }

                else
                {
                    print("You just got jumpscared");
                    //Jumpscare
                }
            }
            yield return null;
        }

        yield return null;
    }


    private bool AiLevelCheck(int AiLevel)
    {
       var value = Random.Range(0, 20);
    
      return (value >= AiLevel) ?  true :  false ;
    }


    private void AdvanceAnimatronic(TargetWaypointData waypoint, EditorAnimatronic target)
    {
        target.SetAnimatronicWaypoint(waypoint);
        target.nextWaypointId++;
    }
  


    private void FailedConditionTest(EditorAnimatronic target)
    {
        target.SetAnimatronicWaypoint(0);
        
    }
}
