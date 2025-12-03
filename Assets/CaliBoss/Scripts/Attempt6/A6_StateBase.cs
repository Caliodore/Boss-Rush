using UnityEngine;
using UnityEngine.Events;

namespace Cali6
{ 
    public abstract class A6_StateBase : MonoBehaviour
    {
        public string StateName = "";
        public A6_StateBase(string nameIn) { StateName += nameIn; }

        public static A6_StateBase StateInstance;

        //Public Fields
        public bool IsAnimating { get {  return isAnimating; } }
        public bool StateComplete { get { return stateComplete; } }
        public float CurrentStateDuration { get { return currentStateDuration; } }

        //Backing Fields
        private bool isAnimating;
        private bool stateComplete;
        private float currentStateDuration = 0f;

        private void Awake()
        {
            
        }

        public virtual void OnStateEnter() { currentStateDuration = 0f; stateComplete = false;}
        public virtual void OnStateUpdate() { currentStateDuration += Time.deltaTime; }
        public virtual void OnStateExit() { stateComplete = true; }

        public virtual void OnDamagedDuringState() { }

        protected void ToggleAnimatingBool(bool animatingState) { 
            isAnimating = animatingState;
        }
    }
}
