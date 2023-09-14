using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor_MouseMode_TargetSelect : Editor_MouseMode_Abstract
{
    public static Editor_MouseMode_TargetSelect instance;

    public void Awake()
    {
        instance = this;
    }




    public override void EnableMouseMode()
    {
        base.EnableMouseMode();
    }

    public override void DisableMouseMode()
    {
        base.DisableMouseMode();
    }
}
