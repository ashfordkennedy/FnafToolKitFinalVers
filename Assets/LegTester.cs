using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegTester : MonoBehaviour
{
    
   public MeshFilter meshFilter;
    public MeshFilter[] meshFilters;
    // Start is called before the first frame update
    void Start()
    {
        
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        Matrix4x4 myTransform = transform.worldToLocalMatrix;
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform =myTransform * meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }

        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine,false);
       
        transform.gameObject.SetActive(true);
    }


    // Gather and combine meshes
    void skinned()
    {
        SkinnedMeshRenderer[] meshFilters = GetComponentsInChildren<SkinnedMeshRenderer>();
        var Basemesh = this.GetComponent<SkinnedMeshRenderer>().sharedMesh;
        var BoneData = Basemesh.boneWeights;
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }
        
        meshFilter.sharedMesh = new Mesh();
        meshFilter.sharedMesh.CombineMeshes(combine,true);

        meshFilter.sharedMesh.boneWeights = BoneData;
        transform.gameObject.active = true;
    }
}
