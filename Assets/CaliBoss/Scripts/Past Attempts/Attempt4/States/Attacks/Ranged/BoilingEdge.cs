using UnityEngine;

namespace Cali_4
{ 
    public class BoilingEdge : Aggro
    {
        public BoilingEdge() : base("BoilingEdge") { }

        public override void OnAwake()
        {
            StateEntering.AddListener(() => C4_HostBrain.Buster.TurnToPlayer(false));
            base.OnAwake();
        }
    }
}
