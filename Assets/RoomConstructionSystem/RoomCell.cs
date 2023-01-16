using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// Room Cells are the objects which allow for room editing, They store no data, Simply access the rooms Controller and report changes.
/// </summary>
/// 

[Serializable]
public class RoomCell : MonoBehaviour
{
    //  public WallData[] Wall_Data;
    public RoomController Room_Ctrl = null;
    public WallType[] Wall_Data;
    public MeshFilter[] InnerCorners;
    public MeshRenderer[] InnerCornerRenderers;
    public MeshRenderer FloorRenderer;
    public MeshFilter FloorFilter;
    /// <summary>
    /// 0 = North, 1 = east, 2 = south, 3 = west
    /// </summary>
    [Header("0 = North, 1 = east, 2 = south, 3 = west")]
    public WallComponent[] Walls;
    public Vector2Int CellID;
    private string _wallFormat = "";
    public List<MeshFilter> CompileWallMeshData()
    {
        List<MeshFilter> MergableMeshes = new List<MeshFilter>();

        for (int x = 0; x < Walls.Length; x++)
        {
            if (Walls[x].gameObject.activeInHierarchy == true)
            {
                MergableMeshes.Add(Walls[x].WallMesh);
                print("adding wall to list");
            }
        }

        for (int x = 0; x < InnerCorners.Length; x++)
        {
            if (InnerCorners[x].gameObject.activeInHierarchy == true)
            {
                MergableMeshes.Add(InnerCorners[x]);
            }
        }

        return MergableMeshes;
    }



    public void UpdateMaterial(Material NewWallMat, Material NewFloorMat)
    {
        Walls[0].Renderer.material = NewWallMat;
        Walls[1].Renderer.material = NewWallMat;
        Walls[2].Renderer.material = NewWallMat;
        Walls[3].Renderer.material = NewWallMat;
        InnerCornerRenderers[0].material = NewWallMat;
        InnerCornerRenderers[1].material = NewWallMat;
        InnerCornerRenderers[2].material = NewWallMat;
        InnerCornerRenderers[3].material = NewWallMat;
        FloorRenderer.material = NewFloorMat;
    }

    public void CellInitialize()
    {
        Room_Ctrl = EditorController.Instance.SelectedRoom;
        Room_Ctrl.RegisterCell(CellID, this, true);


    }


    /// <summary>
    /// Sets wall states and meshes after having set their states before calling
    /// </summary>
    /// <param name="wall"></param>
    /// <param name="WallSet"></param>
    void ReloadWallMesh(WallComponent wall, int WallSet, Material wallmat)
    {

        switch (wall.Wall_State)
        {


            case WallState.Empty:
                wall.UpdateWallMesh(null, null);
              //  wall.gameObject.SetActive(false);
               
                break;


            case WallState.Straight:
                wall.UpdateWallMesh(EditorController.Instance.WallSets[WallSet].straightWalls[(int)wall.Wall_Type], wallmat);
                break;

            case WallState.BothCorner:
                wall.UpdateWallMesh(EditorController.Instance.WallSets[WallSet].bothCornerWalls[(int)wall.Wall_Type], wallmat);
                break;

            case WallState.LeftCorner:
                wall.UpdateWallMesh(EditorController.Instance.WallSets[WallSet].leftCornerWalls[(int)wall.Wall_Type], wallmat);
                break;

            case WallState.RightCorner:
                wall.UpdateWallMesh(EditorController.Instance.WallSets[WallSet].rightCornerWalls[(int)wall.Wall_Type], wallmat);
                break;

        }
    }


    /// <summary>
    /// Restores a cell from its saved state 
    /// </summary>
    /// <param name="CD"></param>
    public void ReconstructCell(CellSaveData CD, int WallSet, int wallSkin, int floorSet)
    {
        Material mat = EditorController.Instance.WallSets[WallSet].Swatches[wallSkin].material;


        Walls[0].Wall_State = CD.NorthWall;
        Walls[0].Wall_Type = CD.NorthType;
        ReloadWallMesh(Walls[0],WallSet, mat);

        Walls[1].Wall_State = CD.EastWall;
        Walls[1].Wall_Type = CD.EastType;
        ReloadWallMesh(Walls[1], WallSet, mat);

        Walls[2].Wall_State = CD.SouthWall;
        Walls[2].Wall_Type = CD.SouthType;
        ReloadWallMesh(Walls[2], WallSet, mat);

        Walls[3].Wall_State = CD.WestWall;
        Walls[3].Wall_Type = CD.WestType;
        ReloadWallMesh(Walls[3], WallSet, mat);

        InnerCorners[0].gameObject.SetActive(CD.Corner0);
        InnerCorners[1].gameObject.SetActive(CD.Corner1);
        InnerCorners[2].gameObject.SetActive(CD.Corner2);
        InnerCorners[3].gameObject.SetActive(CD.Corner3);

        UpdateCorners(EditorController.Instance.WallSets[WallSet].InnerCorner, mat);

        FloorRenderer.material = EditorController.Instance.Floors[floorSet].material;
    }


    public void UpdateCorners(Mesh CornerMesh, Material CornerMat)
    {
        InnerCorners[0].mesh = CornerMesh;
        InnerCorners[1].mesh = CornerMesh;
        InnerCorners[2].mesh = CornerMesh;
        InnerCorners[3].mesh = CornerMesh;
    }

    public void EraseCell()
    {
        Room_Ctrl.DeregisterCell(CellID);
        Destroy(this.gameObject);
    }



    /// <summary>
    /// generates the ABCD Code used to define the state of the cell walls
    /// </summary>
    public void GenerateFormat()
    {
        string newFormat = "";


            if(Walls[0].gameObject.activeInHierarchy == true)
            {
            newFormat += "A";
            }
        if (Walls[1].gameObject.activeInHierarchy == true)
        {
            newFormat += "B";
        }
        if (Walls[2].gameObject.activeInHierarchy == true)
        {
            newFormat += "C";
        }
        if (Walls[3].gameObject.activeInHierarchy == true)
        {
            newFormat += "D";
        }

        _wallFormat = newFormat;



    }

}


/// <summary>
/// Used to pass Cell wall data on to the mesh combiner
/// </summary>
public struct  CellWallStates{
    public bool North;
    public bool East;
    public bool South;
    public bool West;

    public bool Corner0;
    public bool Corner1;
    public bool Corner2;
    public bool Corner3;

    public CellWallStates(bool north, bool east, bool south, bool west, bool corn0, bool corn1, bool corn2, bool corn3 )
    {
        this.North = north;
        this.East = east;
        this.South = south;
        this.West = west;
        this.Corner0 = corn0;
        this.Corner1 = corn1;
        this.Corner2 = corn2;
        this.Corner3 = corn3;


    }

   
        }






