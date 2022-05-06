using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorAnimatronic : DecorObject
{
    [SerializeField] private LineRenderer _pointOverlay = null;
    public List<int> AiLevelData = new List<int>(7);
    public List<int> AggressionData = new List<int>(7);

    private bool _selected = false;
    public List<TargetWaypointData> waypoints;

    [SerializeField] private SkinnedMeshRenderer _endoRenderer;
    [SerializeField] private SkinnedMeshRenderer _skinRenderer;
    [SerializeField] public int endoType;

    [Header("nights data")]
    [SerializeField] internal int AiLevel = 0;
    [SerializeField] internal int Agression = 0;

    [SerializeField] internal int nextWaypointId = 0;
   


    [Header("Animation")]
    [SerializeField] AnimatronicAnimations animations;
    [SerializeField] public Animator animatior;
    public AnimatorOverrideController OverrideController;

    private Vector3 startPos = new Vector3();
    private Quaternion startRot = new Quaternion();
    public override void EditorSelect(Material SelectMaterial)
    {
        base.EditorSelect(SelectMaterial);
        if (AnimatronicMenu.instance.MenuOpen == false)
        {
            AnimatronicMenu.instance.OpenMenu(this);
        }
    }

    /// <summary>
    /// puts the animatronic into its start position and sets the relevant data.
    /// </summary>
    /// <param name="targetNight"></param>
    public void NightSetup(int targetNight)
    {
        AiLevel = AiLevelData[targetNight];
        AiLevel = AggressionData[targetNight];
        print(InternalName + " has been loaded with their night data");
    }

    public override void EditorDeselect()
    {
        base.EditorDeselect();
        //ObjectTransformController.ObjectTransformGizmo.OpenTransformController(false, null);
        SetLineRenderer(false);
        AnimatronicMenu.instance.CloseMenu();
    }


    public override SavedObject CompileObjectData()
    {

        var data = new AnimatronicData(ObjectSaveDataType.Animatronic, this);
        var animatronic = new SavedAnimatronic(InternalName, SwatchID, data, new SavedTransform(this.transform.position, this.transform.rotation.eulerAngles, this.transform.localScale));

        return animatronic;
    }

    /// <summary>
    /// Used by the editor controller when loading save games back into the editor
    /// </summary>
    /// <param name="data"></param>
    public void RestoreAnimatronicData(AnimatronicData data)
    {
        AiLevelData = data.AiLevel;
        AggressionData = data.Aggression;
        waypoints = data.ConvertStoredWaypoints();


    }

    public override void SwatchSwap(Mesh[] mesh, Material[] mats, int swatchid)
    {
        var endomats = new Material[2];
        endomats[0] = mats[0];
        endomats[1] = mats[1];

        _endoRenderer.materials = endomats;
        print("swapping animatronic swatch");
        // _endoRenderer.materials[0] = mats[0];
        // _endoRenderer.materials[1] = mats[1];

        SwatchID = swatchid;
        if (_skinRenderer != null)
        {
            _skinRenderer.material = endomats[1];
        }
        //  _endoRenderer.materials = 

    }

    public void PreviewPoseToggle(bool newState, int poseId)
    {
        AnimationClip clip;
        switch (newState)
        {
            case false:
                animatior.Play("Editor Idle", 0);
                break;

            case true:

                clip = animations.AnimationSets[endoType].animations[poseId].animation;

                OverrideController["V2EndoTestPose"] = clip;
                animatior.Play("PoseTest", 0);
                break;
        }
    }

    public void PreviewPositionToggle(bool newState, int waypointId)
    {
        switch (newState)
        {
            case false:
                this.transform.position = startPos;
                this.transform.rotation = startRot;
                break;

            case true:
                startPos = this.transform.position;
                startRot = this.transform.rotation;

                this.transform.position = waypoints[waypointId].waypoint.transform.position;
                this.transform.rotation = waypoints[waypointId].waypoint.transform.rotation;
                break;
        }
    }





    /*
    public void DestroyObject()
    {

    }
    */


    /*
    /// <summary>
    /// pass through the waypointData index of the now removed waypoint to remove from the list stored here.
    /// </summary>
    /// <param name="index"></param>
    public void RemovewaypointIndex(string waypointInternalName)
    {
        while (waypoints.IndexOf(waypointInternalName) != -1)
        {
            int index = waypoints.IndexOf(waypointInternalName);
           // if (index != -1)
          //  {
                waypoints.RemoveAt(index);

          //  }
        }
    }
    */

    /*
    /// <summary>
    /// checks waypoints stored in the animatronic and removes any indexes that are no longer stored in the waypointManager
    /// </summary>
    public void CheckWaypoints()
    {
        var waypointAmount = WaypointManager.instance.waypointData.Count;
        List<int> removables = new List<int>();
        for (int i = 0; i < waypoints.Count; i++)
        {
             if(waypoints[i] > waypointAmount)
            {
                removables.Add(i);
            }
         //   waypoint.transform.position = _waypoints[i];           
        }

        for (int i = 0; i < removables.Count; i++)
        {
            waypoints.RemoveAt(removables[i]);
            //   waypoint.transform.position = _waypoints[i];           
        }
    }
    */
    public void GenerateLineRendererData()
    {
        _pointOverlay.positionCount = waypoints.Count;
        for (int i = 0; i < _pointOverlay.positionCount; i++)
        {
            _pointOverlay.SetPosition(i, waypoints[i].waypoint.gameObject.transform.position);
        }
    }

    /// <summary>
    /// toggles visibility of line renderer for animatronic waypoints.
    /// </summary>
    /// <param name="active"></param>
    public void SetLineRenderer(bool active)
    {
        GenerateLineRendererData();
        _pointOverlay.enabled = active;
    }


    public void ToggleLineRenderer()
    {
        GenerateLineRendererData();
        _pointOverlay.enabled = !_pointOverlay.enabled;
    }


    public void TriggerWaypointTest()
    {
        StartCoroutine(PlayWaypointTest());
    }

    public IEnumerator PlayWaypointTest()
    {
        UIController.instance.HideCanvas(true);
        Vector3 startingPos = this.transform.position;
        Quaternion startingRot = this.transform.rotation;
        for (int i = 0; i < waypoints.Count; i++)
        {
            this.transform.position = waypoints[i].waypoint.transform.position;
            this.transform.rotation = waypoints[i].waypoint.transform.rotation;
            yield return new WaitForSeconds(1);
        }
        this.transform.position = startingPos;
        this.transform.rotation = startingRot;
        UIController.instance.HideCanvas(false);

        yield return null;
    }


    public TargetWaypointData GetNextWaypoint()
    {
        if (nextWaypointId != -1)
        {

            return waypoints[nextWaypointId];
        }
        else
        {
            return null;
        }    
    }

        

    public bool SetAnimatronicWaypoint(int waypointId)
    {
        nextWaypointId =waypointId + 1;
        var waypoint = waypoints[waypointId];
        this.gameObject.transform.position = waypoint.waypoint.transform.position;
        this.gameObject.transform.rotation = waypoint.waypoint.transform.rotation;

        AnimationClip clip;

                clip = animations.AnimationSets[endoType].animations[waypoint.condition.AnimationId].animation;
                OverrideController["V2EndoTestPose"] = clip;
                animatior.Play("PoseTest", 0);
                return true;
    }

    public bool SetAnimatronicWaypoint(TargetWaypointData waypoint)
    {       
        this.gameObject.transform.position = waypoint.waypoint.transform.position;
        this.gameObject.transform.rotation = waypoint.waypoint.transform.rotation;

        AnimationClip clip;

        clip = animations.AnimationSets[endoType].animations[waypoint.condition.AnimationId].animation;
        OverrideController["V2EndoTestPose"] = clip;
        animatior.Play("PoseTest", 0);
        return true;
    }


}



