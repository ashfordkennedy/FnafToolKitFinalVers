using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Decor Swatch", menuName = "DecorSystem/DecorSwatch", order = 0)]
public class DecorSwatch : ScriptableObject
{

    public string Name;
    public Sprite swatchIcon;
    public List<Mesh> meshes;
    public List<Material> materials;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
