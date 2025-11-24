using UnityEngine;
using UnityEngine.Events;

namespace Caliodore
{
    namespace States_Phase1
    {
        public class Idle : Phase1
        {
            public Idle() : base("Idle") { }

            public bool sentToArena = false;

            public override void OnStateEnter()
            {
                base.OnStateEnter();
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
            }
        } 
    }
}
