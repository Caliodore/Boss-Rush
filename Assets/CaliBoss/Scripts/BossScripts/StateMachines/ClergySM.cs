using Caliodore.States_Phase1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Caliodore
{

    /// <summary>
    /// Boilerplate for Chosen/Clergy individual StateMachines. Intended for the individual prefabs. <br/>
    /// NOT for use alongside the BossOverseer, as that is for the entire phase.
    /// </summary>
    public class ClergySM : BossStateMachine
    {
        /*
         * This will be the basis for both types of P1 enemies, so either we will have two separate scripts inherited from this attached to all enemies, or...
         * ...have a singular script that is able to handle both states (Chosen/Clergy). I am leaning towards the latter.
         * 
         * We will have this be attached at the base level that most other scripts/components are attached to, with an empty child to hold all the States.
         * Hopefully this will make things more organized.
         */

        //Refs
        public override BossBrain AttachedBM { get => attachedBM; protected set => attachedBM = (ClergyBrain)value; }
        private ClergyBrain attachedBM;
        public ClergySM Instance;

        //Collections
        public Dictionary<string, Phase1> AttP1States = new Dictionary<string, Phase1>();
        public List<Phase1> attStatesHere = new List<Phase1>();

        //Constructor
        public ClergySM() : base() { attachedStateMachine = this; }

        private void Awake()
        {
            if(Instance == null)
                Instance = this;

            AttP1States = CreateStateDictionary(attStatesHere);
        }

        private void Start()
        {
            if(AttP1States.Count > 0)
            {
                bool entryCheck = AttP1States.ContainsKey("Entry");
                //CurrentState = entryState;
            }
        }

        public override void ChangeState<Phase1>(Phase1 inputState)
        {
            base.ChangeState(inputState);
        }

        /// <summary>
        /// Method to handle and keep track of an enemy before it enters the arena.
        /// </summary>
        public void DequeueSpawn()
        { 
            //if()
        }

        public void DeathFadeAway()
        { 
            
        }
    }
}