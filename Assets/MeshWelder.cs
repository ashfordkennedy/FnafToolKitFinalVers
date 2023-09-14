using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// code generated through chatGPT as an example
/// </summary>
public class MeshWelder : MonoBehaviour
{
    public float weldThreshold = 0.001f; // Adjust this value based on your requirements.
    /*
    private void Start()
    {
        // Access the MeshFilter component
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;

        // Weld vertices and preserve UVs
        WeldVertices(mesh);

        // Recalculate normals and bounds for correct lighting and rendering
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
    */
    /*
    private void WeldVertices(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = mesh.uv;
        int[] triangles = mesh.triangles;

        // Create a dictionary to store unique vertices and their corresponding index
        Dictionary<Vector3, int> uniqueVertices = new Dictionary<Vector3, int>();

        // Loop through all vertices and check for duplicates
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            if (!uniqueVertices.ContainsKey(vertex))
            {
                // Add the vertex to the dictionary if it doesn't exist
                uniqueVertices.Add(vertex, i);
            }
            else
            {
                // If a similar vertex already exists within the threshold distance, use the existing index
                int existingIndex = uniqueVertices[vertex];

                // Update UVs of the existing vertex by averaging with the current UV
                Vector2 avgUV = (uvs[i] + uvs[existingIndex]) * 0.5f;
                uvs[existingIndex] = avgUV;

                // Replace vertex index in triangles with the existing index
                triangles = ReplaceVertexIndex(triangles, i, existingIndex);
            }
        }

        // Apply the welded vertices and UVs to the mesh
        Vector2[] weldedVertices = new Vector2[uniqueVertices.Count];
        Vector2[] weldedUVs = new Vector2[uniqueVertices.Count];
        uniqueVertices.Keys.CopyTo(weldedVertices, 0);
       // uniqueVertices.Values.CopyTo(weldedUVs, 0);
        mesh.vertices = weldedVertices;
        mesh.uv = weldedUVs;
        mesh.triangles = triangles;
    }
    */
    private int[] ReplaceVertexIndex(int[] triangles, int oldIndex, int newIndex)
    {
        for (int i = 0; i < triangles.Length; i++)
        {
            if (triangles[i] == oldIndex)
            {
                triangles[i] = newIndex;
            }
        }
        return triangles;
    }
}