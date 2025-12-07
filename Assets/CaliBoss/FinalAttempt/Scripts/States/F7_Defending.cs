using UnityEngine;
using UnityEngine.Events;

namespace Cali7
{ 
    public class F7_Defending : F7_StateBase
    {
        public F7_Defending(string nameIn) : base("Defending") { }

        public ActionChoice defenseChoice = null;

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
        }

        public override void OnStateExit() { 
            base.OnStateExit();
        }

        public void UpdateDefenseChoice() { }


        public override void StopThisState() { }
    }
}
