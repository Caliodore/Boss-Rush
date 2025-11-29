using UnityEngine;

namespace Cali_4
{ 
    public class C4_AnimManager : MonoBehaviour
    {
        public static C4_AnimManager Instance;
        public static Animator bossAnimator;
        [SerializeField] AnimClip_SO attackAnimations;
        [SerializeField] AnimClip_SO movingAnimations;
        [SerializeField] AnimClip_SO defenseAnimations;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }
    }
}
