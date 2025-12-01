using UnityEngine;

namespace Cali_4
{ 
    public class ShardSpray : Aggro
    {
        public ShardSpray() : base("ShardSpray") { }

        public override void OnAwake()
        {
            StateEntering.AddListener(() => C4_HostBrain.Buster.TurnToPlayer(false));
            base.OnAwake();
        }
    }
}
