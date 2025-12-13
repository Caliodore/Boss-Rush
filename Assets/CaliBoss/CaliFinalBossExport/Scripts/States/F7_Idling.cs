using UnityEngine;
using UnityEngine.Events;

namespace Cali7
{ 
    public class F7_Idling : F7_StateBase
    {
        public bool printDebugLogs = true;
        public F7_Idling() : base("Idling") { }


        public override void OnStateEnter() { 
            base.OnStateEnter();
        }

        public override void OnStateUpdate() { 
            base.OnStateUpdate();
            if(F7_RefManager.BCNT.encounterStarted && currentStateDuration > 1f) { 
                F7_RefManager.BCNT.StateChangeRequest();
            }
        }

        public override void OnStateExit() { 
            base.OnStateExit();
        }


        public override void StopThisState() { }
    }
}
