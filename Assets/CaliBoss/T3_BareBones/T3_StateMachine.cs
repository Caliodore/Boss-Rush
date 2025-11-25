using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Cali3
{ 
    public class T3_StateMachine : MonoBehaviour
    {
        public T3_State currentState;

        private void Start()
        {
            print("T3_SM Start Finished");
            print("T3_SM prints: " + T3_Brain.debugTestMsg + " in Start.");
        }

        public void ChangeState(string inputString)
        {
            print("T3_SM prints: " + T3_Brain.debugTestMsg + " in ChangeState.");
            if(T3_Brain.MainBrain.BossStates == null)
            { 
                print("Apparently BossStates doesn't exist despite it explicitly existing.\nSo no changing states for you :3");
                return;
            }
            int stateIndex = 0;
            foreach(T3_State currentEntry in T3_Brain.MainBrain.BossStates)
            {
                print(currentEntry.ToString());
                if(currentEntry.ToString() == inputString)
                { 
                    if(currentState != null)
                        currentState.OnStateExit();

                    currentState = currentEntry;

                    currentState.OnStateEnter();
                    
                    return;
                }
                else
                { 
                    print($"Skipped past index {stateIndex}.");    
                }
                stateIndex++;
            }
        }

        public void TestingBrainRef() { print("Can reference T3_SM."); }

        public void Update()
        {
            if(currentState != null)
                currentState.OnUpdate();
        }

    }
}
