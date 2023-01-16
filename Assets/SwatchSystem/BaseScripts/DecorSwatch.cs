using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Decor Swatch", menuName = "DecorSystem/DecorSwatch", order = 0)]
public class DecorSwatch : ScriptableObject
{

    public string Name;
    public Sprite swatchIcon;
    public Mesh[] meshes;
    public Material[] materials;
    


    public void SetData(ObjectSwatch target, string name)
    {
        Name = name;
        swatchIcon = target.Swatch;
        meshes = target.mesh;
        materials = target.material;
    }
}
