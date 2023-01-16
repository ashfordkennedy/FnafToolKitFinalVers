using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwatchUI : EditorMenuAbstract
{
    public static SwatchUI Instance;
    public GameObject SwatchObjectPrefab;
    public Transform Content;
    public DecorObject target = null;

    [SerializeField] DecorObjectCatalogue Catalogue;

    private void Awake()
    {
        Instance = this;
    }




    public void OpenSwatchPanel(string objectname, DecorObject Target)
    {
       
        foreach (Transform child in Content)
        {
            GameObject.Destroy(child.gameObject);
        }

       // print("opening swatches");
        target = Target;
        int objectID = -1;
       if(Catalogue.ObjectDictionary.TryGetValue(objectname, out objectID) == true)
        {
            ///Item exists in the dictionary. Process swatches
            ProcessSwatches(objectID);
            base.OpenMenu();
        }       
    }

     


    public override void CloseMenu()
    {
        base.CloseMenu();
        target = null; 
    }

    private void ProcessSwatches(int ObjectID)
    {       

        for (int i = 0; i < Catalogue.MapObjects[ObjectID].Swatches.Count; i++)
        {

            Toggle NewSwatch = Instantiate(SwatchObjectPrefab, Content).GetComponent<Toggle>();
            NewSwatch.image.sprite = Catalogue.MapObjects[ObjectID].Swatches[i].swatchIcon;         
            NewSwatch.gameObject.SetActive(true);
        }
    }



    /// <summary>
    /// processes new swatch for object
    /// </summary>
    /// <param name="swatch"></param>
    public void SwatchSelect(GameObject swatch)
    {
        if (target != null)
        {
            int newswatch = swatch.transform.GetSiblingIndex();


            int objectID = -1;
            if (Catalogue.ObjectDictionary.TryGetValue(target.InternalName, out objectID) == true)
            {
                print("settingswatch");
                AudioManager.Audio_M.PlayUIClick();
                target.SwatchSwap(Catalogue.MapObjects[objectID].Swatches[newswatch].meshes, Catalogue.MapObjects[objectID].Swatches[newswatch].materials,newswatch);


            }
            // target.SwatchSwap()
        }
    }





}
