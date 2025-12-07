using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Cali7
{ 
    public class F7_StateMachine : MonoBehaviour
    {
        public bool printDebugLogs = true;
        public static F7_StateMachine Instance;
        public F7_StateBase CurrentState;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        private void Update()
        {
            if(CurrentState != null)
                CurrentState.OnStateUpdate();
            F7_EventManager.Instance.OnArenaEntered?.AddListener(() => ChangeState(F7_RefManager.BSTI));
            F7_EventManager.Instance.OnBossTakesDamage?.AddListener(dmgIn => ReactToDamage());
            F7_EventManager.Instance.OnBossTakesDamage?.AddListener(dmgIn => F7_EventManager.Instance.OnBossTakesDamage?.RemoveListener(dmgIn => ReactToDamage()));
        }

        public void ReactToDamage() { ChangeState(F7_RefManager.BSTA); }

        public void ChangeState(F7_StateBase stateTo) { 
            bool canSwitch = CheckIfDiffState(stateTo);
            F7_Help.DebugPrint(printDebugLogs, "Changing state.");
            if(!canSwitch) { 
                F7_Help.DebugPrint(printDebugLogs, "The requested state is already running.");
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
            else
                return true;
        }
    }
}
