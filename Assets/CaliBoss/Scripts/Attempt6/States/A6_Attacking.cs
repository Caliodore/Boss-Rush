using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Cali6
{ 
    public class A6_Attacking : A6_StateBase
    {
        public A6_Attacking() : base("Attacking") { }

        public static A6_Attacking AttackInstance;
        public BossAction ChosenAttack;
        private object[] refList = new object[0];

        private void Start()
        {
            refList = new object[] { A6_BossActions.Instance };
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
        }
        
        private void DetermineAttackType()
        { 
            print("Determining attack type.");
            if(A6_Brain.Instance.playerInMelee) { 
                //Determine what melee attack to do and set its trigger.
                ChosenAttack = A6_BossActions.Instance.RandomActionChoice((int)BossAction.ActionChoice.Melee);
            }
            else { 
                //Determine what ranged melee attack to do and set its trigger.
                ChosenAttack = A6_BossActions.Instance.RandomActionChoice((int)BossAction.ActionChoice.Ranged);
            }
            if(ChosenAttack == null)
                print("Screaming and crying.");
        }

        public void StartAttackInState()
        { 
            //StartAnimation
            print("State starts attack.");
            ChosenAttack.assignedCall.DynamicInvoke();
            A6_Brain.Instance.BossAnimator.SetTrigger("AttackStart");
            print("Post-Invoke");
            StartCoroutine(WaitForAnimationEnd());
        }

        IEnumerator WaitForAnimationEnd() { 
            ToggleAnimatingBool(true);
            while(IsAnimating) { 
                yield return null;
            }
            if(!IsAnimating)
                A6_StateMachine.Instance.RequestStateChange(A6_Brain.Instance.RecoveringState);
            A6_Brain.Instance.OnAttackEnd.Invoke();
            yield return null;
        }

        public override void OnDamagedDuringState()
        {
            base.OnDamagedDuringState();
        }
    }

    
}
