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






    /// Old conversion code from converting from map object to catalogue object
    /*
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
                var item = catalogue.MapObjects[id];
                Debug.Log("found matching id");

                string path = "Assets/ObjectPrefabs"; 
                if (AssetDatabase.IsValidFolder(path + "/" + item.InternalName) == false)
                {
                    AssetDatabase.CreateFolder(path, item.InternalName);
                }
               var newObject = ScriptableObject.CreateInstance<CatalogueObject>();
                newObject.SetData(item);

                AssetDatabase.CreateAsset(newObject, $"Assets/ObjectPrefabs/{item.InternalName}/{item.InternalName}.asset" );
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            

           // AssetDatabase.CreateFolder("Assets/ObjectPrefabs", )
        }

        if (GUILayout.Button("Convert All Data") && catalogue != null)
        {
            Debug.Log("Converting all catalogue data");
            catalogue.WriteDictionary();

            string path = "Assets/ObjectPrefabs";
            for (int i = 0; i < catalogue.MapObjects.Count; i++)
            {
                var item = catalogue.MapObjects[i];
                var objectpath = path + "/" + item.InternalName;
                // create folder
                if (AssetDatabase.IsValidFolder(objectpath) == false)
                {
                    AssetDatabase.CreateFolder(path, item.InternalName);
                }

                var newObject = ScriptableObject.CreateInstance<CatalogueObject>();
                newObject.SetData(item);


                AssetDatabase.CreateAsset(newObject, $"Assets/ObjectPrefabs/{item.InternalName}/{item.InternalName}.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

               // CatalogueObject savedObject = (CatalogueObject)AssetDatabase.LoadAssetAtPath($"Assets/ObjectPrefabs/{item.InternalName}/{item.InternalName}.asset", typeof(CatalogueObject));
                
                for (int j = 0; j < item.Swatches.Count; j++)
                {
                    var swatch = GenerateSwatchData(item, j);

                    
                    AssetDatabase.CreateAsset(swatch, $"Assets/ObjectPrefabs/{item.InternalName}/{item.InternalName}_Swatch{j}.asset");

                   // DecorSwatch savedSwatch = (DecorSwatch)AssetDatabase.LoadAssetAtPath($"Assets/ObjectPrefabs/{item.InternalName}/{item.InternalName}.asset", typeof(DecorSwatch));

                    
                    var element = savedObject.Swatches.ElementAt(savedObject.Swatches.Count - 1);

                    element = savedSwatch;
                    
                    //EditorUtility.SetDirty(savedObject);
                }             
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }



        if (GUILayout.Button("Collect Swatches"))
        {
           var swatches = AssetDatabase.LoadAllAssetsAtPath($"Assets/ObjectPrefabs/{Target.InternalName}");

            for (int i = 1; i < swatches.Length; i++)
            {
                Target.Swatches.Add((DecorSwatch)swatches[i]);
                EditorUtility.SetDirty(Target);
            }
            
        }
            this.Repaint();
    }


    DecorSwatch GenerateSwatchData(MapObject targetObject,int SwatchId)
    {

        var newSwatch = ScriptableObject.CreateInstance<DecorSwatch>();

        newSwatch.SetData(targetObject.Swatches[SwatchId], targetObject.InternalName + " Swatch " + SwatchId);

        return newSwatch;
    }

    */
}
