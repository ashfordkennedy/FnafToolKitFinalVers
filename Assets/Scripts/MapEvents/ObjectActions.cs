using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
//using UnityEditor.SceneManagement;
using UnityEngine.Events;
/*
public class ObjectActions : MonoBehaviour
{
    public List<KeyValuePair<string, dynamic>> Events;
    private delegate void blankReference();
    private blankReference methodReference;

    public void RegisterEvents(UnityEvent targetEvent)
    {
        for (int i = 0; i < Events.Count; i++)
        {
            
           // targetEvent.AddListener(t);
        }
    }

    private void GenerateEventCall(KeyValuePair<string, bool> Event)
    {
        var returnValue = 0;

        switch (Event.Key)
        {

        }


       // return returnValue;
    }

    void Dummymethod()
    {

    }

    void Dummymethod2(int Int)
    {

    }

}




public enum ObjectActionType {Float,Bool,Transform}
public enum ObjectAction_FloatCondition {equal, minus, plus, divide, times}
public enum ObjectAction_BoolCondition {True,False,Switch}
[System.Serializable]
public abstract class ObjectAction
{
    public string Name = "";
    public string ActionIdentifier = "None";
}

public class ObjectAction_Bool : ObjectAction
{

    ObjectAction_BoolCondition SetState = ObjectAction_BoolCondition.True;

}





/*

[CustomEditor(typeof(ObjectActions))]
public class ObjectActionsEditor : Editor
{

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
      //  ObjectActions target = this.target as ObjectActions;

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
          //  EditorSceneManager.MarkSceneDirty(target.gameObject.scene);
        }
    }
}
*/
