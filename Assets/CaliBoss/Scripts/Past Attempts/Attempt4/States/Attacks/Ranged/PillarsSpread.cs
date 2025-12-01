using UnityEngine;

namespace Cali_4
{ 
    public class PillarsSpread : Aggro
    {
        public PillarsSpread() : base("PillarsSpread") { }

        public override void OnAwake()
        {
            StateEntering.AddListener(() => C4_HostBrain.Buster.TurnToPlayer(false));
            base.OnAwake();
        }
    }
}
