using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class containing boring formatting methods required for room system
/// </summary>
public class RCS_Utility : MonoBehaviour
{
  

    public static Vector2Int GenerateNeighborDirections(Direction direction)
    {
        int rightNeighbor = 0;
        int leftNeighbor = 0;
        switch ((int)direction)
        {
            // north
            case 0:
                rightNeighbor = 1;
                leftNeighbor = 3;
                break;

            //east
            case 1:
                rightNeighbor = 2;
                leftNeighbor = 0;
                break;

            //South
            case 2:
                rightNeighbor = 3;
                leftNeighbor = 1;
                break;


            //West
            case 3:
                rightNeighbor = 0;
                leftNeighbor = 2;
                break;
        }

       return new Vector2Int(leftNeighbor, rightNeighbor);

    }



}



public struct CellOffsets{

   public Vector2Int originalPosition;
   public Vector2Int offsetPosition;
    public Direction originalDirection;
    public Direction opposingDirection;


    public CellOffsets(Vector2Int cellPosition,Direction direction)
    {
        this.originalPosition = cellPosition;
        this.originalDirection = direction;

        this.offsetPosition = new Vector2Int();
        this.opposingDirection = (0);


        var offsets = GenerateOffsets(direction, cellPosition);
        this.offsetPosition = new Vector2Int(offsets.x, offsets.y);
        this.opposingDirection = (Direction)offsets.z;
    }


    private Vector3Int GenerateOffsets(Direction direction, Vector2Int cellPosition)
    {
         
        Vector2Int Offset = new Vector2Int();
   
        int OffsetDirection = 0;
        // set direction values    
        switch (direction)
        {

            //left
            case Direction.West:
                Offset.x = cellPosition.x - 1;
                Offset.y = cellPosition.y;
                OffsetDirection = 1;
                break;

            //up
            case Direction.North:
                Offset.x = cellPosition.x;
                Offset.y = cellPosition.y + 1;
                OffsetDirection = 2;
                break;

            //Right
            case Direction.East:
                Offset.x = cellPosition.x + 1;
                Offset.y = cellPosition.y;
                OffsetDirection = 3;
                break;

            case Direction.South:
                Offset.x = cellPosition.x;
                Offset.y = cellPosition.y - 1;
                OffsetDirection = 0;
                break;
        }

        Vector3Int value = new Vector3Int(Offset.x,Offset.y,OffsetDirection);

        return value;


    }
    }