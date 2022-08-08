using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectActionEvents;
using TMPro;
public class ObjectActionListing : MonoBehaviour
{
    [SerializeField] TMP_Text _actionText;
    [SerializeField] private string _ActionTag = "";
    [SerializeField] private ObjectActionType _actiontype = ObjectActionType.none;
    public void SetListing(ObjectActionIndex ActionIndex)
    {
        _actionText.text = ActionIndex.ActionName;
        _actiontype = ActionIndex.actionType;
        _ActionTag = ActionIndex.ActionTag;
    }

    public void AddAction()
    {
        ObjectActionsMenu.instance.AddAction(_actiontype, _ActionTag, _actionText.text);
    }

    
   
}
