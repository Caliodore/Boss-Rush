using UnityEngine;

namespace Caliodore
{
    /// <summary>
    /// Framework for state classes for first boss phase.<br/>
    /// Inherited from base State class.
    /// </summary>
    public abstract class Phase1 : State
    {
        protected bool isAlerted;
        protected bool isChosen;
        public virtual bool IsAlerted { get { return isAlerted; } set { isAlerted = value; } }
        public virtual bool IsChosen { get { return isChosen; } set { isChosen = value; } }

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
