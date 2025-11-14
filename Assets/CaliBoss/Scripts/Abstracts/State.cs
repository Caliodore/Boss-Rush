using UnityEngine;
using UnityEngine.Events;

namespace Caliodore
{
    /// <summary>
    /// The abstract class for which the state machine references to determine behaviour.
    /// </summary>
    abstract public class State : MonoBehaviour
    {
        [Header("Consistent Vars")]
        public bool stateComplete { get; protected set; }

        [Header("Protected Vars")]
        protected float stateStartTime;
        public float time => Time.time - stateStartTime;
        protected string stateName;

        [Header("References")]
        [SerializeField] static GameObject playerObj;
        [SerializeField] BossStateMachine attachedSM;

        public State(BossStateMachine thisSM)
        { 
            attachedSM = thisSM;    
        }

        public virtual void OnStateEnter() 
        {
            print($"Entered state: {stateName}");
            stateStartTime = 0;    
        }
        public virtual void OnUpdate() { }
        public virtual void OnStateExit() { }
    }
}
