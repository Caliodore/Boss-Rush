using UnityEngine;
using System;
using System.Collections;

namespace Cali6
{ 
    public class A6_Defending : A6_StateBase
    {
        public bool printDebugLogs = true;
        public A6_Defending() : base("Defending") { }

        public static A6_Defending DefendingInstance;
        private BossAction ReactionChoice;

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            DetermineReaction();
        }

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();
        }

        public override void OnDamagedDuringState()
        {
            base.OnDamagedDuringState();
        }

        private void DetermineReaction() { 
            if(A6_Brain.Instance.hitsTakenRecently >= 5) {
                ReactionChoice = A6_BossActions.Instance.RandomActionChoice((int)BossAction.ActionChoice.Defense);
            }
            else { 
                if(A6_BossActions.Instance.bloodBarrier.isReady)
                    ReactionChoice = A6_BossActions.Instance.bloodBarrier;
                else if(A6_BossActions.Instance.aoePunish.isReady)
                    ReactionChoice = A6_BossActions.Instance.aoePunish;
                else
                    A6_Brain.Instance.BossSM.RequestStateChange(A6_Brain.Instance.PursuingState);
            }
        }

        IEnumerator WaitForAnimationEnd() { 
            ToggleAnimatingBool(true);
            while(IsAnimating) { 
                yield return null;
            }
            if(!IsAnimating) { 
                A6_StateMachine.Instance.RequestStateChange(A6_Brain.Instance.RecoveringState);
                //A6_Brain.Instance.OnAttackEnd.Invoke();
            }
            yield return null;
        }

        public void DoReaction() { ReactionChoice.assignedCall.DynamicInvoke(); StartCoroutine(WaitForAnimationEnd()); }
        
    }
}
