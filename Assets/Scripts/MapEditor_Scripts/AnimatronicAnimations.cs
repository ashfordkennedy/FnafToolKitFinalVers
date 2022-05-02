using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimatronicAnimations", menuName = "ScriptableObjects/AnimatronicAnimations")]
public class AnimatronicAnimations : ScriptableObject
{
    public List<EndoAnimationSet> AnimationSets = new List<EndoAnimationSet>();
}

[System.Serializable]
public class EndoAnimationSet
{
    public string frameName;
    public List<EndoAnimation> animations = new List<EndoAnimation>();
}

[System.Serializable]
public class EndoAnimation
{
    public string animationName;
    public AnimationClip animation;
}