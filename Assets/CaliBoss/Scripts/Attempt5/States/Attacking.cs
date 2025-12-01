using UnityEngine;

namespace CaliBoss
{ 
    public class Attacking : StateBase
    {
        public Attacking() : base("Attacking") { }

        public static Attacking AttInstance;

        public override void OnAwake()
        {
            base.OnAwake();
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}
