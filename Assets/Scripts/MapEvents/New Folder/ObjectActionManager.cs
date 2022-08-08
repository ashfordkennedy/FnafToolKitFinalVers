using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ObjectActionEvents;
[System.Serializable]
public class ObjectActionManager
{

    public List<ObjectActionIndex> objectActionSettings = new List<ObjectActionIndex>();


    public static void ProcessObjectAction(UnityEvent targetEvent, ObjectAction targetAction)
    {

        switch (targetAction.ActionTag)
        {
            default:
                return;

            case "":

                break;
        }
    }






}

[System.Serializable]
public class ObjectActionIndex{
    public string ActionTag;
    public string ActionName;
    public ObjectActionType actionType;

    public ObjectActionIndex(string ActionTag, string ActionName, ObjectActionType actionType)
    {
        this.ActionTag = ActionTag;
        this.ActionName = ActionName;
        this.actionType = actionType;
    }
    }