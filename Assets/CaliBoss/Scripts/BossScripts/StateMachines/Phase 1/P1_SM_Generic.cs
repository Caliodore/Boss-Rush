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
        private P1_Overseer attachedBM; 
        public override BossBrain AttachedBM { get => attachedBM; protected set => attachedBM = (P1_Overseer)value; }

        public P1_SM_Generic() : base() { }

        public P1_SM_Generic Instance;

        public List<Phase1> attachedStates = new List<Phase1>();
        public Dictionary<string, Phase1> AttP1StatesDict = new Dictionary<string, Phase1>();

        private void Awake()
        {
            if(Instance == null)
                Instance = this;

            CreateStateDictionary<Phase1>();

            /*attachedStates = GetAttachedStates<Phase1>();
            foreach(Phase1 thisState in attachedStates)
            {
                thisState.SetSMRef<P1_SM_Generic>(this);
            }*/
        }

        private void Start()
        {
            Phase1 entryState = (Phase1)AttStatesDict["Entry"];
            CurrentState = entryState;
        }

        public override void ChangeState<Phase1>(string stateName)
        {
            base.ChangeState<Phase1>(stateName);
        }

        public void ChangeState(string inputState)
        { 
            base.ChangeState<Phase1>(inputState);
        }

        /// <summary>
        /// Method to handle and keep track of an enemy before it enters the arena.
        /// </summary>
        public void DequeueSpawn()
        { 
            //if()
        }

    }
}