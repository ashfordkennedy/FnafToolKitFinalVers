using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CatalogueObject", menuName = "DecorSystem/WallSet", order = 2)]
public class WallMeshSet : ScriptableObject
{
    [Header("")]
    [Header("0 Blank, 1 Window, 2 Door, 3 Vent, 4 DoorLeft, 5 DoorCenter, 6 DoorRight }")]
    [Header("Fill in as appropriate with each variant under the indexs above, both corners do not need window and door pieces}")]
    public new string name;
    public Mesh[] straightWalls = new Mesh[7];
    public Mesh[] leftCornerWalls = new Mesh[7];
    public Mesh[] rightCornerWalls = new Mesh[7];
    public Mesh[] bothCornerWalls = new Mesh[7];
    public Mesh InnerCorner = null;

    public List<WallSwatch> Swatches = new List<WallSwatch>();

    public void Setup(WallSet data)
    {
        name = data.name;
        straightWalls = data.StraightWalls;
        leftCornerWalls = data.LeftCornerWalls;
        rightCornerWalls = data.RightCornerWalls;
        bothCornerWalls = data.BothCornerWalls;
        InnerCorner = data.InnerCorner;
        Debug.Log("wall data recieved");
    }


}



