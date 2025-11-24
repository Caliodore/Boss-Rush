using UnityEngine;

namespace CaliJR
{ 
    public class Attacking : JRState
    {
        public Attacking() : base("Attacking") { }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            //AddRemoveListeners(C2JR_Brain.BossBrain.OnHitPlayerSuccess, true, (() => C2JR_Brain.BossBrain.ComboCheck()));
            //AddRemoveListeners();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            //AddRemoveListeners(C2JR_Brain.BossBrain.OnHitPlayerSuccess, false, (() => C2JR_Brain.BossBrain.ComboCheck()));
        }
    }
}
