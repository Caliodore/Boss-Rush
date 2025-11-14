using UnityEngine;
using Caliodore.States_Phase1;
using Caliodore.States_Phase2;
using Caliodore.States_Phase3;

namespace Caliodore
{
    public class BossStateMachine : MonoBehaviour
    {
        /*
         * Script for object that will persist through phases on any entity considered part of the boss.
         * Clergy, chosen, and Martyr boss all included and all will have this script, or an inherited version of it.
         * Intakes a state, then determines actions based on phase, and type of boss enemy (i.e.: if clergy or chosen). 
         */

        State currentState;
        BossMain attachedBM;

        public int currentPhase { get; private set; }

        public BossStateMachine(BossMain thisBM)
        { 
            attachedBM = thisBM;
        }

        public void Update()
        {
            currentState.OnUpdate();
        }

        public void ChangeState(State changeToState)
        { 
            if(currentState != null)
                currentState.OnStateExit();

            currentState = changeToState;

            currentState.OnStateEnter();
        }
    }
}
