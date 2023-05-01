using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Editor_Mousemode_Cell : MonoBehaviour, IMouseMode
{
    [Header("old Build Settings")]

    public GameObject RoomCellPrefab = null;
    public GameObject CellBuildingCursor = null;
    public LayerMask FloorLayerMask;
    public bool? CellOccupied = null;
    
    public Color BuildableColour;
    public Color BlockedColour;
    public Material SelectionMaterial;
    public ParticleSystem BuildParticles;
    private float lastClicked = 3;

    [SerializeField] private Vector2Int currentCell = new Vector2Int();

    public RoomCell[,] CellRegistry { get => EditorController.Instance.CellRegistry;}
    public int MapGridScale { get => EditorController.Instance.MapGridScale; }
    public RoomController SelectedRoom { get => EditorController.Instance.SelectedRoom; }
    public Camera EditorCamera;



    //assign to mouse move event

        /// <summary>
        /// Performs necessary checks for whether a cell can be constructed
        /// </summary>
    public void CellCheck()
    {
        RaycastHit hit;
        Ray ray = EditorCamera.ScreenPointToRay(Input.mousePosition);

        //hit floor
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, FloorLayerMask))
        {
            Transform objectHit = hit.transform;


            var cellInfo = SnapPosition(hit.point, MapGridScale);
            currentCell = cellInfo.ID;

            CellBuildingCursor.transform.position = cellInfo.position;

            //print(cellInfo.ID);
            CellOccupied = (CellRegistry[cellInfo.ID.x, cellInfo.ID.y] != null);
            CellBuildingCursor.SetActive(true);

            var Colour = (CellOccupied.Value) ? BuildableColour : BlockedColour;
            SelectionMaterial.SetColor("_Color", Colour);
        }

        // no hits - I think the fuck not you trick ass bitch!
        else
        {
            CellBuildingCursor.SetActive(false);
            CellOccupied = null;
        }
    }

    public void LeftClick()
    {
        if(CellRegistry[currentCell.x,currentCell.y] == null)
        {
            print("spawning the cell eventually");

            RoomCell NewCell = Instantiate(RoomEditorMouse.Instance.RoomCellPrefab).GetComponent<RoomCell>();
            NewCell.CellID = currentCell;
            var id = currentCell * MapGridScale;
            NewCell.transform.position = new Vector3(id.x,0, id.y);

            if(SelectedRoom == null)
            {
                //generate new room object
                EditorController.Instance.CreateRoom(NewCell);
                NewCell.CellInitialize();
            }

            else
            {
                //assign to current room
                NewCell.CellInitialize();
            }

           

        }
    }

    public void RightClick()
    {
        print("trying to delete " + CellOccupied);
        if (CellOccupied == true)// && lastClicked >= Time.time - 0.05f)
        {
            print("if cleared");
            lastClicked = Time.time;
            EditorController.Instance.CellRegistry[currentCell.x, currentCell.y].EraseCell();
        }

    }




    /// <summary>
    /// Used to generate Grid safe positions for placement of room cells.
    /// </summary>
    /// <param name="input">Current position.</param>
    /// <param name="factor">Grid unit size.</param>
    /// <returns></returns>
    (Vector3Int position,Vector2Int ID) SnapPosition(Vector3 input, float factor = 1f)
    {
        if (factor <= 0f)
        {
            throw new UnityException("factor argument must be above 0");
        }
        
        float x = (Mathf.Round(input.x / factor) * factor);
        float y = 0;
        float z = Mathf.Round(input.z / factor) * factor;
        
        return (Vector3Int.RoundToInt(new Vector3(x, y, z)), new Vector2Int((int)x /10, (int)z / 10));
    }

    public void RegisterModeEvents()
    {
        Editor_Mouse.instance.LeftClick.AddListener(LeftClick);
        Editor_Mouse.instance.MouseUpdate.AddListener(CellCheck);
        Editor_Mouse.instance.RightClick.AddListener(RightClick);
    }
}


public interface IMouseMode
{

    void RegisterModeEvents();



}
