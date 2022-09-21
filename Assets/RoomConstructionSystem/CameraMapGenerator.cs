using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMapGenerator : MonoBehaviour
{
    public static CameraMapGenerator instance;
    public Texture2D MapTexture;
    public Texture2D BlankTexture;
    int mapBorderSize = 31;

    public void Awake()
    {
        instance = this;
        BlankMap();
    }


    public void BlankMap()
    {
         Color[] colors = new Color[1];

        colors[0] = Color.clear;
        MapTexture.SetPixels(BlankTexture.GetPixels());
        MapTexture.Apply();
    }

    public void EraseRoom(RoomController roomController)
    {
        /*    
         for (int x = 0; x < roomController.RoomCells.GetLength(0); x++)
         {
             for (int y = 0; y < roomController.RoomCells.GetLength(1); y++)
             {
                 if (roomController.RoomCells[x, y] != null)
                 {
                     EraseCell(new Vector2Int(x, y));
                 }
             }

         }
         */
        BlankMap();
        RedrawMap();



    }

    public void RedrawMap()
    {
       var mapCells = EditorController.Instance.CellRegistry;
        for (int x = 0; x < mapCells.GetLength(0); x++)
        {
            for (int y = 0; y < mapCells.GetLength(1); y++)
            {
                var cellId = new Vector2Int(x, y);
                if (mapCells[x, y] != null)
                {
                    DrawCell(cellId, mapCells[x,y]);
                   
                }
                /*
                else
                {
                    EraseCell(cellId);
                }
                */
            }
            
        }
    }


    public void EraseCell(Vector2Int CellID)
    {
        Vector2Int CellCenter = ((CellID * 10) + new Vector2Int(mapBorderSize, mapBorderSize));

        Vector2Int StartPixel = new Vector2Int(CellCenter.x - 5, CellCenter.y - 5);

        Color[] blank = BlankTexture.GetPixels(StartPixel.x, StartPixel.y, 11, 11);
        MapTexture.SetPixels(StartPixel.x, StartPixel.y, 10, 10, blank);
        MapTexture.Apply();
    }


    public void DrawCell(Vector2Int CellID,RoomCell roomCell)
    {
        Vector2Int StartPixel = new Vector2Int();
        for (int i = 0; i < roomCell.Walls.Length; i++)
        {
            Color line = (roomCell.Walls[i].gameObject.activeInHierarchy) ? Color.white : Color.clear;
            Color[] lineColor = { line, line, line, line, line, line, line, line, line, line, line, line, line, line, line, line, line, line, line, line };
            

           

            Vector2Int CellCenter = ((CellID * 10) + new Vector2Int(mapBorderSize,mapBorderSize));

            
            switch (i)
            {

                //north wall
                case 0:
                   StartPixel  = new Vector2Int(CellCenter.x - 5, CellCenter.y + 4);
                    MapTexture.SetPixels(StartPixel.x, StartPixel.y, 10, 2, lineColor);
                    break;

                // east wall
                case 1:
                    StartPixel = new Vector2Int(CellCenter.x + 4, CellCenter.y - 5);
                    MapTexture.SetPixels(StartPixel.x, StartPixel.y, 2, 10, lineColor);
                    break;

                // South wall
                case 2:
                    StartPixel = new Vector2Int(CellCenter.x - 5, CellCenter.y - 5);
                    MapTexture.SetPixels(StartPixel.x, StartPixel.y, 10, 2, lineColor);
                    break;

                // west wall
                case 3:
                    StartPixel = new Vector2Int(CellCenter.x - 5, CellCenter.y - 5);
                    MapTexture.SetPixels(StartPixel.x, StartPixel.y, 2, 10, lineColor);
                    break;
            }   
        }
        MapTexture.Apply();
    }

}
