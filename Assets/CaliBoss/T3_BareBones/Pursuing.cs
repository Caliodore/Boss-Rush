using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Cali3
{ 
    public class Pursuing : T3_State
    {
        public Pursuing() : base("Pursuing") { }

        public float timeSpentPursuing;
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            //T3_Brain.MainBrain.BossSensor.OnEnter?.AddListener(() => T3_Brain.MainBrain.BossSM.ChangeState("Attacking"));
            //T3_Brain.MainBrain.OnPlayerKeepingDistance?.AddListener(() => T3_Brain.MainBrain.InterruptStateForAttack());
            //T3_Brain.MainBrain.BossStates.Find(stateOut => stateOut.stateName == "Attacking").OnEnteringStateOneShot?.AddListener(() => T3_Brain.MainBrain.ShardSpray());
            timeSpentPursuing = 0f;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            /*if(!T3_Brain.MainBrain.playerInMeleeRange && (timeSpentPursuing < T3_Brain.MainBrain.timeUntilAttemptRanged))
            {
                T3_Brain.MainBrain.PursuePlayer();
                timeSpentPursuing += Time.deltaTime;
            }
            else if(timeSpentPursuing >= T3_Brain.MainBrain.timeUntilAttemptRanged)
            {
                T3_Brain.MainBrain.OnPlayerKeepingDistance?.Invoke();
                timeSpentPursuing = 0f;
            }*/
            if(T3_Brain.MainBrain.playerInMeleeRange)
                T3_Brain.MainBrain.BossSM.ChangeState("Attacking");
            else
                T3_Brain.MainBrain.PursuePlayer();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            //T3_Brain.MainBrain.BossSensor.OnEnter?.RemoveListener(() => T3_Brain.MainBrain.BossSM.ChangeState("Attacking"));
            //T3_Brain.MainBrain.OnPlayerKeepingDistance?.RemoveListener(() => T3_Brain.MainBrain.ShardSpray());
        }
    }
}
