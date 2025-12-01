using UnityEngine;

namespace Cali_4
{ 
    public class Slam : Aggro
    {
        public Slam() : base("Slam") { }

        public override void OnAwake()
        {
            StateEntering.AddListener(() => C4_HostBrain.Buster.TurnToPlayer(false));
            attackAnimName = "SlamRegular";
            base.OnAwake();
        }

    }
}
