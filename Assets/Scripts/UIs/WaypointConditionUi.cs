using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum targetObjectType {Default,Door,Light, Camera};
public class WaypointConditionUi : MonoBehaviour
{
    [SerializeField] AnimatronicAnimations animations;
    public TargetWaypointData targetWaypointData;
    private DecorObject targetObject = null;
    [SerializeField] Dropdown _conditiontype;
    [SerializeField] Text objectName;
    [SerializeField] Slider poseSlider;
    [SerializeField] TMP_Dropdown poseDropdown;
    private List<Dropdown.OptionData> defaultConditions = new List<Dropdown.OptionData>
    { new Dropdown.OptionData("none"),
    new Dropdown.OptionData("< Power"),
    new Dropdown.OptionData("> Power")};

    [SerializeField] Toggle positionToggle;
    [SerializeField] Toggle poseToggle;

    #region conditionReferences
    [SerializeField] Transform ConditionParent;
    [Header("Power less than")]
    [SerializeField] GameObject PowerLess;
    [SerializeField] InputField powerLessInput;

    [Header("Power more than")]
    [SerializeField] GameObject PowerMore;
    [SerializeField] InputField powerMoreInput;
    #endregion


    private void OnAwake()
    {
        RefreshConditionField(defaultConditions);
        PopulatePoseDropdown();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TargetSelectMode()
    {
        RoomEditorMouse.Instance.tempTarget = this.gameObject;
        RoomEditorMouse.Instance.ChangeMouseMode(12);

    }

    public void PopulatePoseDropdown()
    {
        poseDropdown.ClearOptions();
        int endotype = AnimatronicMenu.instance.targetAnimatronic.endoType;
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < animations.AnimationSets[endotype].animations.Count; i++)
        {
           string name = animations.AnimationSets[endotype].animations[i].animationName;
            options.Add(new TMP_Dropdown.OptionData(name));
        }

        poseDropdown.options = options;
    }





    /// <summary>
    /// Reset menu to display target values
    /// </summary>
    public void UpdateConditionDisplay()
    {
        print("updating conditions");
        WaypointCondition condition = targetWaypointData.condition;
        ResetConditionWindows();
       
        //poses
        int endotype = AnimatronicMenu.instance.targetAnimatronic.endoType;
        poseDropdown.SetValueWithoutNotify(targetWaypointData.condition.AnimationId);

        //  poseSlider.maxValue = animations.AnimationSets[endotype].animations.Count -1;
        //  poseSlider.SetValueWithoutNotify(targetWaypointData.condition.AnimationId);

         PopulatePoseDropdown();
        //conditions
        if (condition.target != null)
        {
            targetObject = condition.target;
            objectName.text = targetObject.name;
            RefreshConditionField(targetObject.GetWaypointConditionOptions());

            int conditionid = 0;


          //  _conditiontype.SetValueWithoutNotify((int)condition.condition);
        }
        else
        {
            objectName.text = "No Target";
            // no target, Hide them settings
            RefreshConditionField(defaultConditions);
            
        }
           

    }

    public void UpdateUi()
    {

    }

    public void ResetConditionWindows()
    {
        foreach (Transform child in ConditionParent)
        {
            child.gameObject.SetActive(false);
        }
        ConditionParent.GetChild(0).gameObject.SetActive(true);
    }

    public void SetConditionType(Dropdown dropdown)
    {
        ResetConditionWindows();
        int id = dropdown.value;
        string condition = dropdown.options[id].text;
        if (targetObject != null)
        {
            targetWaypointData.condition.condition = targetObject.GetConditionEnum(condition);
        }

        else
        {
            waypointConditionSetting value = waypointConditionSetting.none;
            // condition setting for non target specific conditions
            switch (condition)
            {
                case "None":
                    value = waypointConditionSetting.none;
                    break;

                case "< Power":
                    value = waypointConditionSetting.PowerLessThan;
                    PowerLess.SetActive(true);
                    break;

                case "> Power":
                    value = waypointConditionSetting.PowerMoreThan;
                    PowerMore.SetActive(true);
                    break;
            }

            targetWaypointData.condition.condition = value;
        }




        targetWaypointData.condition.DropdownReturnId = id;
            // (waypointConditionSetting)System.Enum.Parse(typeof(waypointConditionSetting), condition);
        print("new condition is" + targetWaypointData.condition.condition);


    }

