using UnityEngine;

namespace Caliodore
{
    /// <summary>
    /// Framework for state classes for second boss phase.<br/>
    /// Inherited from base State class.
    /// </summary>
    abstract public class Phase2 : State
    {
        public Phase2(BossStateMachine thisSM) : base(thisSM) { }
        public override void OnStateEnter()
        {
            stateName = "Phase2_";
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
