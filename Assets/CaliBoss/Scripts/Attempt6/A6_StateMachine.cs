using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Cali6
{ 
    public class A6_StateMachine : MonoBehaviour
    {
        public bool printDebugLogs = true;
        public A6_StateBase CurrentState {  get { return currentState; } }
        private A6_StateBase currentState;
        public List<A6_StateBase> BossStates = new();
        public A6_StateMachine() { }
        public static A6_StateMachine Instance;

        private void Awake() {
            if(Instance == null)
                Instance = this;
            GetStates();
        }

        private void Start() {
            RequestStateChange(BossStates.Find(entryState => entryState.GetType().Equals(typeof(A6_Idling))));
        }

        private void GetStates() { 
            if(BossStates.Count < 1) { 
                var stateArray = GetComponentsInChildren<A6_StateBase>();
                foreach(A6_StateBase indexState in stateArray) {
                    BossStates.Add(indexState);
                }
            }
        }

        private void Update() {
            if(currentState != null)
                currentState.OnStateUpdate();
        }

        public bool RequestStateChange(A6_StateBase stateTo) { 
            bool outputBool = false;

            if(stateTo == null) { 
                A6_Help.DebugPrint(printDebugLogs, "Cannot change to a null state.");
                return false;
            }

            if(stateTo != currentState) { 
                if(currentState != null)
                    currentState.OnStateExit();

                currentState = stateTo;

                currentState.OnStateEnter();
                outputBool = true;
            }

            return outputBool;
        }

        public void CurrentStateAnimationEnd() {
            A6_Help.DebugPrint(printDebugLogs, "CurrentState's animation ended.");
            currentState.ToggleAnimatingBool(false);
        }
    }
}
