using UnityEngine;
using UnityEngine.Events;

namespace Cali6
{ 
    public abstract class A6_StateBase : MonoBehaviour
    {
        public string StateName = "";
        public A6_StateBase(string nameIn) { StateName += nameIn; }

        public static A6_StateBase StateInstance;
        public UnityEvent OnStartingAnimation;

        //Public Fields
        public bool IsAnimating { get {  return isAnimating; } }
        public bool StateComplete { get { return stateComplete; } }
        public float CurrentStateDuration { get { return currentStateDuration; } }
        public bool isCurrentState;

        //Backing Fields
        private bool isAnimating;
        private bool stateComplete;
        private float currentStateDuration = 0f;

        private void Awake()
        {
            OnStartingAnimation ??= new();
        }

        public virtual void OnStateEnter() { currentStateDuration = 0f; stateComplete = false; isCurrentState = true; print($"Entered the {StateName} state.");}
        public virtual void OnStateUpdate() { currentStateDuration += Time.deltaTime; }
        public virtual void OnStateExit() { stateComplete = true; isCurrentState = false; }

        public virtual void OnDamagedDuringState() { if(!isCurrentState) return; }

        public void ToggleAnimatingBool(bool animatingState) { 
            isAnimating = animatingState;
        }
    }
}
