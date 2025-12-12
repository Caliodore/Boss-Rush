using UnityEngine;
using UnityEngine.Events;

namespace Cali7
{ 
    public class F7_Defending : F7_StateBase
    {
        public bool printDebugLogs = true;
        public F7_Defending() : base("Defending") { }

        public ActionChoice defenseChoice = null;
        public bool holdingDefense;

        private void Start()
        {
            
        }


        public override void OnStateEnter() { 
            base.OnStateEnter();
            if(defenseChoice == null) { 
                defenseChoice = F7_RefManager.BACM.DecideAction(ActionType.Defense);
            }
            F7_RefManager.BACM.StartAction(defenseChoice.actionName);
        }

        public override void OnStateUpdate() { 
            base.OnStateUpdate();
            if(!holdingDefense) { 
                F7_RefManager.BCNT.StateChangeRequest();
            }
        }

        public override void OnStateExit() { 
            base.OnStateExit();
        }

        public void UpdateDefenseChoice() { }


        public override void StopThisState() { }
    }
}