    /// <summary>
    /// Resets the input fields for the conditions
    /// </summary>
    /// <param name="condition"></param>
    public void FirstTimeSelect(waypointConditionSetting condition)
    {
        switch (condition)
        {
            case waypointConditionSetting.PowerLessThan:
                PowerLess.SetActive(true);
                powerLessInput.text = "50";
                powerLessInput.onEndEdit.Invoke("50");
                break;

            case waypointConditionSetting.PowerMoreThan:
                PowerMore.SetActive(true);
                powerMoreInput.text = "50";
                powerMoreInput.onEndEdit.Invoke("50");
                break;


        }
    }

    public void SetObjectTarget(DecorObject newTarget) {
        print("object target recieved back at the condition");
        targetWaypointData.condition.target = newTarget;
        UpdateConditionDisplay();
    }


    /// <summary>
    /// called when menu is opened, puts your values back on the menu
    /// </summary>
    /// <param name="newOptions"></param>
    public void RefreshConditionField(List<Dropdown.OptionData> newOptions)
    {
        


        print("restoring old waypoint condition");
        _conditiontype.ClearOptions();
        _conditiontype.AddOptions(newOptions);
        _conditiontype.SetValueWithoutNotify(targetWaypointData.condition.DropdownReturnId);
        var condition = targetWaypointData.condition;

        /*
        if(condition.target != null)
        {
            SetObjectTarget(condition.target);

        }
        else
        {

        }
        */

        switch (targetWaypointData.condition.condition)
        {
            
            case waypointConditionSetting.PowerLessThan:
                PowerLess.SetActive(true);
                
                powerLessInput.text = "" + (condition.conditionValues as WaypointConditionValues_Float).value;
                break;

            case waypointConditionSetting.PowerMoreThan:
                PowerMore.SetActive(true);
                powerMoreInput.text = "" + (condition.conditionValues as WaypointConditionValues_Float).value;
                break;

            case waypointConditionSetting.DoorClosed:

                break;

            case waypointConditionSetting.DoorOpen:

                break;

        }




    }

    public void SetConditionValues()
    {
        int id = this.gameObject.transform.GetSiblingIndex();
    }

    public void TogglePosePreview(Toggle toggle)
    {
        int Id = poseDropdown.transform.GetSiblingIndex();
        AnimatronicMenu.instance.targetAnimatronic.PreviewPoseToggle(toggle.isOn, Id);
    }

    public void TogglePositionPreview(Toggle toggle)
    {
        int id = AnimatronicMenu.instance.targetAnimatronic.waypoints.IndexOf(targetWaypointData);
        AnimatronicMenu.instance.targetAnimatronic.PreviewPositionToggle(toggle.isOn, id);
    }

    public void DisablePreviews()
    {
        positionToggle.SetIsOnWithoutNotify(false);
        AnimatronicMenu.instance.targetAnimatronic.PreviewPositionToggle(false, 0);

   
    }


    public void SetWaypointPose(Slider slider)
    {
        targetWaypointData.condition.AnimationId = (int)slider.value;
    }

    public void SetWaypointPose(TMP_Dropdown dropDown)
    {
        targetWaypointData.condition.AnimationId = (int)dropDown.value;
    }

    /// <summary>
    /// methods that control the various stored values for conditions
    /// </summary>
    /// <param name="input"></param>
#region ConditionSetMethods

    public void SetPowerLessCondition(InputField input)
    {
        float result;
        if (float.TryParse(input.text, out result))
        {           
        }
        else
        {
            result = 50f;
            input.text = "50";
        }

        targetWaypointData.condition.conditionValues = new WaypointConditionValues_Float(result);
    }


    public void SetPowerMoreCondition(InputField input)
    {
        float result;
        if (float.TryParse(input.text, out result))
        {
        }
        else
        {
            result = 50f;
            input.text = "50";
        }

        targetWaypointData.condition.conditionValues = new WaypointConditionValues_Float(result);
    }

    #endregion







}

public enum ConditionType {None, Float, Bool }

[System.Serializable]
public class WaypointConditionValues_base
{
   public ConditionType type = ConditionType.None;
   // public T value;
}

[System.Serializable]
public class WaypointConditionValues_Float : WaypointConditionValues_base
{
    
    public float value = 0f;

    public WaypointConditionValues_Float(float value)
    {
        this.value = value;
        this.type = ConditionType.Float;
    }
}

[System.Serializable]
public class WaypointConditionValues_Bool : WaypointConditionValues_base
{

     public bool value = false;

    public WaypointConditionValues_Bool(bool value)
    {
        this.value = value;
        this.type = ConditionType.Bool;
    }
}





