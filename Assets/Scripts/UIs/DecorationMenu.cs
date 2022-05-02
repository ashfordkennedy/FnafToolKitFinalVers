using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DecorationMenu : MainBarTab
{
    public enum DecorationFilters { };

    public EditorTab catalogueType = EditorTab.Decor;
    public static DecorationMenu instance;
    public DecorObjectCatalogue objectCatalogue;
    [SerializeField] Transform container;
    [SerializeField] GameObject decorButtonPrefab;

    [SerializeField] TMP_Text _descriptionText;
    [SerializeField] TMP_Text _nameText;
    [SerializeField] TMP_Text _priceText;

    private void Awake()
    {
        instance = this;
        GenerateCatalogueButtons(catalogueType);
        // _animator.openAnimator.SetToEnd();
    }



    void GenerateCatalogueButtons(EditorTab editorTab)
    {
        for (int i = 0; i < objectCatalogue.MapObjects.Count; i++)
        {
            if (objectCatalogue.MapObjects[i].editorTab == editorTab)
            {
                GameObject newButton = Instantiate(decorButtonPrefab, container);
                Button button = newButton.GetComponent<Button>();

                MapObject itemData = objectCatalogue.MapObjects[i];
                newButton.name = "" + i;
                (button.targetGraphic as Image).sprite = itemData.Menusprite;
                newButton.SetActive(true);
            }
        }
        

    }

    public void GenerateCatalogueButton(int itemIndex)
    {
       GameObject newButton = Instantiate(decorButtonPrefab, container);
        Button button = newButton.GetComponent<Button>();

       MapObject itemData = objectCatalogue.MapObjects[itemIndex];
        newButton.name = "" + itemIndex;
        (button.targetGraphic as Image).sprite = itemData.Menusprite;




    }


    public void DisplayItemData(Transform target)
    {
        int id = int.Parse(target.gameObject.name);
        var targetObject = objectCatalogue.MapObjects[id];
        _descriptionText.text = targetObject.Description;
        _nameText.text = targetObject.name;
        _priceText.text = "$" + targetObject.Price; 

    }

    public void EraseDisplayText()
    {
        _descriptionText.text = "";
        _nameText.text = "";
        _priceText.text = "";
    }

    public void SelectItem(Transform target)
    {
        int id = int.Parse(target.gameObject.name);
        GameObject targetObject = objectCatalogue.MapObjects[id].Object;
        //old - ObjectPlacementWidgit.PlacementWidgit.CreateBlueprint(targetObject);
        RoomEditorMouse.Instance.ChangeMouseMode(3);
        ObjectPlacer.instance.SpawnNewObject(targetObject, true);
        

    }

    public void PlayHoverSound()
    {
        AudioManager.Audio_M.PlayHoverSound();
    }


    public void FilterItems()
    {





    }




    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
    }
}
