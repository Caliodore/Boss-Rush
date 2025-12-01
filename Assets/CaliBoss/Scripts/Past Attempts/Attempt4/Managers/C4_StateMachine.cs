using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cali_4
{ 
    public class C4_StateMachine : MonoBehaviour
    {
        [Header("Components")]
        private C4_StateDeterminant attachedDeterminant = C4_StateDeterminant.Instance;

        [Header("State Change Related")]
        public StC4 currentState = null;
        protected StC4 mostRecentRequest = null;
        public static List<StC4> AllStates = new List<StC4>();
        public bool acceptingRequests = false;

        [Header("Testing Vars")]
        public bool printDebugs = true;
        public float limiterTimer = 0.1f;

        public static C4_StateMachine Instance;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;

            if(AllStates.Count < 1)
                AllStates = GetAttachedStates();

            currentState = AllStates.Find(startState => startState.Equals(typeof(EntryWait)));

            C4_HostBrain.BossAnimManager.bossAnimator.SetTrigger("Transition");

            currentState.OnStateEnter();
        }

        private void Update()
        {
            if(currentState != null)
                currentState.OnUpdate();
        }

        private List<StC4> GetAttachedStates()
        {
            StC4[] attStates = GetComponentsInChildren<StC4>();
            List<StC4> tempList = new();

            foreach(StC4 indexState in attStates)
            { 
                tempList.Add(indexState);
            }
            return tempList;
        }

        /// <summary>
        /// How state machine changes currentState. Checks with helper methods whether the requested change is possible, then runs corresponding state methods. <br/>
        /// If currentState != null, its OnExit method is called, then the currentState is updated and its OnEnter method is called.
        /// </summary>
        /// <param name="changeState"></param>
        public void ChangeState(StC4 changeState)
        {
            if(acceptingRequests)
                StartCoroutine(RequestLimiter());

            var requestedChange = AllStates.Find(x => x.Equals(changeState));
            if(requestedChange == null)
            { 
                Helpers.DebugPrint("Something is requesting to change to a null state, or one that is not within the SM's collection.",printDebugs);
                return;
            }

            bool canChange = CompareStates(requestedChange);
            if(canChange)
            {
                Helpers.DebugPrint($"Changing states from {currentState.StateName} to {changeState.StateName}.", printDebugs);
                if(currentState != null)
                    currentState.OnStateExit();

                currentState = changeState;

                C4_HostBrain.BossAnimManager.bossAnimator.SetTrigger("Transition");

                currentState.OnStateEnter();
            }
            else
            {
                Helpers.DebugPrint($"Cannot change state to the requested one.\nCurrent: {currentState?.ToString()} | Requested: {changeState.ToString()}", printDebugs);
                return;
            }
        }

        /// <summary>
        /// Internal method to handle updating current state and the latest request for reference when changing state.
        /// </summary>
        /// <param name="changeState">State being requested to change to from the currentState.</param>
        /// <returns>Whether or not the most recent request is the same as the currentState.</returns>
        private bool CompareStates(StC4 changeState)
        {
            if(mostRecentRequest != currentState)
            {
                if(mostRecentRequest != changeState)
                {
                    mostRecentRequest = changeState;
                    Helpers.DebugPrint($"Updated requested state: {changeState.ToString()}", printDebugs);
                }
                return true;
            }
            else
            {
                Helpers.DebugPrint("CompareStates says mostRecentRequest == currentState",printDebugs);
                return false;
            }
        }

        /// <summary>
        /// Coroutine to help limit how often state changes can be requested. Changed by limiterTimer.
        /// </summary>
        /// <returns></returns>
        IEnumerator RequestLimiter()
        { 
            acceptingRequests = false;
            yield return new WaitForSeconds(limiterTimer);
            acceptingRequests = true;
        }

        public static StC4 GetStateRef(string refName)
        {
            return AllStates.Find(stateOut => stateOut.StateName == refName);
        }
    }
}
