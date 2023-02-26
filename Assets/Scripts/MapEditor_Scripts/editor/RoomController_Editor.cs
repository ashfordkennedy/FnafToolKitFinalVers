using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor (typeof(RoomController))]
public class RoomController_Editor : Editor
{

    public override void OnInspectorGUI()
    {
        RoomController controller = target as RoomController;
        base.OnInspectorGUI();


        if(GUILayout.Button("Save RoomMesh"))
        {
           var container = new GameObject();
           container.transform.position = controller.FindRoomCentre();
           var filter = container.AddComponent<MeshFilter>();

            MeshCombiners.GenerateMesh<ConsolidateRoomMesh>(controller.RoomCells, container.transform, filter);
            
            AssetDatabase.CreateAsset(filter.mesh, "Assets/GeneratedRoom");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
