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

        public override void OnStateEnter()
        {
            base.OnStateEnter();
        }

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();
            if(A6_Brain.Instance.bossCanAttack) { 
                
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
        }
        
        private void DetermineAttackType()
        { 
            if(A6_Brain.Instance.playerInMelee) { 
                //Determine what melee attack to do and set its trigger.
            }
            else { 
                //Determine what ranged melee attack to do and set its trigger.
            }
        }

        private void StartAttack()
        { 
            //StartAnimation
        }

        IEnumerator WaitForAnimationEnd() { 
            ToggleAnimatingBool(true);
            while(IsAnimating) { 
                yield return null;
            }
            if(!IsAnimating)
                A6_StateMachine.Instance.RequestStateChange(A6_Brain.Instance.RecoveringState);
            yield return null;
        }
    }

    public class BossAttack { 
        public enum AttackType { 
            Default = 0,
            Melee = 1,
            Ranged = 2,
            Punishment = 3
        }

        public AttackType AttackChoice;

        public delegate void AttackMethod();
        public AttackMethod assignedCall;

        public BossAttack(int attackInt, AttackMethod attackCall) { AttackChoice = (AttackType)attackInt; assignedCall = attackCall; }
    }
}
