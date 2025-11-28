using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Cali3
{ 
    public class Idle : T3_State
    {
        public Idle() : base("Idle") { }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            //T3_Brain.MainBrain.BossSM.TestingBrainRef();
            if(currentStateDuration > 2f)
                T3_Brain.MainBrain.BossSM.ChangeState("Pursuing");
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}
