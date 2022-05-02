using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class DecorSecurityDoor : DecorObject
{
    public float PowerDrain = 0.05f;
    public bool StartOpen = true;
    public bool IsOpen = true;
    [SerializeField] Animator DoorAnimator;
    [SerializeField] AudioClip DoorSFX;
    new public static List<Dropdown.OptionData> conditions = new List<Dropdown.OptionData> {
        new Dropdown.OptionData("Open"),
        new Dropdown.OptionData("Closed"),
    };

    public override List<Dropdown.OptionData> GetConditionOptions()
    {
        return conditions;
    }

    public override waypointConditionSetting GetConditionEnum(string condition)
    {
        waypointConditionSetting value = waypointConditionSetting.none;

        switch (condition)
        {
            case "None":
                value = waypointConditionSetting.none;
                break;

            case "Open":
                value = waypointConditionSetting.DoorOpen;
                break;

            case "Closed":
                value = waypointConditionSetting.DoorClosed;
                break;
        }
        return value;
    }


    public void SetDoorOpen(bool Open)
    {
        IsOpen = Open;
        DoorAnimator.SetBool("IsOpen", IsOpen);

        switch (IsOpen)
        {
            case true:
                NightManager.instance.RegisterPowerLoss(PowerDrain);
                break;

            case false:
                NightManager.instance.RemovePowerLoss(PowerDrain);
                break;
        }
    }


    public void ToggleDoor()
    {
        AudioManager.Audio_M.BGS_Player.PlayOneShot(DoorSFX);
        IsOpen = !IsOpen;
        DoorAnimator.SetBool("IsOpen", IsOpen);

        switch (IsOpen)
        {
            case true:
                NightManager.instance.RemovePowerLoss(PowerDrain);
               
                break;

            case false:
                NightManager.instance.RegisterPowerLoss(PowerDrain);
                break;
        }
    }



    public override void SwatchSwap(Mesh[] mesh, Material[] mats, int swatchid)
    {
        SwatchID = swatchid;
        var filter = this.transform.GetChild(0).GetComponent<MeshFilter>();
        var rend = this.transform.GetChild(0).GetComponent<MeshRenderer>();

        filter.mesh = mesh[0];
        rend.materials = mats;
     //   base.SwatchSwap(mesh, mats, swatchid);
    }


    public override void NightStartSetup()
    {
        if(StartOpen == true)
        {
            SetDoorOpen(true);

        }
        else
        {
            SetDoorOpen(false);
        }
    }



}



public class SecurityDoorSaveData : ObjectSaveData
{
    public float PowerDrain;
    public bool StartOpen;

    public SecurityDoorSaveData(float Powerdrain, bool startopen, ObjectSaveDataType dataType) : base(dataType)
    {
        this.DataType = dataType;
        this.PowerDrain = Powerdrain;
        this.StartOpen = startopen;

    }
    }
