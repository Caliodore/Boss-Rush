using UnityEngine;

namespace CaliJR
{ 
    public class Idle : JRState
    {
        public Idle() : base("Idle") { }

        private void Awake()
        {
            
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if(currentStateDuration > 2.5f)
            { 
                C2JR_StateMachine.BossSM.ChangeState("Pursue");
            }
        }
    }
}
