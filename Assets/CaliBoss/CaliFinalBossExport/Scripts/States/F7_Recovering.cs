using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Cali7
{ 
    public class F7_Recovering : F7_StateBase
    {
        public bool refsLoaded = false;
        public bool printDebugLogs = true;
        public F7_Recovering() : base("Recovering") { }

        public float recoveryTime = 5f;
        public int recoveryType;

        private void Start()
        {
            if(!F7_RefManager.Instance.gotRefs)
                F7_RefManager.OnRefsLoaded?.AddListener(() => SetReferences());
            else
                SetReferences();
        }

        public void SetReferences() { 
            F7_EventManager.Instance.OnBossTakesDamage?.AddListener(dmgIn => DamageTakenDuringRecovery(dmgIn));
            F7_RefManager.BEVM.OnRecoveryStart?.AddListener(typeInt => RecoveryTimeUpdate(typeInt));
            refsLoaded = true;
        }

        public override void OnStateEnter() { 
            base.OnStateEnter();
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

        private void RecoveryTimeUpdate(int typeInt) {
            recoveryType = typeInt;
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
