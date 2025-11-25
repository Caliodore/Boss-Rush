using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Cali3
{ 
    public class Attacking : T3_State
    {
        public Attacking() : base("Attacking") { }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            T3_Brain.MainBrain.BossSensor.OnExit.AddListener(() => T3_Brain.MainBrain.BossSM.ChangeState("Pursuing"));
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if(T3_Brain.MainBrain.playerInMeleeRange)
            { 
                if(!T3_Brain.MainBrain.FacingPlayerCheck())
                    T3_Brain.MainBrain.TurnTowardsPlayer();
                else if(!T3_Brain.MainBrain.attackRecovering)
                    T3_Brain.MainBrain.SwingSword();
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            T3_Brain.MainBrain.BossSensor.OnExit.RemoveListener(() => T3_Brain.MainBrain.BossSM.ChangeState("Pursuing"));
        }
    }
}
