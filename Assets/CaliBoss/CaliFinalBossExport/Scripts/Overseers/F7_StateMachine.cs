using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;

namespace Cali7
{ 
    public class F7_StateMachine : MonoBehaviour
    {
        public bool refsLoaded = false;
        public bool printDebugLogs = true;
        public bool testRandomStateSwap = false;
        public static F7_StateMachine Instance;
        public F7_StateBase CurrentState;
        public F7_StateBase lastRequested;
        public float changeLimiterTimer = 0.75f;
        public bool rateBeingLimited;
        private Transform animatorTransform;
        public float stateTimeOut = 20f;
        Coroutine limiterCoro;

        [SerializeField] public float currentStateDuration;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;}

        private void Start()
        {
            if(!F7_RefManager.Instance.gotRefs)
                F7_RefManager.OnRefsLoaded?.AddListener(() => SetReferences());
            else
                SetReferences();
        }

        public void SetReferences() { 
            F7_EventManager.Instance.OnArenaEntered?.AddListener(() => ChangeState(F7_RefManager.BSTI));
            F7_EventManager.Instance.OnBossTakesDamage?.AddListener(dmgIn => ReactToDamage());
            F7_EventManager.Instance.OnBossTakesDamage?.AddListener(dmgIn => F7_EventManager.Instance.OnBossTakesDamage?.RemoveListener(dmgIn => ReactToDamage()));
            refsLoaded = true;
        }

        private void Update()
        {
            if(refsLoaded) {
                if(CurrentState != null) { 
                    CurrentState.OnStateUpdate();
                    currentStateDuration = CurrentState.currentStateDuration;

                    if(currentStateDuration >= changeLimiterTimer) { 
                        rateBeingLimited = false;
                    }
                    else if(currentStateDuration < changeLimiterTimer){ 
                        rateBeingLimited = true;
                    }
                }
                else { 
                    rateBeingLimited = false;
                }
            }
        }

        public void ReactToDamage() { ChangeState(F7_RefManager.BSTA); }

        public void ChangeState(F7_StateBase stateTo) { 

            lastRequested = stateTo;
            F7_Help.DebugPrint(printDebugLogs, $"Attempting swap to: {stateTo.ToString()} State from {CurrentState?.ToString()} State.");

            if(rateBeingLimited) { 
                F7_Help.DebugPrint(printDebugLogs,"The rate of state changes is being limited currently. State will change in a moment.");
                return;
            }
            else { 
                F7_Help.DebugPrint(printDebugLogs,"The rate is NOT being limited.");
            }

            bool canSwitch = CheckIfDiffState(stateTo) && !rateBeingLimited;

            if(!canSwitch) { 
                F7_Help.DebugPrint(printDebugLogs, "The requested state is already running, or it is being limited atm.");
                return;
            }
            else if(stateTo == null) { 
                F7_Help.DebugPrint(printDebugLogs, "The requested state cannot be null.");
                return;
            }
            else { 
                if(CurrentState != null)
                    CurrentState.OnStateExit();

                CurrentState = stateTo;

                CurrentState.OnStateEnter();
            }
        }

        private bool CheckIfDiffState(F7_StateBase stateTo) {
            if(stateTo == CurrentState) { 
                return false;
            }
            else { 
                return true;
            }
        }
    }
}
