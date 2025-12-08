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

        public virtual void OnStateEnter() { currentStateDuration = 0f; stateComplete = false; print($"Entering {stateName} State."); }
        public virtual void OnStateUpdate() { currentStateDuration += Time.deltaTime; if(currentStateDuration > 25f) F7_RefManager.BCNT.StateChangeRequest(); }
        public virtual void OnStateExit() { stateComplete = true; OnStateEnding?.Invoke(); print($"Leaving {stateName} State."); }

        public virtual void StopThisState() { }

        public override string ToString()
        {
            return stateName;
        }
    }
}
