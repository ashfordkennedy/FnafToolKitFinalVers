using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CatalogueObject", menuName = "DecorSystem/CatalogueObject", order = 1)]
public class CatalogueObject : ScriptableObject
{

    public DecorType CatalogueTags;
    public DecorTheme CatalogueTheme;

    public string name = "Object";
    public string InternalName = "";
    public EditorTab editorTab = EditorTab.Decor;
    public List<DecorSwatch> Swatches;
    [TextArea] public string Description = "";
    public int Price;
    public GameObject Object;
    public Sprite Menusprite;
    //public int ObjectId;

}
