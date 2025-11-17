using UnityEngine;
using Caliodore;
using Caliodore.States_Phase1;
using Caliodore.States_Phase2;
using Caliodore.States_Phase3;
using System.Collections;
using System.Collections.Generic;

namespace Caliodore
{
    public abstract class BossStateMachine : MonoBehaviour
    {
        /*
         * Script for object that will persist through phases on any entity considered part of the boss.
         * Clergy, chosen, and Martyr boss all included and all will have this script, or an inherited version of it.
         * Intakes a state, then determines actions based on phase, and type of boss enemy (i.e.: if clergy or chosen). 
         * 
         * ----BSMs CONTROL *HOW OFTEN* STATE METHODS GET CALLED----
         */

        public static GameObject playerObjRef;

        private State currentState;
        private int currentPhase;
        private List<State> attachedStates = new List<State>();

        public virtual BossBrain AttachedBM { get; protected set; }

        public virtual int CurrentPhase { get { return currentPhase;} protected set { currentPhase = value;} }
        public virtual State CurrentState { get { return currentState;} protected set { currentState = value;} }
        public virtual List<State> AttachedStates { get { return attachedStates; } protected set { attachedStates = value; } }

        public BossStateMachine() { }

        public BossStateMachine(BossBrain thisBM)
        { 
            AttachedBM = thisBM;
        }

        public void Update()
        {
            //currentState.OnUpdate();
        }

        public void ChangeState(State changeToState)
        { 
            if(currentState != null)
                currentState.OnStateExit();

            currentState = changeToState;

            currentState.OnStateEnter();
        }
        
        protected void GetAttachedStates()
        { 
            attachedStates.Clear();
            var childComps = GetComponentsInChildren<Phase1>();
            foreach(Phase1 stateCurrent in childComps) 
            {
                attachedStates.Add(stateCurrent);
            }
        }
    }
}
