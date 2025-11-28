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
        public static StC4 currentState = null;
        protected StC4 mostRecentRequest = null;
        protected List<StC4> AllStates = new List<StC4>();
        public bool acceptingRequests = false;

        [Header("Testing Vars")]
        public bool printDebugs = true;
        public float limiterTimer = 0.5f;

        public static C4_StateMachine Instance;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;

            if(AllStates.Count < 1)
                AllStates = GetAttachedStates();
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
                if(currentState != null)
                    currentState.OnStateExit();

                currentState = changeState;

                currentState.OnStateEnter();
            }
            else
            {
                Helpers.DebugPrint($"Cannot change state to the requested one.\nCurrent: {currentState.ToString()} | Requested: {changeState.ToString()}", printDebugs);
                return;
            }
        }

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
                return false;
        }

        IEnumerator RequestLimiter()
        { 
            acceptingRequests = false;
            yield return new WaitForSeconds(limiterTimer);
            acceptingRequests = true;
        }
    }
}
