using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaypointSettingsPanel : EditorMenuAbstract
{
    public static WaypointSettingsPanel Instance;
    [SerializeField] private InputField _nameField;
    public AnimatronicWaypoint target;

    public WaypointCondition targetWaypointCondition;
    private void OnEnable()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseMenu(AnimatronicWaypoint waypoint)
    {
        if(target == waypoint)
        {
            base.CloseMenu();
            target = null;
        }
    }


    public void OpenMenu(AnimatronicWaypoint newTarget)
    {
        base.OpenMenu();
        SetTarget(newTarget);

    }

    public void SetTarget(AnimatronicWaypoint newTarget)
    {
        target = newTarget;
        if (newTarget != null)
        {
            _nameField.text = newTarget.text.text;
        }
    }

    public void DeleteWaypoint()
    {
        target.DestroyObject();
    }

    public void RenameWaypoint(InputField inputField)
    {
        target.UpdateName(inputField.text);
    }
    
}
