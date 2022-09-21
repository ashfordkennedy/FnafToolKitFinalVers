using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// WallState Describes the mesh variant to be used by the formatter so that its edges conform with surrounding walls and can easily be merged.
/// </summary>
public enum WallState {Empty,Straight,LeftCorner,RightCorner,BothCorner }

/// <summary>
/// WallType describes the wall variant to use when generating.
/// A variant is characterised by the walls purpose, (blank, door etc.) not how it should relate to surrounding walls  (See WallState)
/// </summary>
public enum WallType {Blank,Window,Door,Vent, DoorLeft,DoorCenter,DoorRight }
public class WallComponent : MonoBehaviour
{
    public Direction direction;
    public RoomCell Room_Cell;
    public MeshFilter WallMesh;
    public MeshRenderer Renderer;
    public MeshCollider Collider;
    public WallType Wall_Type = WallType.Blank;
    public WallState Wall_State = WallState.BothCorner;




    public void UpdateWallMesh(Mesh NewMesh, Material Mat)
    {
        if (NewMesh != null)
        {
          
            WallMesh.mesh = NewMesh;
            Renderer.material = Mat;
            Collider.sharedMesh = NewMesh;
            Collider.enabled = true;
            
        }
        else
        {
            Wall_State = WallState.Empty;
            WallMesh.mesh = null;
            Collider.enabled = false;
          

        }
    }

    public void SetWallType(WallType NewWalltype = WallType.Blank)
    {
        Wall_Type = NewWalltype;
        Room_Cell.Room_Ctrl.SingleCellWallFormat(Room_Cell.CellID.x, Room_Cell.CellID.y);


       var registry = EditorController.Instance.CellRegistry;
        var cellLimit = new Vector2Int(EditorController.Instance.CellRegistry.GetUpperBound(0), EditorController.Instance.CellRegistry.GetUpperBound(1));
        print("cell limit calculated as" + cellLimit);
        Vector2Int newID = Room_Cell.CellID;
        print("NewID is " + newID);
        Direction targetDirection = Direction.North;
        switch (direction)
        {
            case Direction.North:
                newID.y += 1;
                targetDirection = Direction.South;
                break;
            case Direction.East:
                newID.x += 1;
                targetDirection = Direction.West;
                break;

            case Direction.South:
                newID.y -= 1;
                targetDirection = Direction.North;
                break;
            case Direction.West:
                newID.x -= 1;
                targetDirection = Direction.East;
                break;
        }





        if(newID.x >1 && newID.y >1 && newID.x < cellLimit.x && newID.y < cellLimit.y)
        {
            print("can run wall behaviour");
            if (EditorController.Instance.CellRegistry[newID.x, newID.y] != null) {
                RoomCell adjacentCell = EditorController.Instance.CellRegistry[newID.x, newID.y];
                print(adjacentCell.CellID);
                adjacentCell.Walls[(int)targetDirection].RecieveWallType(Wall_Type);
            }
        }
        else
        {
            print("cant run wall behaviour");
        }



        /*
        switch (NewWalltype)
        {
            case WallType.Blank:
                break;
        }
        */
        Vector2Int CellID = Room_Cell.CellID;             
    }



    public void RecieveWallType(WallType newWallType = WallType.Blank)
    {
        print("recieving wall type");
        WallType wallSetting = newWallType;

        switch (newWallType)
        {
            case WallType.DoorLeft:
                wallSetting = WallType.DoorRight;
                break;

            case WallType.DoorRight:
                wallSetting = WallType.DoorLeft;
                break;

        }
        Wall_Type = wallSetting;

        // Room_Cell.EraseCell();
        RoomEditorMouse.Instance.CellBuildingCursor.SetActive(true);
        RoomEditorMouse.Instance.CellBuildingCursor.transform.position = Room_Cell.transform.position;

        Room_Cell.Room_Ctrl.SingleCellWallFormat(Room_Cell.CellID.x, Room_Cell.CellID.y);

    }
}
