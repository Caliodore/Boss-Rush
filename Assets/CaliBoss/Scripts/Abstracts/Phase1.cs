using UnityEngine;
using UnityEngine.Events;

namespace Caliodore
{
    /// <summary>
    /// Framework for state classes for first boss phase.<br/>
    /// Inherited from base State class.
    /// </summary>
    public abstract class Phase1 : State
    {
        protected ClergySM clergySM;
        protected ClergyBrain clergyBrain;

        protected Phase1(string name) : base("Phase1") { stateName += name; }

        private void Awake()
        {
            clergySM = attachedSM as ClergySM;
            clergyBrain = attachedSM.AttachedBM as ClergyBrain;
        }

        public override void OnStateEnter()
        {
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