[System.Serializable]
public class TargetWaypointData
{
    public AnimatronicWaypoint waypoint;
    public string waypointName = "";
    //store the waypoints id at save, use to restore reference
    int waypointID = -1;
    public WaypointCondition condition = new WaypointCondition();


    public TargetWaypointData(AnimatronicWaypoint waypoint, string name)
    {
        this.waypoint = waypoint;
        this.waypointName = name;
    }

    /// <summary>
    /// convert condition data and generate rest of waypoint data for saves
    /// </summary>
    public WaypointConditionSavable GenerateSavedWaypointData()
    {
        var savedCondition = condition.GenerateSavableWaypoint();

        return savedCondition;
    }
}








    public enum waypointConditionSetting {none,DoorClosed,DoorOpen,PowerOn,PowerOff,PowerLessThan,PowerMoreThan,}
/// <summary>
/// a waypoint condition stores the details an animatronic uses to move through the map
/// </summary>
[System.Serializable]
public class WaypointCondition
{
    public DecorObject target = null;
    public int AnimationId = 0;
    //the id of the dropdown menu option used to set the condition, required for the repopulation of the UI on select
    public int DropdownReturnId = 0;
    public waypointConditionSetting condition = waypointConditionSetting.none;
    public WaypointConditionValues_base conditionValues;

