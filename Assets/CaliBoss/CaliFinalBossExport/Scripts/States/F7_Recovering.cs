using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Cali7
{ 
    public class F7_Recovering : F7_StateBase
    {
        public F7_Recovering(string nameIn) : base("Recovering") { }

        public float recoveryTime;
        public int recoveryType;

        private void Start()
        {
            F7_EventManager.Instance.OnBossTakesDamage?.AddListener(dmgIn => DamageTakenDuringRecovery(dmgIn));
        }

        public override void OnStateEnter() { 
            base.OnStateEnter();
            RecoveryTimeUpdate();
        }

        public override void OnStateUpdate() { 
            base.OnStateUpdate();
            if(currentStateDuration >= recoveryTime)
                F7_RefManager.BCNT.StateChangeRequest();
        }

        public override void OnStateExit() { 
            base.OnStateExit();
        }


        public override void StopThisState() { }

        private void RecoveryTimeUpdate() {
            switch(recoveryType) { 
                case(1):
                default:
                    recoveryTime = F7_RefManager.BPSO.brokenBarrierRecoveryTime;
                    break;
                    
                case(2):
                    recoveryTime = F7_RefManager.BPSO.reelingBackRecoveryTime;
                    break;
                    
                case(3):
                    recoveryTime = F7_RefManager.BPSO.enragedExitRecoveryTime;
                    break;

            }
        }

        public void DamageTakenDuringRecovery(int dmgIn) { if(F7_RefManager.BSTM.CurrentState == this) recoveryTime /= 2;}
    }
}
