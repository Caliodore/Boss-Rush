using UnityEngine;

namespace Cali_4
{ 
    [CreateAssetMenu(fileName = "AnimClip_SO", menuName = "Scriptable Objects/AnimClip_SO")]
    public class AnimClip_SO : ScriptableObject
    {
        public string AnimationGroupName;
        [SerializeField] AnimationClip[] assignedClips;
    }
}
