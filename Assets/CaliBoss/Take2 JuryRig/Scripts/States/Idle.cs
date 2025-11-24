using Caliodore;
using UnityEngine;
using UnityEngine.Events;

namespace CaliJR
{ 
    public class Idle : JRState
    {
        public UnityAction ChangeToAttacking;
        public Idle() : base("Idle") { }

        private void Awake()
        {
            if(ChangeToAttacking == null)
                ChangeToAttacking = new UnityAction(() => C2JR_Brain.BossSM.ChangeState("Attacking"));
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            AddRemoveListeners(C2JR_Brain.BossBrain.OnDamagedByPlayer, true, (() => C2JR_Brain.BossSM.ChangeState("Attacking")));
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if(currentStateDuration >= 2.5f)
            {
                C2JR_Brain.BossSM.ChangeState("Attacking");
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            AddRemoveListeners(C2JR_Brain.BossBrain.OnDamagedByPlayer, false, (() => C2JR_Brain.BossSM.ChangeState("Attacking")));
        }
    }
}
