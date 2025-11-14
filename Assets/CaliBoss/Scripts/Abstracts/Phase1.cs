using UnityEngine;

namespace Caliodore
{
    /// <summary>
    /// Framework for state classes for first boss phase.<br/>
    /// Inherited from base State class.
    /// </summary>
    abstract public class Phase1 : State
    {
        public Phase1(BossStateMachine thisSM) : base(thisSM) { }

        public Phase1() : base() { }

        public bool isChosen { get; private set; }

        public override void OnStateEnter()
        {
            stateName = "Phase1_";
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
