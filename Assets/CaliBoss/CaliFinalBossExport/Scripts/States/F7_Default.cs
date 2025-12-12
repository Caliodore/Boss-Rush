using UnityEngine;
using UnityEngine.Events;

namespace Cali7
{ 
    public abstract class F7_Default : F7_StateBase
    {
        public bool printDebugLogs = true;
        public F7_Default() : base("Default") { }


        public override void OnStateEnter() { 
            base.OnStateEnter();
        }

        public override void OnStateUpdate() { 
            base.OnStateUpdate();
        }

        public override void OnStateExit() { 
            base.OnStateExit();
        }


        public override void StopThisState() { }
    }
}
