using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

namespace CaliJR
{ 
    public class Pursue : JRState
    {
        public Pursue() : base("Pursue") { }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            //AddRemoveListeners(C2JR_Brain.BossBrain.OnDamagedByPlayer, true, (() => C2JR_StateMachine.BossSM.ChangeState("Attacking")));
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if(C2JR_Brain.BossBrain.playerInMelee)
            { 
                C2JR_Brain.BossSM.ChangeState("Attacking");  
            }
            else if(!C2JR_Brain.BossBrain.pursuingPlayer)
            { 
                C2JR_Brain.BossBrain.PursuePlayer();
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            //AddRemoveListeners(C2JR_Brain.BossBrain.OnDamagedByPlayer, false, (() => C2JR_StateMachine.BossSM.ChangeState("Attacking")));
        }
    }
}
