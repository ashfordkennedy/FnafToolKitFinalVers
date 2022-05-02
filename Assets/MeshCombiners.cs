using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class MeshCombiners
{

    public static List<MeshFilter> GenerateRoomMeshList<T>()
    {
        var MeshList = new List<MeshFilter>();

        return MeshList;
    }



    public static void GenerateMesh<T>(RoomCell[,] RoomCells, Transform transform, MeshFilter Filter)
    {
        // Fill list with activated wall objects
        List<MeshFilter> MeshFilters = new List<MeshFilter>();
        List<MeshFilter> FloorMeshFilters = new List<MeshFilter>();
        for (int x = 0; x < RoomCells.GetLength(0); x++)
        {
            for (int y = 0; y < RoomCells.GetLength(1); y++)
            {

                if (RoomCells[x, y] != null)
                {
                    
                    MeshFilters.AddRange(RoomCells[x, y].CompileWallMeshData());
                    FloorMeshFilters.Add(RoomCells[x, y].FloorFilter);
                }
            }
        }
        Debug.Log("Total collected filters " + MeshFilters.Count);

        // take combined list of walls and prepare for merge
        MeshFilter[] usableFilters = MeshFilters.ToArray();
        CombineInstance[] combine = new CombineInstance[usableFilters.Length];


        Matrix4x4 myTransform = transform.worldToLocalMatrix;
        int i = 0;
        while (i < usableFilters.Length)
        {
            
            combine[i].mesh = usableFilters[i].sharedMesh;
            combine[i].transform = myTransform * usableFilters[i].transform.localToWorldMatrix;
            //  MeshFilters[i].gameObject.SetActive(false);
            
            i++;
        }


        CombineInstance[] FloorCombine = new CombineInstance[FloorMeshFilters.Count];
       Debug.Log(FloorMeshFilters.Count + " floor filters, combine = " + FloorCombine.Length);
        int o = 0;
        while (o < FloorMeshFilters.Count)
        {


            FloorCombine[o].mesh = FloorMeshFilters[o].sharedMesh;
            FloorCombine[o].transform = myTransform * FloorMeshFilters[o].transform.localToWorldMatrix;
            
            o++;
        }
        
        

        //generate wall meshes
        Filter.mesh = new Mesh();
        Filter.mesh.CombineMeshes(combine,true,true);
       
        Mesh Wallmeshtemp = Filter.mesh;
        CombineInstance[] RoomCombine = new CombineInstance[2];

        //Generate and store floor mesh
        Filter.mesh = new Mesh();
        Filter.mesh.CombineMeshes(FloorCombine, true,true);

        Mesh Floormeshtemp = Filter.mesh;


        Debug.Log(Floormeshtemp.vertexCount + "vertices in floor mesh. " + Wallmeshtemp.vertexCount + " vertices on wall mesh");

        // test combine
        // WeldVertices(Wallmeshtemp,0.1f);



        RoomCombine[0].mesh = WeldVertices(Floormeshtemp,0.03f);
       // RoomCombine[0].mesh = Floormeshtemp;       
        RoomCombine[1].mesh = Wallmeshtemp;
        // RoomCombine[0].subMeshIndex = 1;

        
       Filter.mesh = new Mesh();
       Filter.mesh.CombineMeshes(RoomCombine,false,false);

        
       Debug.Log("New mesh has " + Filter.mesh.vertexCount);
      

        // 2D array search. walls active and then columns. add them to mesh filters for processing.


        

                
    }


    // from https://answers.unity.com/questions/1382854/welding-vertices-at-runtime.html
    public static Mesh WeldVertices(Mesh aMesh, float aMaxDelta = 0.001f)
    {
        var verts = aMesh.vertices;
        var normals = aMesh.normals;
        var uvs = aMesh.uv;
        List<int> newVerts = new List<int>();
        int[] map = new int[verts.Length];
        // create mapping and filter duplicates.
        for (int i = 0; i < verts.Length; i++)
        {
            var p = verts[i];
            var n = normals[i];
            var uv = uvs[i];
            bool duplicate = false;
            for (int i2 = 0; i2 < newVerts.Count; i2++)
            {
                int a = newVerts[i2];
                if (
                    (verts[a] - p).sqrMagnitude <= aMaxDelta && // compare position
                    Vector3.Angle(normals[a], n) <= aMaxDelta && // compare normal
                    (uvs[a] - uv).sqrMagnitude <= aMaxDelta // compare first uv coordinate
                    )
                {
                    map[i] = i2;
                    duplicate = true;
                    break;
                }
            }
            if (!duplicate)
            {
                map[i] = newVerts.Count;
                newVerts.Add(i);
            }
        }
        // create new vertices
        var verts2 = new Vector3[newVerts.Count];
        var normals2 = new Vector3[newVerts.Count];
        var uvs2 = new Vector2[newVerts.Count];
        for (int i = 0; i < newVerts.Count; i++)
        {
            int a = newVerts[i];
            verts2[i] = verts[a];
            normals2[i] = normals[a];
            uvs2[i] = uvs[a];
        }
        // map the triangle to the new vertices
        var tris = aMesh.triangles;
        for (int i = 0; i < tris.Length; i++)
        {
            tris[i] = map[tris[i]];
        }
        aMesh.vertices = verts2;
        aMesh.normals = normals2;
        aMesh.uv = uvs2;
        aMesh.triangles = tris;

        return aMesh;
    }





















    // https://answers.unity.com/questions/228841/dynamically-combine-verticies-that-share-the-same.html
    // by boxiness  
    public static Mesh AutoWeld(Mesh mesh, float threshold, float bucketStep)
    {
        Vector3[] oldVertices = mesh.vertices;
        Vector3[] newVertices = new Vector3[oldVertices.Length];
        int[] old2new = new int[oldVertices.Length];
        int newSize = 0;

        // Find AABB
        Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        for (int i = 0; i < oldVertices.Length; i++)
        {
            if (oldVertices[i].x < min.x) min.x = oldVertices[i].x;
            if (oldVertices[i].y < min.y) min.y = oldVertices[i].y;
            if (oldVertices[i].z < min.z) min.z = oldVertices[i].z;
            if (oldVertices[i].x > max.x) max.x = oldVertices[i].x;
            if (oldVertices[i].y > max.y) max.y = oldVertices[i].y;
            if (oldVertices[i].z > max.z) max.z = oldVertices[i].z;
        }

        // Make cubic buckets, each with dimensions "bucketStep"
        int bucketSizeX = Mathf.FloorToInt((max.x - min.x) / bucketStep) + 1;
        int bucketSizeY = Mathf.FloorToInt((max.y - min.y) / bucketStep) + 1;
        int bucketSizeZ = Mathf.FloorToInt((max.z - min.z) / bucketStep) + 1;
        List<int>[,,] buckets = new List<int>[bucketSizeX, bucketSizeY, bucketSizeZ];

        // Make new vertices
        for (int i = 0; i < oldVertices.Length; i++)
        {
            // Determine which bucket it belongs to
            int x = Mathf.FloorToInt((oldVertices[i].x - min.x) / bucketStep);
            int y = Mathf.FloorToInt((oldVertices[i].y - min.y) / bucketStep);
            int z = Mathf.FloorToInt((oldVertices[i].z - min.z) / bucketStep);

            // Check to see if it's already been added
            if (buckets[x, y, z] == null)
                buckets[x, y, z] = new List<int>(); // Make buckets lazily

            for (int j = 0; j < buckets[x, y, z].Count; j++)
            {
                Vector3 to = newVertices[buckets[x, y, z][j]] - oldVertices[i];
                if (Vector3.SqrMagnitude(to) < threshold)
                {
                    old2new[i] = buckets[x, y, z][j];
                    goto skip; // Skip to next old vertex if this one is already there
                }
            }

            // Add new vertex
            newVertices[newSize] = oldVertices[i];
            buckets[x, y, z].Add(newSize);
            old2new[i] = newSize;
            newSize++;

        skip:;
        }

        // Make new triangles
        int[] oldTris = mesh.triangles;
        int[] newTris = new int[oldTris.Length];
        for (int i = 0; i < oldTris.Length; i++)
        {
            newTris[i] = old2new[oldTris[i]];
        }

        Vector3[] finalVertices = new Vector3[newSize];
        for (int i = 0; i < newSize; i++)
            finalVertices[i] = newVertices[i];

        mesh.Clear();
        mesh.vertices = finalVertices;
        mesh.triangles = newTris;
        mesh.RecalculateNormals();
 
        mesh.Optimize();
        return mesh;
    }


}



public struct ConsolidateRoomMesh
{
    public Mesh RoomMesh;
    public List<Material> RoomMaterials;

    public ConsolidateRoomMesh(Mesh roomMesh, List<Material> roomMats)
    {
        this.RoomMesh = roomMesh;
        this.RoomMaterials = roomMats;

    }


}
