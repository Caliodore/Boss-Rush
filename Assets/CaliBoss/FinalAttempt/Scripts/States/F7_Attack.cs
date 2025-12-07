using UnityEngine;
using UnityEngine.Events;

namespace Cali7
{ 
    public class F7_Attack : F7_StateBase
    {
        public F7_Attack(string nameIn) : base("Attack") { }


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
