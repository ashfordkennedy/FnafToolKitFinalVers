using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System;
using ObjectActionEvents;
using ObjectActionEvents.Savables;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class ObjectActionsMenu : EditorMenuAbstract
{
    public static ObjectActionsMenu instance;
    [SerializeField]private DecorObject ActionSelectionTarget = null;
    [SerializeField] public ObjectActionSet targetActionSet = new ObjectActionSet();


    [SerializeField] GameObject ActionSelectMenu;
    [SerializeField] GameObject FloatTemplate;
    [SerializeField] GameObject BoolTemplate;
    [SerializeField] GameObject SetLightTemplate;
    [SerializeField] List<ObjectActionListing> ListingButtons;

    [SerializeField] Transform ActionListContainer;

    [SerializeField] TMP_Text SelectionTargetDisplay;
    [SerializeField] DecorObjectCatalogue decorCatalogue;
    private void Awake()
    {
        instance = this;
    }


    public void UpdateUi()
    {


    }


    public void SetActionSelectTarget(DecorObject NewTarget)
    {
        ActionSelectionTarget = NewTarget;
        RefreshActionSelectList();
        ToggleActionSelectMenu(true);

        SelectionTargetDisplay.text = decorCatalogue.GetDecorObjectName(ActionSelectionTarget.InternalName);
    }

    public void SetTargetActionSet(ObjectActionSet NewTarget)
    {
        targetActionSet = NewTarget;
        RefreshActionList();
    }

    

    public void ToggleActionSelectMenu(bool Open)
    {
        ActionSelectMenu.SetActive(Open);
    }

    /// <summary>
    /// refreshes the selectable action list to reflect the current target object
    /// </summary>
    public void RefreshActionSelectList()
    {
        if(ActionSelectionTarget != null)
        {
            var actionTags = ActionSelectionTarget.GetObjectActions();

            int tagCount = actionTags.Count -1;
            for (int i = 0; i < ListingButtons.Count; i++)
            {
                if(i<= tagCount)
                {                
                    ListingButtons[i].gameObject.SetActive(true);
                    ListingButtons[i].SetListing(actionTags[i]);
                }
                else
                {
                    ListingButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }


    /// adds an action to the display
    internal void AddAction(ObjectActionType actiontype, string actionTag, string actionText)
    {
        switch (actiontype)
        {
            case ObjectActionType.SetFloat:
                targetActionSet.objectActions.Add(new SetFloatAction(actionTag,SetFloatActionType.equal,0,ActionSelectionTarget, actionText));
                break;
        }

        RefreshActionList();
    }

    /// <summary>
    /// adds an action to the display
    /// </summary>
    /// <param name="newAction"></param>
    internal void AddAction(ObjectAction newAction)
    {
        GameObject template = null;
        switch (newAction.ObjectActionType)
        {
            case ObjectActionType.SetBool:
                template = Instantiate(BoolTemplate, ActionListContainer);
                template.GetComponent<FloatActionUI>().RestoreActionUi(newAction);
                template.SetActive(true);
                break;

            case ObjectActionType.SetFloat:
                 template = Instantiate(FloatTemplate, ActionListContainer);
                template.GetComponent<FloatActionUI>().RestoreActionUi(newAction);
                template.SetActive(true);
                print("instantiating the desired ui now");
                break;

        }
    }


    internal void RefreshActionList()
    {
        foreach (Transform child in ActionListContainer.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < targetActionSet.objectActions.Count; i++)
        {
            var target = targetActionSet.objectActions[i];
           
            AddAction(targetActionSet.objectActions[i]);

        }

    }

   




    public void ToggleTargetPicker()
    {
        switch (RoomEditorMouse.Instance.mouseMode)
        {
            case EditorMouseMode.ActionSelect:
                RoomEditorMouse.Instance.ChangeMouseMode(3);
                break;

            default:
                RoomEditorMouse.Instance.ChangeMouseMode(13);
                break;


        }
    }

}

[Serializable]
public class ObjectActionSet
{
    public string ActionSetName;
    [SerializeReference]public List<ObjectAction> objectActions;

    public ObjectActionSet(string name = "Action")
    {
        this.ActionSetName = name;
        this.objectActions = new List<ObjectAction>();
    }

    public ObjectActionSet(SavableObjectActionSet savedActionSet)
    {
        this.ActionSetName = savedActionSet.ActionSetName;
        this.objectActions = GenerateActions(savedActionSet);
    }



    public void GenerateUnityEvent(UnityEvent TargetEvent)
    {
        for (int i = 0; i < objectActions.Count; i++)
        {
            objectActions[i].GenerateEventMethod(TargetEvent);
        }

    }

    /// <summary>
    /// converts savableactions back into editor ready references
    /// </summary>
    /// <returns></returns>
    public List<ObjectAction> GenerateActions(SavableObjectActionSet savedActionSet)
    {
        List<ObjectAction> actions = new List<ObjectAction>();

        for (int i = 0; i < savedActionSet.objectActions.Count; i++)
        {
            var action = savedActionSet.objectActions[i];

            switch (action.ActionType)
            {
                case ObjectActionType.none:
                    Debug.LogWarning("An ObjectAction that has attempted to restore from save has been tagged as 'Non', Please ensure the class is properly flagged with the correct identifying type");
                    break;

                case ObjectActionType.SetFloat:
                    var floatAction = action as SavableFloatAction;
                   actions.Add(new SetFloatAction(floatAction));
                    break;

                case ObjectActionType.SetBool:
                    var boolAction = action as SavableBoolAction;
                    actions.Add(new SetBoolAction(boolAction));
                    break;
            }
        }
        Debug.Log("Restoring Actions to Object");
        return actions;
    }


    /// <summary>
    /// Converts and generates the given action as a list of savable actions ready for serialization
    /// </summary>
    /// <returns></returns>
    public List<SavableObjectAction> GenerateActionSavables()
    {
        List<SavableObjectAction> actions = new List<SavableObjectAction>();

        for (int i = 0; i < objectActions.Count; i++)
        {
            var action = objectActions[i];
           switch (action.ObjectActionType)
            {
                case ObjectActionType.none:
                    break;


                case ObjectActionType.SetFloat:
                    var floatAction = action as SetFloatAction;
                   actions.Add(new SavableFloatAction(floatAction));
                    break;

                case ObjectActionType.SetBool:
                    var boolAction = action as SetBoolAction;
                    actions.Add(new SavableBoolAction(boolAction));
                    break;

            }
        }
        return actions;
    }
}

[Serializable]
public class SavableObjectActionSet{

    public string ActionSetName;
    public List<SavableObjectAction> objectActions = new List<SavableObjectAction>();

    public SavableObjectActionSet(ObjectActionSet TargetSet)
    {
        this.objectActions.AddRange(TargetSet.GenerateActionSavables());
        Debug.Log("saved action set with " + this.objectActions.Count +" Actions");
        this.ActionSetName = TargetSet.ActionSetName;

    }


    }











