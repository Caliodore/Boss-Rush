using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;

namespace Cali3
{ 
    public abstract class T3_State : MonoBehaviour
    {
        public bool stateComplete = false;
        protected string stateName = new string("");
        public float currentStateDuration = 0f;
        public UnityEvent OnEnteringStateOneShot = new UnityEvent();

        public T3_State(string inputName) { stateName += inputName; }

        private void Start()
        {
            //print($"This is my state name: {stateName}");
        }

        public virtual void OnStateEnter() 
        {
            stateComplete = false;
            print($"Entered state: {stateName}");
            currentStateDuration = 0;
        }
        public virtual void OnUpdate() 
        { 
            currentStateDuration += Time.deltaTime;
        }
        public virtual void OnStateExit() 
        { 
            stateComplete = true;
            print(stateName + " was active for " + currentStateDuration + " seconds.");
        }
        public virtual void StateEnteredEventChecker()
        { 
            OnEnteringStateOneShot?.Invoke();
            OnEnteringStateOneShot.RemoveAllListeners();
        }
        public override string ToString()
        {
            return stateName;
        }
    }
}
