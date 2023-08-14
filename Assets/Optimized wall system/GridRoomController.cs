using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRoomController : MonoBehaviour
{
    public static GridRoomController SelectedRoom = null;

    public string roomName;
     Transform transform;
    [SerializeField] MeshFilter meshFilter;
    private Mesh RoomMesh = new Mesh();

    public Mesh trialMesh;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CreateCell(Vector2Int cellId)
    {
        


    }
}
