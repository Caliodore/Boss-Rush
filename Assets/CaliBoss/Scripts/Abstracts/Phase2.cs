using UnityEngine;
using UnityEngine.Events;

namespace Caliodore
{
    /// <summary>
    /// Framework for state classes for second boss phase.<br/>
    /// Inherited from base State class.
    /// </summary>
    abstract public class Phase2 : State
    {
        //public static int hitsTakenWithoutRetaliation;

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
