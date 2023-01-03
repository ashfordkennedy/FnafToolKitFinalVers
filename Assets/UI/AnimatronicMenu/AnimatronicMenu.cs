using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimatronicMenu : EditorMenuAbstract
{
    public static AnimatronicMenu instance;

    public EditorAnimatronic targetAnimatronic;

    public Transform WaypointContainer;
    public GameObject WaypointButtonPrefab;
    public GameObject WaypointObject;
   // public List<AnimatronicWaypoint> activeWaypoints;

    public GameObject WaypointMenu;
    public GameObject ConditionMenu;

    public TargetWaypointData targetWaypoint;

    [SerializeField] WaypointConditionUi conditionUI;

    [SerializeField] Text NightDisplay;
    private int _selectedNight = 0;

    [SerializeField] Gradient SliderHues;
    [SerializeField] Slider AISlider;

    private int aILevel;

    [SerializeField] Slider Aggression;

    [SerializeField] Slider UnusedSlider;

    public int AILevel { get => aILevel;

        set { aILevel = value;
            targetAnimatronic.AiLevelData[_selectedNight] = value;
        }    
    }

    public void Awake()
    {
        instance = this;
    }


    public void SetTargetNight(int SwitchTotal)
    {
        _selectedNight += SwitchTotal;

        if (_selectedNight > 6)
        {
            _selectedNight = 0;
        }
        if (_selectedNight < 0)
        {
            _selectedNight = 6;
        }


        NightDisplay.text = "Night " + (_selectedNight + 1);
        AISlider.value = (targetAnimatronic.AiLevelData[_selectedNight]);
       // UpdateDisplay();
    }

    public void UpdateAiSlider(Slider slider)
    {
        AILevel = (int)slider.value;
    }

    public void UpdateAggressionSlider(Slider slider)
    {

    }

    public void ToggleWaypointLineRenderer()
    {
        
        targetAnimatronic.ToggleLineRenderer();
    }

    public void UpdateAIDisplay()
    {


    }


    public override void OpenMenu()
    {
        ClearWaypointMenu();
        SetTargetNight(0);
        base.OpenMenu();
        WaypointMenu.SetActive(true);
        ConditionMenu.SetActive(false);
        InitializeWaypointMenu();
    }

    public  void OpenMenu(EditorAnimatronic editorAnimatronic)
    {
        ClearWaypointMenu();
        targetAnimatronic = editorAnimatronic;    
        WaypointMenu.SetActive(true);
        base.OpenMenu();
        InitializeWaypointMenu();
        ConditionMenu.SetActive(false);
        
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    public void OpenConditionList(Transform target)
    {
        var id = target.GetSiblingIndex();
        targetWaypoint = targetAnimatronic.waypoints[id];
        conditionUI.targetWaypointData = targetWaypoint;
        //conditionUI.UpdateUi();     
        conditionUI.UpdateConditionDisplay();
        WaypointMenu.SetActive(false);
        ConditionMenu.SetActive(true);
    }


    public override void CloseMenu()
    {
        ClearWaypointMenu();
        base.CloseMenu();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleWaypointSelect()
    {
        if (EditorController.Instance.Editor_MouseMode == EditorMouseMode.waypointSelect)
        {
            RoomEditorMouse.Instance.ChangeMouseMode(3);
        }

        else
        {
            RoomEditorMouse.Instance.ChangeMouseMode(11);
        }

    }


    public void AddWaypoint(AnimatronicWaypoint waypoint)
    {
        print("waypoint recieved, now adding to target animatronic waypoint list");
        targetAnimatronic.waypoints.Add(new TargetWaypointData(waypoint, waypoint.text.text));
        print("waypoint added without incident");
        InitializeWaypointMenu();
        print("waypoint recieved, now adding to target animatronic waypoint list");
        RoomEditorMouse.Instance.ChangeMouseMode(3);
    }


    public void RenameWaypointData(InputField inputField)
    {
        var id = inputField.transform.parent.GetSiblingIndex();
        targetAnimatronic.waypoints[id].waypointName = inputField.text;
        print( "Rename waypoint Data has been called");
    }

    public void InitializeWaypointMenu()
    {
        ClearWaypointMenu();

        print(targetAnimatronic.waypoints.Count + " is the count of added waypoints");
            for (int i = 0; i < targetAnimatronic.waypoints.Count; i++)
            {
            print(targetAnimatronic.waypoints.Count + " is the current count, the index is at " + i);
            var button = Instantiate(WaypointButtonPrefab, WaypointContainer);
            print("Instantiation successful");
            button.SetActive(true);
            print("set active successful");
            string name = targetAnimatronic.waypoints[i].waypointName;
            print("name retreival successful");
            print("Waypoint name  = " + name);
             button.transform.GetChild(0).GetComponent<InputField>().SetTextWithoutNotify(name);
            //print("Set name successful");


            //var waypoint = newbut.GetComponent<AnimatronicWaypoint>();
            // activeWaypoints.Add(waypoint);
            // waypoint.InitialiseWaypoint(i);
        }
        



    }

    public void ReorderWaypointMenu()
    {


    }

    /// <summary>
    /// clears up the waypoints listed on the UI, not the data itself
    /// </summary>
    public void ClearWaypointMenu()
    {
        foreach (Transform child in WaypointContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }


    public void LowerWaypointPriority(Transform target)
    {
        var id = target.GetSiblingIndex();
        if (id != targetAnimatronic.waypoints.Count - 1)
        {
            TargetWaypointData tempindex = targetAnimatronic.waypoints[id + 1];
            targetAnimatronic.waypoints[id + 1] = targetAnimatronic.waypoints[id];
            targetAnimatronic.waypoints[id] = tempindex;
            InitializeWaypointMenu();
        }
    }

    public void RaiseWaypointPriority(Transform target)
    {
        var id = target.GetSiblingIndex();
        if (id != 0)
        {
            TargetWaypointData tempindex = targetAnimatronic.waypoints[id - 1];
            targetAnimatronic.waypoints[id - 1] = targetAnimatronic.waypoints[id];
            targetAnimatronic.waypoints[id] = tempindex;
            InitializeWaypointMenu();
        }
    }

    public void DeleteWaypoint(Transform target)
    {
        var id = target.GetSiblingIndex();
        targetAnimatronic.waypoints.RemoveAt(id);
        InitializeWaypointMenu();
    }


    public static int GetSiblingIndex(Transform transform)
    {
        return transform.GetSiblingIndex();
    }


    public void PlayWaypointSequence()
    {
        targetAnimatronic.TriggerWaypointTest();
    }
}


