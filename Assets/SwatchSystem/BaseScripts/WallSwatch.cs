using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WallSwatch", menuName = "DecorSystem/WallSwatch", order = 3)]
public class WallSwatch : ScriptableObject
{
    public string name;
   [TextArea] public string notes;
    public Material material; 
    public Sprite icon;


}
