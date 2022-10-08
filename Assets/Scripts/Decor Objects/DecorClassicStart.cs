using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorClassicStart : DecorObject
{
    public static DecorClassicStart instance;
    private bool[] ActiveNights = new bool[7];
    public float lowerBound = 90;
    public float upperBound = 90;


    public override void NightStartSetup()
    {
       
    }

    private void OnEnable()
    {
        if(instance != null)
        {
            DestroyObject();
        }
    }

    private void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }


    


    public void StartPreview()
    {
        PlayerController_Classic.instance.BeginPreview(this.transform,lowerBound,upperBound);
        GuiController.instance.InitialiseNightGui();
        GuiController.instance.EnableOfficeHudGui(true);
        StartCoroutine(PreviewExit());

    }

    private IEnumerator PreviewExit()
    {
        while(!Input.GetKey(KeyCode.Escape))
        {
           yield return new WaitForEndOfFrame();
            yield return null;
        }
        StopPreview();

    }

    public void StopPreview()
    {
        PlayerController_Classic.instance.EndPreview();
        GuiController.instance.EnableOfficeHudGui(false);
        GuiController.instance.DisableNightGui();
    }

    public void RestoreObjectData(ClassicStartData startData)
    {
        this.ActiveNights = startData.activeNights;
        this.lowerBound = startData.lowerBound;
        this.upperBound = startData.upperBound;

        

    }

    public override SavedObject CompileObjectData()
    {
        ClassicStartData startData = new ClassicStartData(ObjectSaveDataType.ClassicStart, ActiveNights,lowerBound,upperBound);
        SavedObject SO = new SavedObject(InternalName, SwatchID,startData, new SavedTransform(transform));

        return SO;
    }
}



[System.Serializable]
public class ClassicStartData : ObjectSaveData
{
    public bool[] activeNights = new bool[7];
    public float lowerBound = 90;
    public float upperBound = 90;
    public ClassicStartData(ObjectSaveDataType dataType, bool[] activeNights, float lowerBound, float upperBound) : base(dataType)
    {
        base.DataType = ObjectSaveDataType.ClassicStart;
        //this.DataType = ObjectSaveDataType.ClassicStart;
        this.activeNights = activeNights;
        this.upperBound = upperBound;
        this.lowerBound = lowerBound;
        this.activeNights = activeNights;
    }

}
