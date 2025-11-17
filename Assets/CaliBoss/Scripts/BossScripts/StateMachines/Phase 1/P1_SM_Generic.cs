using UnityEngine;
using Caliodore.States_Phase1;
using System.Collections;
using System.Collections.Generic;

namespace Caliodore
{

    /// <summary>
    /// Boilerplate for Chosen/Clergy individual StateMachines. Intended for the individual prefabs. <br/>
    /// NOT for use alongside the BossOverseer, as that is for the entire phase.
    /// </summary>
    public class P1_SM_Generic : BossStateMachine
    {
        /*
         * This will be the basis for both types of P1 enemies, so either we will have two separate scripts inherited from this attached to all enemies, or...
         * ...have a singular script that is able to handle both states (Chosen/Clergy). I am leaning towards the latter.
         * 
         * We will have this be attached at the base level that most other scripts/components are attached to, with an empty child to hold all the States.
         * Hopefully this will make things more organized.
         */

        P1_SM_Generic Instance;
        
        public List<Phase1> attachedStates;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            GetAttachedStates();
        }

        private void Start()
        {
            Phase1 entryState = attachedStates.Find(x => x.ToString() == "Entry");
            CurrentState = entryState;
        }


    }
}