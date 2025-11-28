using UnityEngine;
using Caliodore.States_Phase1;
using Caliodore.States_Phase2;
using Caliodore.States_Phase3;

namespace Caliodore
{ 
    public class HostellusSM : BossStateMachine
    {
        public static HostellusSM HostSM;
        public BossBrain attachedBrain;

        private void Awake()
        {
            attachedBrain = HostellusBrain.HostBrain;
        }

    }
}
