using UnityEngine;

namespace Caliodore
{
    /// <summary>
    /// Framework for state classes for third boss phase.<br/>
    /// Inherited from base State class.
    /// </summary>
    abstract public class Phase3 : State
    {
        public Phase3(BossStateMachine thisSM) : base(thisSM) { }
        
        public Phase3() : base() { }
        
        public static int hitsTakenWithoutRetaliation;

        public override void OnStateEnter()
        {
            stateName = "Phase3_";
            base.OnStateEnter();
        }

        public override void OnStateExit() 
        { 
            base.OnStateExit();
        }

        public override void OnUpdate() 
        { 
            base.OnUpdate();
        }
    }
}
