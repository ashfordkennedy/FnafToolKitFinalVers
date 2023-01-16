using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { North,East,South,West}
/// <summary>
/// Maintains and stores all wall data for the room.
/// </summary>
public class RoomController : MonoBehaviour
{
    public string Name;
    public RoomCell[,] RoomCells = new RoomCell[50, 50];
    private EditorController Map_Editor;
    public int WallSet = 0;
    public int WallSkin = 0;
    public int FloorSet = 0;
    public int CellCount = 0;
    public GameObject EditModeContainer;
    public MeshFilter CombinedRoomMesh;
    public MeshRenderer CombinedRoomRenderer;

    // Start is called before the first frame update
    void Start()
    {
        Map_Editor = EditorController.Instance;
    }

    /// <summary>
    /// Check adjacent cells wall based on given Direction
    /// </summary>
    /// <param name="x">Cell X ID</param>
    /// <param name="y">Cell Y ID</param>
    /// <param name="DirectionMode"> The Direction you want to perform the check </param>
    void NewWallCheck(int x, int y, Direction DirectionMode)
    {

        var offsets =  new CellOffsets(new Vector2Int(x, y), DirectionMode);
        var cell = RoomCells[x, y];
       
        //Check for Adjoining Cells
        {

            var currentCellID = offsets.originalPosition;
            RoomCell OppossingWallCell = RoomCells[offsets.offsetPosition.x, offsets.offsetPosition.y];
            RoomCell WallCell = RoomCells[x, y];

            int OffsetDirection = (int)offsets.opposingDirection;
            int WallDirection = (int)offsets.originalDirection;


            if (RoomCells[offsets.offsetPosition.x, offsets.offsetPosition.y] != null)
            {

               
                OppossingWallCell.Walls[OffsetDirection].UpdateWallMesh(null, null);
                cell.Walls[WallDirection].UpdateWallMesh(null, null);

                // disable obj
                cell.Walls[WallDirection].gameObject.SetActive(false);
               
            }
            else
            {
                // reuse this line in the wall formatter               
                cell.Walls[WallDirection].gameObject.SetActive(true);
                cell.Walls[WallDirection].Wall_State = WallState.Straight;
            }



        }
    }

    public void SetRoomMaterials()
    {
        /*
        var mats = CombinedRoomRenderer.materials;
        mats[1] = EditorController.Instance.WallSets[WallSet].MaterialSets[WallSkin].material;
        mats[0] = EditorController.Instance.Floors[FloorSet].material;
        CombinedRoomRenderer.materials = mats;
        */
    }



    /// <summary>
    /// Runs wallcheck for a single given cell
    /// </summary>
    public void SingleCellWallFormat(int x, int y)
    {

        NewWallCheck(x, y, Direction.North);
        NewWallCheck(x, y, Direction.South);
        NewWallCheck(x, y, Direction.East);
        NewWallCheck(x, y, Direction.West);

        WallFormatter(x, y, 0);
        WallFormatter(x, y, 1);
        WallFormatter(x, y, 2);
        WallFormatter(x, y, 3);

        CornerFormatter(x, y);
        RoomCells[x, y].UpdateMaterial(EditorController.Instance.WallSets[WallSet].Swatches[WallSkin].material, EditorController.Instance.Floors[FloorSet].material);
        CameraMapGenerator.instance.DrawCell(new Vector2Int(x, y), RoomCells[x, y]);
    }










