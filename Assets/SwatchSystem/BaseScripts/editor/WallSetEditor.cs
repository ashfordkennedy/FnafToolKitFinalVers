using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using UnityEditor.SceneManagement;
[CustomEditor(typeof(WallMeshSet))]
public class WallSetEditor : Editor
{
    /*
    List<WallSet> WallSets;
    EditorController editorController;
    public override void OnInspectorGUI()
    {
        WallMeshSet Target = (WallMeshSet)target;
        if (WallSets == null)
        {
            var editorscript = EditorSceneManager.GetActiveScene().GetRootGameObjects();
            for (int i = 0; i < editorscript.Length; i++)
            {
                if (editorscript[i].name == "EditorController")
                {
                    Debug.Log("controller found");
                    editorController = editorscript[i].GetComponent<EditorController>();
                  //  WallSets = editorController.WallSets;
                }

            }
        }


        base.OnInspectorGUI();

        if (GUILayout.Button("Convert All Data") && WallSets != null)
        {
            Debug.Log("Converting all Wall data");


            string path = "Assets/ObjectPrefabs";
            for (int i = 0; i < WallSets.Count; i++)
            {
                var item = WallSets[i];
                var objectpath = path + "/" + item.name;
                // create folder
                if (AssetDatabase.IsValidFolder(objectpath) == false)
                {
                    AssetDatabase.CreateFolder(path, item.name);
                }

                var newObject = ScriptableObject.CreateInstance<WallMeshSet>();
                newObject.Setup(WallSets[i]);



                AssetDatabase.CreateAsset(newObject, $"Assets/ObjectPrefabs/{item.name}/{item.name}.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();


            }

            this.Repaint();
        }
    }
    */
}
    

