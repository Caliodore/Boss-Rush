using UnityEngine;

namespace Cali6
{ 
    public class A6_Idling : A6_StateBase
    {
        public A6_Idling() : base("Idling") { }

        public static A6_Idling IdlingInstance;

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();
            if(CurrentStateDuration >= 6f) {
                LeaveState();
            }
        }

        public override void OnDamagedDuringState()
        {
            base.OnDamagedDuringState();
            LeaveState();
        }

        private void LeaveState() { 
            A6_Brain.Instance.BossSM.RequestStateChange(A6_Brain.Instance.AttackingState);
        }
        
    }
}
