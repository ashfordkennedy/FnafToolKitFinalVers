using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AnimatronicAnimationSet", menuName = "DecorSystem/Animatronic Animation Set", order = 0)]
public class AnimatronicAnimationSet : ScriptableObject
{
    public string frameName;
    public List<EndoAnimation> animations = new List<EndoAnimation>();
    public List<EndoAnimation> Jumpscares = new List<EndoAnimation>();
    [TextArea]
    public string notes;
}
