using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR.Haptics;

namespace Caliodore
{
    namespace States_Phase1
    {
        /// <summary>
        /// State that handles when clergy members are dequeued and spawned into the upper pews of the arena. <br/>
        /// Also will handle the opening logic and checks when the player first starts fight.
        /// </summary>
        public class Entry : Phase1
        {
            /*
             * Purpose of the entry state: to handle clergy members incoming to the arena after being dequeued mainly.
             * Is meant to translate directly into the Idle state, being a transient state to confirm checks.
             * Queued enemies will linger in this state until they are called to jump into the arena.
             * This will be their state when they are dequeued and set active, up until they enter the actual arena.
             */
            public Entry() : base("Entry") { }

            public bool waitingToEnter = true;

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                waitingToEnter = true;

                if(attachedSM.AttachedBM.BossAlerted)
                {
                    clergySM.ChangeState(clergySM.AttP1States["Aggro"]);
                }
                else
                { 
                    clergySM.ChangeState(clergySM.AttP1States["Idle"]);
                }
            }

            public override void OnUpdate() { }

            public void OnEnable()
            {
                OnStateEnter();
            }
        } 
    }
}
