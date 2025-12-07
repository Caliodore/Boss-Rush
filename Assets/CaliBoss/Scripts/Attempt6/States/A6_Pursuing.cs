using Cali3;
using DG.Tweening;
using UnityEngine;

namespace Cali6
{ 
    public class A6_Pursuing : A6_StateBase
    {
        public bool printDebugLogs = true;
        public A6_Pursuing() : base("Pursuing") { }

        public static A6_Pursuing PursuingInstance;
        public BossAction MovementChoice;
        public bool isDoingSpecialMove;

        public override void OnStateEnter()
        {
            isDoingSpecialMove = false;
            base.OnStateEnter();
            if((A6_Brain.Instance.currentHealth == A6_Brain.Instance.maxHealth * 0.5f) || (A6_Brain.Instance.isEnraged)) { 
                DetermineMovement();
                isDoingSpecialMove = true;
            }
        }

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();
            if(A6_Brain.Instance.playerInMelee)
                A6_Brain.Instance.BossSM.RequestStateChange(A6_Brain.Instance.AttackingState);
            if (!isDoingSpecialMove) { 
                A6_Brain.Instance.gameObject.transform.LookAt(A6_Brain.Instance.playerTransform);
                //Vector3 directionVector = (A6_Brain.Instance.playerTransform.position - A6_Brain.Instance.transform.position).normalized;
                A6_Brain.Instance.BossNMAgent.SetDestination(A6_Brain.Instance.playerTransform.position);
            }

        }

        public override void OnDamagedDuringState()
        {
            base.OnDamagedDuringState();
            A6_Brain.Instance.BossSM.RequestStateChange(A6_Brain.Instance.AttackingState);
        }
        
        public void DetermineMovement() { MovementChoice = A6_BossActions.Instance.RandomActionChoice((int)BossAction.ActionChoice.Movement); }
    }
}
