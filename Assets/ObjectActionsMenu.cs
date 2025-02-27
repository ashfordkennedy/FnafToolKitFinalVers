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
    [SerializeField] GameObject TaskButton;
    private int targetActionIndex = -1;
    private void Awake()
    {
        instance = this;
        TargetGlobalActions();
    }


    public void UpdateUi()
    {


    }

    public override void OpenMenu()
    {
        TargetGlobalActions();
        base.OpenMenu();
    }


    public void SetActionSelectTarget(DecorObject NewTarget)
    {
        ActionSelectionTarget = NewTarget;
        RefreshActionSelectList(ActionSelectionTarget.GetObjectActions());
        ToggleActionSelectMenu(true);

        SelectionTargetDisplay.text = decorCatalogue.GetDecorObjectName(ActionSelectionTarget.InternalName);
    }

    public void SetTargetActionSet(ObjectActionSet NewTarget)
    {
        targetActionSet = NewTarget;
        RefreshTargetActionList();
    }

    

    public void ToggleActionSelectMenu(bool Open)
    {
        ActionSelectMenu.SetActive(Open);
    }

    /// <summary>
    /// refreshes the selectable action list to reflect the current target object
    /// </summary>
    public void RefreshActionSelectList(List<ObjectActionIndex> actions)
    {
        if(ActionSelectionTarget != null)
        {
            int tagCount = actions.Count -1;
            for (int i = 0; i < ListingButtons.Count; i++)
            {
                if(i<= tagCount)
                {                
                    ListingButtons[i].gameObject.SetActive(true);
                    ListingButtons[i].SetListing(actions[i]);
                }
                else
                {
                    ListingButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }


    /// adds an action to the display. called by the object Action listing UI element
    internal void AddAction(ObjectActionType actiontype, string actionTag, string actionText)
    {
        switch (actiontype)
        {
            case ObjectActionType.SetFloat:
                targetActionSet.objectActions.Add(new SetFloatAction(actionTag,SetFloatActionType.equal,0,ActionSelectionTarget, actionText));
                break;

            case ObjectActionType.SetBool:
                targetActionSet.objectActions.Add(new SetBoolAction(actionTag, SetBoolActionType.Off, ActionSelectionTarget, actionText));
                break;
        }

        RefreshTargetActionList();
    }


    internal void EditAction(Transform action)
    {
        int id = action.GetSiblingIndex();


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
                template.GetComponent<BoolActionUI>().RestoreActionUi(newAction);
                template.SetActive(true);
                print("instantiating bool action UI");
                break;

            case ObjectActionType.SetFloat:
                 template = Instantiate(FloatTemplate, ActionListContainer);
                template.GetComponent<FloatActionUI>().RestoreActionUi(newAction);
                template.SetActive(true);
                print("instantiating float action UI");
                break;

        }
    }

    /// <summary>
    /// Recreate the Action task list to reflect the target action set
    /// </summary>
    internal void RefreshTargetActionList()
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

    public void RemoveAction(Transform actionUiPanel)
    {
        int id = actionUiPanel.GetSiblingIndex();
        targetActionSet.objectActions.RemoveAt(id);
        RefreshTargetActionList();
    }

    public void CenterToTarget(BaseObjectActionUI target)
    {
        EditorController.Instance.CenterToRoom(target.targetObject.transform);
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


    public void TargetGlobalActions()
    {
        RefreshActionSelectList(EditorController.Instance.ObjectActions);
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











