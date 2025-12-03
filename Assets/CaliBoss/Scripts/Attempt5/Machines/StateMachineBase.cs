using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace CaliBoss
{ 
    public class StateMachineBase : MonoBehaviour
    {
        public StateMachineBase() { }
        public static StateMachineBase Instance;
        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        //CurrentState
        public StateBase CurrentState {get { return Instance.currentState; } }
        private StateBase currentState;

        //Collections
        public List<StateBase> BossStates = new List<StateBase>();

        //Testing
        public bool isFailure = false;

        private void Start()
        {
            GenerateStateList();
            ChangeState(BossStates.Find(x => x.Equals(typeof(Pursuing))));
        }

        private void Update()
        {
            if(currentState != null)
                currentState.OnUpdate();
            else
                isFailure = true;
        }

        /// <summary>
        /// Internal method to grab all states and put them into a list for future reference.<br/>
        /// Also makes sure the Entering state is started on and active.
        /// </summary>
        private void GenerateStateList()
        { 
            var compArray = GetComponentsInChildren<StateBase>();
            foreach(StateBase indexState in compArray)
            {
                BossStates.Add(indexState);
                if(indexState.Equals(typeof(Entering)))
                { 
                    currentState = indexState;
                    currentState.gameObject.SetActive(true);
                }
            }
            print($"Generated State List Count: {BossStates.Count}");
        }

        /*
         * On entering a state have it call a method to determine what exactly it is doing, and that will be the only time that type of interaction occurs.
         * This helps limit how often methods get called and can guarantee we only get a single proc of events starting.
         * While going through update we can check if a state has been going on for too long and then ask for a change.
         */

        /// <summary>
        /// Internal method that actually takes in and does the step-by-step change.
        /// </summary>
        /// <param name="stateTo">State ref desired to change to.</param>
        private void ChangeState<T>(T stateTo) where T : StateBase
        { 
            print("ChangeState called");
            if(stateTo != currentState && stateTo != null)
            { 
                if(currentState != null)
                    currentState.OnStateExit();

                currentState = stateTo;

                currentState.OnStateEnter();
            }
        }

        /// <summary>
        /// The method for publicly accessing the ability to change currentState.
        /// </summary>
        /// <typeparam name="T">StateBase child type.</typeparam>
        /// <param name="stateType">The name of the child type.</param>
        public void RequestStateChange<T>(T stateType) where T : StateBase
        { 
            ChangeState(BossStates.Find(inputType => inputType.Equals(stateType)));
        }

        public T GetStateRef<T>() where T : StateBase {
            return BossStates.Find(xIn => xIn.Equals(typeof(T))) as T;
        }

        public void AddStateChangeListenerOnHit(StateBase stateTo) { 
            UnityAction stateChangeShenanigans = null;
            stateChangeShenanigans += (UnityAction)(() => RequestStateChange(stateTo)) + (UnityAction)(() => RemoveOnHitListenersForThisState(stateTo)) + (UnityAction)(() => CSR.Instance.BossDamager.OnSuccessfulHit.RemoveListener(() => RequestStateChange(stateTo)));
            CSR.Instance.BossDamager.OnSuccessfulHit.AddListener(stateChangeShenanigans);
        }

        public void RemoveOnHitListenersForThisState(StateBase stateTo) { 
            UnityAction stateChangeShenanigans = null;
            stateChangeShenanigans += (UnityAction)(() => RequestStateChange(stateTo)) + (UnityAction)(() => RemoveOnHitListenersForThisState(stateTo)) + (UnityAction)(() => CSR.Instance.BossDamager.OnSuccessfulHit.RemoveListener(() => RequestStateChange(stateTo)));
            CSR.Instance.BossDamager.OnSuccessfulHit.RemoveListener(stateChangeShenanigans);
        }
    }
}
