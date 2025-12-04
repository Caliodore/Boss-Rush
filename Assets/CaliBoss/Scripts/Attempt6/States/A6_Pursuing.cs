using UnityEngine;

namespace Cali6
{ 
    public class A6_Pursuing : A6_StateBase
    {
        public bool printDebugLogs = true;
        public A6_Pursuing() : base("Pursuing") { }

        public static A6_Pursuing PursuingInstance;
        public BossAction MovementChoice;

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            DetermineMovement();
        }

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();
        }

        public override void OnDamagedDuringState()
        {
            base.OnDamagedDuringState();
            A6_Brain.Instance.BossSM.RequestStateChange(A6_Brain.Instance.AttackingState);
        }
        
        public void DetermineMovement() { MovementChoice = A6_BossActions.Instance.RandomActionChoice((int)BossAction.ActionChoice.Movement); }
    }
}
