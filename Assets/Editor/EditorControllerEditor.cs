using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

//[CustomEditor(typeof(EditorController))]
public class EditorControllerEditor : Editor
{
    /*



    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        {
            EditorController TargetEC = (EditorController)target;
            
            if (GUILayout.Button("OrderByTab"))
            {
                TargetEC.MapObjects = TargetEC.MapObjects.OrderBy(MapObject => MapObject.EditorTab).ToList<MapObject>();
               /* for (int i = 0; i < TargetEC.MapObjects.Count; i++)
                {
                   
                }

            }


            if (GUILayout.Button("RenumberIndexes"))
            {
                TargetEC.MapObjects = TargetEC.MapObjects.OrderBy(MapObject => MapObject.EditorTab).ToList<MapObject>();
                 for (int i = 0; i < TargetEC.MapObjects.Count; i++)
                 {
                    TargetEC.MapObjects[i].ObjectId = i;
                 }
 
            }

            if (GUILayout.Button("ReinsertSelectedIndex"))
            {
                if (TargetEC.SelectedIndex <= TargetEC.MapObjects.Count && TargetEC.IndexDestination <= TargetEC.MapObjects.Count && TargetEC.SelectedIndex >= 0 && TargetEC.IndexDestination >= 0) {
                    var backup = TargetEC.MapObjects[TargetEC.SelectedIndex];
                    TargetEC.MapObjects.RemoveAt(TargetEC.SelectedIndex);
                    TargetEC.MapObjects.Insert(TargetEC.IndexDestination, backup);

                    for (int i = 0; i < TargetEC.MapObjects.Count; i++)
                    {
                        TargetEC.MapObjects[i].ObjectId = i;
                    }

                }
            }


        }


        
    }


   */
}