    /// <summary>
    /// Runs wallcheck on every cell in the room
    /// </summary>
    public void RoomWallCheck()
    {
        int XLowest = RoomCells.GetLowerBound(0);
        int YLowest = RoomCells.GetLowerBound(1);
        for (int x = 0; x < RoomCells.GetLength(0); x++)
        {
            for (int y = 0; y < RoomCells.GetLength(1); y++)
            {
                if (RoomCells[x, y] != null && x != 0)
                {
                    NewWallCheck(x, y, Direction.North);
                    NewWallCheck(x, y, Direction.South);
                    NewWallCheck(x, y, Direction.East);
                    NewWallCheck(x, y, Direction.West);

                    WallFormatter(x, y, 0);
                    WallFormatter(x, y, 1);
                    WallFormatter(x, y, 2);
                    WallFormatter(x, y, 3);

                    CornerFormatter(x, y);

                    RoomCells[x, y].UpdateMaterial(EditorController.Instance.WallSets[WallSet].Swatches[WallSkin].material, EditorController.Instance.Floors[FloorSet].material);

                    CameraMapGenerator.instance.DrawCell(new Vector2Int(x, y), RoomCells[x, y]);
                }

            }
        }

    }

    WallState r = WallState.BothCorner;
    /// <summary>
    /// Checks cell walls siblings for activity and swap for mergable mesh piece if present
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="WallDirection"></param>
    public void WallFormatter(int x, int y, int WallDirection)
    {

       // #region Calculate state
        // generate Neighbour Values for calculation
        var NeighborValues = RCS_Utility.GenerateNeighborDirections((Direction)WallDirection);
        

        bool left = RoomCells[x, y].Walls[NeighborValues.x].gameObject.activeInHierarchy;
        bool right = RoomCells[x, y].Walls[NeighborValues.y].gameObject.activeInHierarchy;
        

        //Container for mesh gathered by next step
        Mesh WallMesh = null;

        int ComparisonSum = (Convert.ToInt32(right) * 2) + (Convert.ToInt32(left) * 1);
        if(RoomCells[x, y].Walls[(int)WallDirection].Wall_State == WallState.Empty)
        {
            ComparisonSum = -1;
        }

      //  #endregion

        // L+ R = 3 / L = 1 / R = 2 / none = 0
        int wallType = ((int)RoomCells[x, y].Walls[WallDirection].Wall_Type);
        WallMeshSet wallMeshSet = EditorController.Instance.WallSets[WallSet];

        switch (ComparisonSum)
        {
                //none
            case 0:
                RoomCells[x, y].Walls[WallDirection].Wall_State = WallState.Straight;
                WallMesh = wallMeshSet.straightWalls[wallType];
                break;

                //right
            case 2:
                RoomCells[x, y].Walls[WallDirection].Wall_State = WallState.RightCorner;
                WallMesh = wallMeshSet.rightCornerWalls[wallType];
                break;

                //left
            case 1:
                RoomCells[x, y].Walls[WallDirection].Wall_State = WallState.LeftCorner;
                WallMesh = wallMeshSet.leftCornerWalls[wallType];
                break;


                //both
            case 3:
                RoomCells[x, y].Walls[WallDirection].Wall_State = WallState.BothCorner;            
                WallMesh = wallMeshSet.bothCornerWalls[wallType];
                break;
        }


        Material WallMat = wallMeshSet.Swatches[WallSkin].material;
        RoomCells[x, y].Walls[WallDirection].UpdateWallMesh(WallMesh, WallMat);
    }


    


    





    public Vector3 FindRoomCentre()
    {
        var centre = new Vector3(0, 0, 0);
        int ValidCells = 0;
        for (int x = 0; x < RoomCells.GetLength(0); x++)
        {
            for (int y = 0; y < RoomCells.GetLength(1); y++)
            {
                if (RoomCells[x, y] != null)
                {
                    centre +=  RoomCells[x, y].transform.position;
                    ValidCells += 1;
                }
            }
        }


            centre /= ValidCells;
        // offset so room is in view
        centre = new Vector3(centre.x, 70, centre.z);

        print(centre);

        return centre;
    }





