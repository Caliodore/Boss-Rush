using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cali_4
{ 
    [CreateAssetMenu(fileName = "AnimClip_SO", menuName = "Scriptable Objects/AnimClip_SO")]
    public class AnimClip_SO : ScriptableObject
    {
        [SerializeField] public string AnimationGroupName;
        [SerializeField] public List<AnimationClip> assignedClips;

        public AnimationClip FindClip(string nameRef)
        {
            return assignedClips.Find(clipIn => clipIn.name == nameRef);
        }
    }
}
