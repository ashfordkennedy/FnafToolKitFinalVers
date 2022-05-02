using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : CanvasSetManager
{
    public static UIController instance;

    private void Awake()
    {
        instance = this;
    }



}
