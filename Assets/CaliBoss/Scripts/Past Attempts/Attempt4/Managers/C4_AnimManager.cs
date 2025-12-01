using System.Security.Cryptography;
using UnityEngine;

namespace Cali_4
{ 
    public class C4_AnimManager : MonoBehaviour
    {
        public static C4_AnimManager Instance;
        public Animator bossAnimator;
        [SerializeField] AnimClip_SO attackAnimations;
        [SerializeField] AnimClip_SO movingAnimations;
        [SerializeField] AnimClip_SO defenseAnimations;

        [Header("AnimStateMachine Scripts")]
        [SerializeField] StateMachineBehaviour stateDeterminerBhvr;
        [SerializeField] StateMachineBehaviour recoveryTypeBhvr;
        [SerializeField] StateMachineBehaviour movementTypeBhvr;
        [SerializeField] StateMachineBehaviour attackTypeBhvr;
        [SerializeField] StateMachineBehaviour defenseTypeBhvr;

        [Header("Testing Vars")]
        public bool printDebugs;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            bossAnimator = C4_HostBrain.Buster.GetComponent<Animator>();
        }

        public void ChooseAnimBhvr()
        { 
            StC4 stateCheck = C4_HostBrain.BossSM.currentState;
            string stateGrouping = stateCheck.GetType().BaseType.Name;
            Helpers.DebugPrint($"Finding grouping based on currentState.BaseType.Name, shown as: {stateGrouping}", printDebugs);

            switch(stateGrouping) 
            { 
                case("Aggro"):
                    bossAnimator.SetInteger("CurrentState", 1);
                    DetermineWhichAttack();
                    break;

                case("Moving"):
                    bossAnimator.SetInteger("CurrentState",2);
                    break;

                case("Defending"):
                    bossAnimator.SetInteger("CurrentState",3);
                    break;
                    
                case("Transition"):
                default:
                    bossAnimator.SetInteger("CurrentState",0);
                    break;
            }
        }

        public void DetermineWhichAttack()
        { 
            Aggro refState = C4_HostBrain.BossSM.currentState as Aggro;
            int attackAnimIndex = attackAnimations.assignedClips.FindIndex(refClip => refClip.name == refState.attackAnimName);
            bossAnimator.SetInteger("AttackType",attackAnimIndex);
        }

        /// <summary>
        /// Called when StateMachine calls its currentState's OnStateEnter method, to make sure all triggers are set proper.
        /// </summary>
        public void UpdateStateTriggers()
        {
            bossAnimator.ResetTrigger("StartMove");
            bossAnimator.ResetTrigger("StartAttack");
            bossAnimator.ResetTrigger("StartWalking");
            bossAnimator.ResetTrigger("StartLeap");
            bossAnimator.ResetTrigger("Transition");
        }

        public void ClearInts()
        { 
            bossAnimator.SetInteger("AttackType",0);
            bossAnimator.SetInteger("CurrentState",0);
        }

        //Animation Methods
        public void DamagerColliderToggle(bool toggleState)
        { 
            C4_HostBrain.BossDamagerCollider.gameObject.SetActive(toggleState);
        }
    }
}
