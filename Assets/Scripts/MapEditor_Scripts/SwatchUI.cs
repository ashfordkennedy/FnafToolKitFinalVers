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

    CatalogueObject catalogueObject; 
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

       catalogueObject = Catalogue.GetCatalogueData(objectname);
       if(catalogueObject != null)
        {
            ///Item exists in the dictionary. Process swatches
            ProcessSwatches(catalogueObject);
            base.OpenMenu();
        }       
    }

     


    public override void CloseMenu()
    {
        base.CloseMenu();
        target = null; 
    }

    private void ProcessSwatches(CatalogueObject catalogueObject)
    {       

        for (int i = 0; i < catalogueObject.Swatches.Count; i++)
        {

            Toggle NewSwatch = Instantiate(SwatchObjectPrefab, Content).GetComponent<Toggle>();
            NewSwatch.image.sprite = catalogueObject.Swatches[i].swatchIcon;         
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


            if (catalogueObject != null)
                print("settingswatch");
                AudioManager.Audio_M.PlayUIClick();
                target.SwatchSwap(catalogueObject.Swatches[newswatch].meshes, catalogueObject.Swatches[newswatch].materials,newswatch);


            }
            // target.SwatchSwap()
        }
    }