    public void CornerFormatter(int x, int y)
    {
        var Cell = RoomCells[x, y];

        Cell.InnerCorners[0].mesh = EditorController.Instance.WallSets[WallSet].InnerCorner;
        Cell.InnerCorners[1].mesh = EditorController.Instance.WallSets[WallSet].InnerCorner;
        Cell.InnerCorners[2].mesh = EditorController.Instance.WallSets[WallSet].InnerCorner;
        Cell.InnerCorners[3].mesh = EditorController.Instance.WallSets[WallSet].InnerCorner;

        Cell.InnerCorners[0].gameObject.SetActive(false);
        Cell.InnerCorners[1].gameObject.SetActive(false);
        Cell.InnerCorners[2].gameObject.SetActive(false);
        Cell.InnerCorners[3].gameObject.SetActive(false);

        if (RoomCells[x, y + 1] != null && RoomCells[x - 1, y] != null && RoomCells[x - 1, y + 1] == null)
        {
            Cell.InnerCorners[0].gameObject.SetActive(true);

        }
        if (RoomCells[x, y + 1] != null && RoomCells[x + 1, y] != null && RoomCells[x + 1, y + 1] == null)
        {
            Cell.InnerCorners[1].gameObject.SetActive(true);

        }
        if (RoomCells[x, y - 1] != null && RoomCells[x + 1, y] != null && RoomCells[x + 1, y - 1] == null)
        {
            Cell.InnerCorners[2].gameObject.SetActive(true);

        }
        if (RoomCells[x, y - 1] != null && RoomCells[x - 1, y] != null && RoomCells[x - 1, y - 1] == null)
        {
            Cell.InnerCorners[3].gameObject.SetActive(true);

        }




        /*  old code
         *  for pickings only
        if (RoomCells[x, y + 1] != null && RoomCells[x - 1, y] != null)
        {


            bool corner0 = RoomCells[x, y + 1].Walls[3].gameObject.activeInHierarchy;
            corner0 = RoomCells[x - 1, y].Walls[0].gameObject.activeInHierarchy;

            Cell.InnerCorners[0].gameObject.SetActive(corner0);
        }

        if (RoomCells[x, y + 1] != null && RoomCells[x + 1, y] != null)
        {


            bool corner1 = RoomCells[x, y + 1].Walls[1].gameObject.activeInHierarchy;
            corner1 = RoomCells[x + 1, y].Walls[2].gameObject.activeInHierarchy;

            Cell.InnerCorners[1].gameObject.SetActive(corner1);
        }
        else
        {

        }
        */




    }


   
    // register the cell
    public void RegisterCell(Vector2Int CellID, RoomCell CellRef, bool PerformWallFormat)
    {

        EditorController.Instance.RegisterCell(CellID, CellRef);
        RoomCells[CellID.x,CellID.y] = CellRef;
        Debug.Log("A new room cell has been registered at " + CellID);
        CellCount++;
        if (CellCount > 1 && PerformWallFormat == true)
        {
            //RoomWallCheck();
            SurroundingWallCheck(CellID);
        }


        //testing
       // SwapRoomEditMode(false);


    }

    /// <summary>
    /// Changes wall materials of the room to the select material;
    /// </summary>
    public void EditorHighlight()
    {

       
          //  RoomEditorMouse.Instance.SelectTarget = this.gameObject;
            print("selecting room");

        {
            // does not need to be performed
            /*   for (int x = 0; x < RoomCells.GetLength(0); x++)


                    {
                   for (int y = 0; y < RoomCells.GetLength(1); y++)
                   {
                       if (RoomCells[x, y] != null && x != 0)
                       {
                           RoomCells[x, y].UpdateMaterial(RoomEditorMouse.Instance.SelectionMaterial, RoomEditorMouse.Instance.SelectionMaterial);
                       }

               }
           }*/
        }

        var CameraPos = FindRoomCentre();
        Map_Editor.SelectedRoom = this;
        RoomEditorMouse.Instance.StartCoroutine("CenterCamera",CameraPos);
         RoomSettingsUI.Instance.OpenMenu();
    }
    
