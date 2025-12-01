using UnityEngine;

namespace Cali_4
{ 
    public class EntryWait : Transition
    {
        public EntryWait() : base("EntryWait") { }

        public override void OnAwake()
        { 
            transitionDuration = C4_HostBrain.Buster.entranceWaitDuration;    
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
        }
    }
}
