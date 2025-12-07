using UnityEngine;
using UnityEngine.Events;

namespace Cali7
{ 
    public class F7_Idling : F7_StateBase
    {
        public F7_Idling(string nameIn) : base("Idling") { }


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
