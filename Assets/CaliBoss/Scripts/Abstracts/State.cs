using UnityEngine;
using UnityEngine.Events;

namespace Caliodore
{
    /// <summary>
    /// The abstract class for which the state machine references to determine behaviour.
    /// </summary>
    public abstract class State : MonoBehaviour
    {
        [Header("Consistent Vars")]
        public string StateName { get { return stateName; } set { stateName = value; } }
        public bool stateComplete { get; protected set; }
        public UnityEvent HitPlayerSuccess { get; protected set; }
        public UnityEvent DamagedByPlayer { get; protected set; }

        [Header("Protected Vars")]
        protected float stateStartTime;
        public float time => Time.time - stateStartTime;
        protected float currentStateDuration;
        protected string stateName;

        [Header("References")]
        [SerializeField] static GameObject playerObj;
        [SerializeField] public BossStateMachine attachedSM;

        public virtual void SetSMRef<T>(BossStateMachine inputBSM) where T : BossStateMachine
        { 
            attachedSM = (T)inputBSM;
        }

        public virtual void OnStateEnter() 
        {
            print($"Entered state: {stateName}");
            stateStartTime = 0;
            currentStateDuration = 0;
        }
        public virtual void OnUpdate() 
        { 
            currentStateDuration += Time.deltaTime;
        }
        public virtual void OnStateExit() { }

        public virtual void StateDurationElapsed() { }

        public virtual void DamageTaken() { }

        public virtual void PlayerDamaged() { }
    }
}
