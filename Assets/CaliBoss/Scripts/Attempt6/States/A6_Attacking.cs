using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Cali6
{ 
    public class A6_Attacking : A6_StateBase
    {
        public bool printDebugLogs = true;
        public A6_Attacking() : base("Attacking") { }

        public static A6_Attacking AttackInstance;
        public BossAction ChosenAttack;

        private void Start()
        {

        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            DetermineAttackType();
        }

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();
            if(A6_Brain.Instance.bossCanAttack) { 
                A6_Brain.Instance.OnStartingAttack.Invoke();
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            A6_Brain.Instance.OnAttackEnd.Invoke();
        }
        
        private void DetermineAttackType()
        { 
            A6_Help.DebugPrint(printDebugLogs, "Determining attack type.");
            if(A6_Brain.Instance.playerInMelee) { 
                //Determine what melee attack to do and set its trigger.
                ChosenAttack = A6_BossActions.Instance.RandomActionChoice((int)BossAction.ActionChoice.Melee);
            }
            else { 
                //Determine what ranged melee attack to do and set its trigger.
                ChosenAttack = A6_BossActions.Instance.RandomActionChoice((int)BossAction.ActionChoice.Ranged);
            }
            if(ChosenAttack == null)
                A6_Help.DebugPrint(printDebugLogs, "Screaming and crying.");
        }

        public void StartAttackInState()
        { 
            //StartAnimation
            A6_Help.DebugPrint(printDebugLogs, "State starts attack.");
            ChosenAttack.assignedCall.DynamicInvoke();

            A6_Brain.Instance.BossAnimator.SetTrigger("AttackStart");

            A6_Help.DebugPrint(printDebugLogs, "Post-Invoke");
            StartCoroutine(WaitForAnimationEnd());
        }

        IEnumerator WaitForAnimationEnd() { 
            ToggleAnimatingBool(true);
            while(IsAnimating) { 
                yield return null;
            }
            if(!IsAnimating) { 
                A6_StateMachine.Instance.RequestStateChange(A6_Brain.Instance.RecoveringState);
            }
            yield return null;
        }

        public override void OnDamagedDuringState()
        {
            base.OnDamagedDuringState();
        }
    }

    
}
