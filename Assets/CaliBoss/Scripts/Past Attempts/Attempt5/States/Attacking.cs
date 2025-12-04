using UnityEngine;

namespace CaliBoss
{ 
    public class Attacking : StateBase
    {
        public Attacking() : base("Attacking") { }

        public static Attacking AttInstance;

        public override void OnAwake()
        {
            base.OnAwake();
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            CSR.Instance.BossSM.AddStateChangeListenerOnHit(CSR.Instance.BossSM.GetStateRef<Recovering>());
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            CSR.Instance.BossSM.RemoveOnHitListenersForThisState(CSR.Instance.BossSM.GetStateRef<Recovering>());
        }
    }
}
