using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ObjectActionEvents;
public class BaseObjectActionUI : MonoBehaviour
{
    [SerializeField] internal string ActionTag = "";
    [SerializeField] internal GameObject targetObject = null;
    [SerializeField] internal ObjectAction targetAction = null;
    public TMP_Text ActionNameDisplay;

    public void SetDisplayName(string NewName)
    {
        ActionNameDisplay.text = NewName;
    }


    public virtual void RestoreActionUi(ObjectAction newTarget)
    {
        targetAction = newTarget;
        

    }



    public virtual void UpdateActionClass()
    {

    }





    public void FocusOnTarget()
    {

    }

}

