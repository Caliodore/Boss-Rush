using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;

namespace CaliJR
{ 
    public class C2JR_StateMachine : MonoBehaviour
    {
        public JRState currentState;
        public static C2JR_StateMachine BossSM;
        public string stateName;
        public bool stateComplete;
        public static UnityEvent PlayerEntersMelee;
        public static UnityEvent PlayerLeavesMelee;

        private void Awake()
        {
            ChangeState("Idle");

        }

        public void ChangeState(string inputString)
        { 
            foreach(JRState currentEntry in C2JR_Brain.BossStates)
            { 
                if(currentEntry.ToString() == inputString)
                { 
                    if(currentState != null)
                        currentState.OnStateExit();

                    currentState = currentEntry;

                    currentState.OnStateEnter();

                    
                    return;
                }
            }
        }

        public void Update()
        {
            currentState.OnUpdate();
        }
    }
}