    public WaypointCondition()
    {

    }

    public bool CheckCondition(/*DecorObject target, waypointConditionSetting condition*/)
    {


        switch (condition)
        {
            case waypointConditionSetting.none:
                return true;

            case waypointConditionSetting.DoorOpen:
                {
                    var door = (DecorSecurityDoor)target;

                    return door.IsOpen;
                }

            case waypointConditionSetting.DoorClosed:
                {
                    var door = (DecorSecurityDoor)target;
                    return !door.IsOpen;
                }

            case waypointConditionSetting.PowerLessThan:
                {
                    float value = ((WaypointConditionValues_Float)conditionValues).value;

                   return (NightManager.instance.powerPercent <= value) ?  true : false ;
                }
        }


        return false;
    }

    public WaypointCondition(WaypointConditionSavable waypointSavable)
    {
        if (waypointSavable.targetID != -1)
        {
            target = EditorController.Instance.MapDecor[waypointSavable.targetID];
        }
        DropdownReturnId = waypointSavable.DropdownReturnId;
        condition = waypointSavable.condition;
        conditionValues = waypointSavable.conditionValues;
    }

    /// <summary>
    /// converts the condition to a savable alternative
    /// </summary>
    /// <returns></returns>
    public WaypointConditionSavable GenerateSavableWaypoint()
    {
        return new WaypointConditionSavable(this);
    }


    /// <summary>
    /// Takes a waypointcondition savable and restores its values to this class
    /// </summary>
    public void ConvertSavableWaypointValues(WaypointConditionSavable waypointSavable)
    {
        if(waypointSavable.targetID != -1)
        {
            target = EditorController.Instance.MapDecor[waypointSavable.targetID];
        }
        DropdownReturnId = waypointSavable.DropdownReturnId;
        condition = waypointSavable.condition;
        conditionValues = waypointSavable.conditionValues;
        AnimationId = waypointSavable.AnimationId;
}
    
}

[System.Serializable]
public class WaypointConditionSavable
{
    public int targetID = -1;
    public int DropdownReturnId = 0;
    public int AnimationId = 0;
    public waypointConditionSetting condition = waypointConditionSetting.none;
    public WaypointConditionValues_base conditionValues;

    public WaypointConditionSavable(WaypointCondition waypointCondition)
    {
        if (waypointCondition.target != null)
        {
            this.targetID = EditorController.Instance.MapDecor.IndexOf(waypointCondition.target);
        }
        else
        {
            this.targetID = -1;
        }

        this.AnimationId = waypointCondition.AnimationId;
        this.DropdownReturnId = waypointCondition.DropdownReturnId;
        this.condition = waypointCondition.condition;
        this.conditionValues = waypointCondition.conditionValues;
    }

    public WaypointCondition ConvertToWaypointCondition()
    {
        return new WaypointCondition(this);
    }
}

