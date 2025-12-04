using UnityEngine;
using System;
using System.Collections;

namespace Cali6
{ 
    public class A6_Recovering : A6_StateBase
    {
        public bool printDebugLogs = true;
        public A6_Recovering() : base("Recovering") { }

        public static A6_Recovering RecoveringInstance;
        public BossAction nextRecovery;

        private void Start()
        {
            nextRecovery = A6_BossActions.Instance.barrierBrokenRecover;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            StartRecovery();
        }

        public override void OnDamagedDuringState()
        {
            base.OnDamagedDuringState();
        }
        
        private void StartRecovery() { nextRecovery.assignedCall.DynamicInvoke(); StartCoroutine(WaitForAnimationEnd()); }
        
        IEnumerator WaitForAnimationEnd() { 
            ToggleAnimatingBool(true);
            while(IsAnimating) { 
                yield return null;
            }
            if(!IsAnimating || CurrentStateDuration > 5f) { 
                if(A6_Brain.Instance.playerInMelee)
                    A6_Brain.Instance.BossSM.RequestStateChange(A6_Brain.Instance.AttackingState);
                else
                    A6_Brain.Instance.BossSM.RequestStateChange(A6_Brain.Instance.PursuingState);
            }
            yield return null;
        }
    }
}
