using UnityEngine;

namespace Cali_4
{ 
    public class Swipe : Aggro
    {
        public Swipe() : base("Swipe") { }
        public override void OnAwake()
        {
            StateEntering.AddListener(() => C4_HostBrain.Buster.TurnToPlayer(false));
            attackAnimName = "SwipeRegular";
            base.OnAwake();
        }
    }
}