    public void EditorSelect()
    {
        
        //Map_Editor.SelectedRoom = this;
        //SwapRoomEditMode(true);
        var CameraPos = FindRoomCentre();
        Map_Editor.SelectedRoom = this;
        RoomEditorMouse.Instance.StartCoroutine("CenterCamera", CameraPos);
        RoomSettingsUI.Instance.OpenMenu();

    }



    public void EditorDeselect()
    {
        print("deselecting room");
        //SwapRoomEditMode(false);
        RoomSettingsUI.Instance.CloseMenu();


    }


    /// <summary>
    /// Puts selected room into its edit, or combined state
    /// </summary>
    /// <param name="Editing"></param>
    public void SwapRoomEditMode(bool Editing)
    {

        switch (Editing)
        {

            case true:
                CombinedRoomMesh.gameObject.SetActive(false);
                CombinedRoomMesh.mesh.Clear();
                EditModeContainer.SetActive(true);

                break;


            case false:
                Destroy(CombinedRoomMesh.mesh);
                CombinedRoomMesh.gameObject.SetActive(true);
                MeshCombiners.GenerateMesh<ConsolidateRoomMesh>(RoomCells, this.transform, CombinedRoomMesh);
                CombinedRoomMesh.gameObject.GetComponent<MeshCollider>().sharedMesh = CombinedRoomMesh.sharedMesh;

               
                Debug.Log("submesh Count = " + CombinedRoomMesh.mesh.subMeshCount);
                var renderer = CombinedRoomMesh.GetComponent<MeshRenderer>();
             var mats =  new Material[2];
                // wall set
                
                mats[0] = Map_Editor.Floors[FloorSet].material;
                mats[1] = Map_Editor.WallSets[WallSet].Swatches[WallSkin].material;

                renderer.materials = mats;
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                // floor set
                //    allMaterials[1] = Map_Editor.Floors[FloorSet];





              //  CombinedRoomMesh.gameObject.GetComponent<MeshRenderer>().material = Map_Editor.WallSets[WallSet].WallMaterial;
               // EditModeContainer.SetActive(false);

                break;




        }



    }



    public void DeregisterCell(Vector2Int CellID)
    {
        EditorController.Instance.DeregisterCell(CellID);
      RoomCells[CellID.x, CellID.y] = null;
        CellCount--;
    Debug.Log("RoomCellDeregistered");
        CameraMapGenerator.instance.EraseCell(CellID);
        if (CellCount > 0)
        {
            SurroundingWallCheck(CellID);
            // RoomWallCheck(); - old method


            //testcode
           // SwapRoomEditMode(false);
        }
        else
        {
            CameraMapGenerator.instance.EraseCell(CellID);
            RoomSettingsUI.Instance.CloseMenu();
            DestroyRoom();

        }
        Debug.Log("RoomCell deleted has + " + RoomCells.GetValue(CellID.x, CellID.y));
    }


    public void DestroyRoom()
    {
        CameraMapGenerator.instance.BlankMap();
        EditorController.Instance.DeRegisterRoom(this);
        Destroy(this.gameObject);
        CameraMapGenerator.instance.RedrawMap();
    }


    public void SetRoomName(string newName)
    {
        this.Name = newName;
        this.gameObject.name = newName;
    }


    public void SurroundingWallCheck(Vector2Int CellID)
    {

        int clampedX = Mathf.Clamp(CellID.x -2, 0, 50);
        int clampedY = Mathf.Clamp(CellID.y -2, 0, 50);

        for (int x = clampedX; x < (clampedX +4); x++)
        {
            for (int y = clampedY; y < (clampedY + 4); y++)
            {
                

                if (RoomCells[x, y] != null)
                {
                    SingleCellWallFormat(x, y);
                }
            }
        }
        



    }

}



/// <summary>
/// Stores Room data
/// </summary>
public class RoomCellData
{






}
