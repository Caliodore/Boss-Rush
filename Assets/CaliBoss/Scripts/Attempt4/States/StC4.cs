using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Cali_4
{ 
    public abstract class StC4 : MonoBehaviour
    {
        //Newer stuff trying out
        public UnityEvent StopThisState = new UnityEvent();
        protected C4_StateMachine intendedSM = C4_HostBrain.BossSM;
        protected C4_StateDeterminant intendedSD = C4_HostBrain.BossStateDeterminant;
        protected StC4 intendedNextState = null;
        protected StC4 parentState = null;
        protected List<StC4> SubStates = new List<StC4>();
        protected bool nextStateSet = false;
        protected bool isParentState = false;
        protected StC4 parentRef = null;
        protected Type parentType = null;

        //Original Backend
        protected string stateName = new string("");
        protected float currentStateDuration = 0f;
        protected bool stateComplete = false;
        protected bool stateStopping = false;
        
        //Public properties
        public bool StateComplete { get { return stateComplete;} }
        public bool StateStopping { get { return stateStopping;} }
        public bool IsParentState { get { return isParentState; } }
        public bool printDebugs = true;

        public StC4(string nameIn) { parentType = GetType().BaseType; stateName += nameIn; }

        private void Awake()
        {
            OnAwake();
        }

        public virtual void OnAwake()
        {
            //CheckIfParent();
            //Helpers.DebugPrint($"ParentType: {parentType?.Name} || ChildClass: {GetType().Name}", printDebugs);
            //Helpers.DebugPrint($"ParentRef Name: {parentRef?.stateName}", printDebugs);
            nextStateSet = false;
            currentStateDuration = 0f;
        }

        public virtual void OnStateEnter() 
        {
            //gameObject.SetActive(true);

            //if(!isParentState)
                //parentRef.gameObject.SetActive(true);

            stateComplete = false;
            stateStopping = false;

            Helpers.DebugPrint($"Entered state: {stateName}", printDebugs);
            currentStateDuration = 0f;
        }

        public virtual void OnUpdate() 
        {
            currentStateDuration += Time.deltaTime;

            if(stateComplete)
                StopThisState?.Invoke();
            if(stateStopping)
                OnStateExit();
        }

        public virtual void OnStateExit() 
        { 
            stateComplete = true;
            C4_HostBrain.BossSM.ChangeState(intendedNextState);
            //gameObject.SetActive(false);
        }

        //Inheritance Methods

        private void GetSubStates()
        {
            /*StC4[] subList = GetComponentsInChildren<StC4>();
            foreach(StC4 subState in subList)
                SubStates.Add(subState);*/
        }

        private StC4 GetParentRef()
        {
            var arrayGuh = GetComponentsInParent<StC4>();
            foreach(StC4 currentSt in arrayGuh)
            { 
                if(currentSt.GetType().Equals(GetType().BaseType))
                    return currentSt;
            }
            Helpers.DebugPrint("Didn't find anything that matched this script's base type.", printDebugs);
            return null;
        }

        private void CheckIfParent()
        { 
            if(GetType().BaseType.Name == "StC4")
            {
                isParentState = true;
                //GetSubStates();
            }
            else
            {
                isParentState = false;
                //parentRef = GetParentRef();
                SubStates = null;
            }
        }

        public override string ToString()
        {
            return stateName;
        }

        /// <summary>
        /// Method to be called by other states to set a specific/different from normal state that is desired for this one to go to after completing.
        /// </summary>
        /// <param name="stateTo">State to go to after </param>
        public void SetNextState(StC4 stateTo)
        { 
            intendedNextState = stateTo;
            if(intendedNextState != null)
                nextStateSet = true;
            else
                Helpers.DebugPrint("Called SetNextState but intendedNextState is still null. Make sure the state reference is to one instantiated and attached to a GameObject.", printDebugs);
        }

        /// <summary>
        /// Method to check if there is an intendedNextState set or not, and if so update listeners accordingly.
        /// </summary>
        private void CheckNextStateStatus()
        { 
            if(intendedNextState == null)
            { 
                intendedNextState = intendedSD.DetermineNextState(this);
                if(intendedNextState == null)
                    Helpers.DebugPrint($"The state {stateName} could not have an intended next state determined.", printDebugs);
                else
                { 
                    nextStateSet = true;
                }
            }
            else
                nextStateSet = true;

            SetStopEventListeners(nextStateSet);
        }

        /// <summary>
        /// Method that makes sure there is a listener added to StopThisState, or that all listeners are removed if input is false.<br/>
        /// Intended to be called within other methods that check for an intendedNextState.
        /// </summary>
        /// <param name="addOrRemove">T : AddListener(() => ManualStopState()) || F : RemoveAllListeners</param>
        private void SetStopEventListeners(bool addOrRemove)
        { 
            if(addOrRemove)
            {
                StopThisState?.AddListener(() => ManualStopState());
            }
            else
            {
                StopThisState?.RemoveAllListeners();
                Helpers.DebugPrint("No intended next state could be found, so all listeners to the inherent UnityEvent StopThisState have been removed if they exist.", printDebugs);
            }
        }

        /// <summary>
        /// Method StopThisState calls to stop the state that contains it. Not to be called externally, please Invoke the StopThisState UnityEvent instead.<br/>
        /// If no intendedNextState is set, this method will not be called by invoking StopThisState.
        /// </summary>
        protected void ManualStopState()
        {
            StopThisState?.RemoveAllListeners();
            CheckNextStateStatus();
            stateStopping = true;    
        }

        public virtual void OnTakingDamage() { }

        public virtual void OnGivingDamage() { }

        public virtual void OnPlayerEnterMelee() { }
    }
}
