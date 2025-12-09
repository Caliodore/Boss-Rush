using UnityEngine;
using UnityEngine.Events;

namespace Cali7
{ 
    public class F7_Attack : F7_StateBase
    {
        public F7_Attack() : base("Attacking") { }
        public static F7_Attack AttackInstance;

        public bool wantsToCombo = false;
        public bool wantsToPunish = false;

        ActionChoice attackChoice = null;


        public override void OnStateEnter() { 
            base.OnStateEnter();
            F7_RefManager.BEVM.OnStartAttack?.Invoke();
            if(!wantsToCombo && !wantsToPunish) { 
                attackChoice = F7_RefManager.BACM.DecideAction(ActionType.Attack);
            }
            else if((wantsToCombo && !wantsToPunish) || (wantsToCombo && wantsToPunish)) { 
                attackChoice = F7_RefManager.BACM.GetSpecificAction("ComboFinisher");
            }
            else if(!wantsToCombo && wantsToPunish) { 
                attackChoice = F7_RefManager.BACM.DecideAction(ActionType.Punish);
            }
            if(attackChoice != null)
                F7_RefManager.BACM.StartAction(attackChoice.actionName);
            else
                F7_RefManager.BCNT.StateChangeRequest();
        }

        public override void OnStateUpdate() { 
            base.OnStateUpdate();
        }

        public override void OnStateExit() { 
            base.OnStateExit();
        }


        public override void StopThisState() { }
    }
}
