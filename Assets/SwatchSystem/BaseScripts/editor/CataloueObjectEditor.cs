using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
[CustomEditor(typeof(CatalogueObject))]
public class CataloueObjectEditor : Editor
{

    

    private string m_id = "";
    private string directory = "";
    DecorObjectCatalogue catalogue;
    public override void OnInspectorGUI()
    {
        CatalogueObject Target = (CatalogueObject)target;
        if (catalogue == null)
        {
            string[] guids = AssetDatabase.FindAssets("ObjectCatalogue");
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var catalogued = AssetDatabase.LoadAssetAtPath(path, typeof(DecorObjectCatalogue));
            catalogue = (DecorObjectCatalogue)catalogued;
            Debug.Log("catalogue found");
        }


        base.OnInspectorGUI();

         m_id = EditorGUILayout.TextField("InternalID", m_id);

        if (GUILayout.Button("ConvertData") && catalogue != null)
        {
            Debug.Log("Converting catalogue data");
            catalogue.WriteDictionary();

            int id;
            if(catalogue.ObjectDictionary.TryGetValue(m_id,out id)){

                Debug.Log("found matching id");
            }
            

           // AssetDatabase.CreateFolder("Assets/ObjectPrefabs", )
        }
        this.Repaint();
    }


}
