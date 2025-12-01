using UnityEngine;

namespace CaliBoss
{ 
    public class Entering : StateBase
    {
        public Entering() : base("Entering") { }

        public static Entering EnterInstance;

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
