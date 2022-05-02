using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor;
public enum EditorTabs{WallTools, Decor, Gameplay, Animatronics }
public class EditorTabControls : MonoBehaviour
{
    public static EditorTabControls EditorTabs;
    [SerializeField] Image PanelBG;
    [SerializeField] Image[] Tabs;
    [SerializeField] GameObject[] TabContentBars;
    [SerializeField] GameObject[] TabContentSlots;
    [SerializeField] GameObject CatalogueButtonPrefab;
    RectTransform rectTransform;
    public bool TabsExpanded = false;

   [SerializeField] DecorObjectCatalogue Catalogue;







    // Start is called before the first frame update
    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
        EditorTabs = this;
        PopulateEditorTabs();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ExpandTabs(bool expand)
    {    
      var NewPos = new Vector2(0,0);
        float LerpAmount = 0.05f;
        switch (expand)
        {
            case true:
              
                NewPos = new Vector2(0.5f, 0);
             

                break;

            case false:
               NewPos = new Vector2(0.5f,1);
              
                break;

        }
        yield return new WaitForSeconds(0.01f);
        var currentPos = rectTransform.pivot;
        while (LerpAmount <= 1)
        {
            rectTransform.pivot = Vector2.Lerp(currentPos, NewPos, LerpAmount);
            LerpAmount += 0.1f;
            yield return new WaitForSeconds(0.01f);

        }
        rectTransform.pivot = NewPos;    
        yield return null;
    }

    public void SwapTab(int TabId)
    {
       
        if (PanelBG.color == Tabs[TabId].color && TabsExpanded == true)
        {
            StartCoroutine("ExpandTabs",false);
            TabsExpanded = false;
        }
        else if(TabsExpanded == false)
        {
            StartCoroutine("ExpandTabs",true);
            TabsExpanded = true;
        }

        PanelBG.color = Tabs[TabId].color;
      
        for (int i = 0; i < TabContentBars.Length; i++)
        {
            TabContentBars[i].SetActive(false);
        }


            TabContentBars[TabId].SetActive(true);

    }


    public void PopulateEditorTabs() {
        var editorObjects = Catalogue.MapObjects;

        for (int i = 0; i < editorObjects.Count; i++)
        {

            GameObject GroupParent = null;
            switch(editorObjects[i].editorTab){

                case EditorTab.Decor:
                    GroupParent = TabContentSlots[1];
                    break;

                

                case EditorTab.GamePlay:
                    GroupParent = TabContentSlots[2];

                    break;

              

                case EditorTab.Animatronic:
                    GroupParent = TabContentSlots[3];


                    break;
            }
            int x = new int();
            x = i;
            GameObject butObj = Instantiate(CatalogueButtonPrefab, GroupParent.transform);

            Button NewButton = butObj.GetComponent<Button>();
            NewButton.image.sprite = editorObjects[i].Menusprite;
            NewButton.name = editorObjects[i].InternalName;
            NewButton.gameObject.SetActive(true);
            //   editorObjects[i].ObjectId
            // NewButton.onClick.AddListener(() => EditorController.Instance.PlaceObjectSetup(2));

            butObj.GetComponent<Button>().onClick.AddListener(delegate { EditorController.Instance.PlaceObjectSetup(editorObjects[x].ObjectId); });
            
        }
    }

    public void test(string ok)
    {
        Debug.Log(ok);
    }
}
