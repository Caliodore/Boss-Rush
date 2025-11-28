using UnityEngine;
using Caliodore.States_Phase2;
using Caliodore.States_Phase3;

namespace Caliodore 
{ 
    /// <summary>
    /// Main logic and data center for Phases 2 and 3 with the main boss.
    /// </summary>
    public class HostellusBrain : BossBrain
    {
        public static HostellusBrain HostBrain;

        [Header("References")]
        public BossStateMachine attachedStateMachine;

        private void Awake()
        {
            SetRefs(false);
            attachedStateMachine = HostellusSM.HostSM;
        }
    }
}
