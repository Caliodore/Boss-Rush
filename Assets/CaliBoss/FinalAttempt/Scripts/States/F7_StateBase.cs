using UnityEngine;
using UnityEngine.Events;

namespace Cali7
{ 
    public abstract class F7_StateBase : MonoBehaviour
    {
        public F7_StateBase(string nameIn) { stateName = nameIn; }
        public string stateName;
        public bool stateComplete;
        public float currentStateDuration;
        public UnityEvent OnStateEnding;

        private void Start()
        {
            OnStateEnding ??= new();
        }

        public virtual void OnStateEnter() { currentStateDuration = 0f; stateComplete = false; }
        public virtual void OnStateUpdate() { currentStateDuration += Time.deltaTime; }
        public virtual void OnStateExit() { stateComplete = true; OnStateEnding?.Invoke(); }

        public virtual void StopThisState() { }

        public override string ToString()
        {
            return stateName;
        }
    }
}
