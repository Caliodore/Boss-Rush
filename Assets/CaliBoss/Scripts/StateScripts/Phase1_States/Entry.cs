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

            public bool waitingToEnter = true;
            public override bool IsChosen { get => isChosen; set => isChosen = value; }

            private void Awake()
            {
                clergySM = (P1_SM_Generic)attachedSM;
            }

            public override void OnStateEnter()
            {
                stateName += "Entry";
                base.OnStateEnter();

                if(waitingToEnter)
                    //Bounce back and forth in the upper pews until can move to spawn.

                if(attachedSM.AttachedBM.BossAlerted)
                {
                    clergySM.ChangeState("Aggro");
                }
                else
                { 
                    clergySM.ChangeState("Idle");
                }
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
            }
        } 
    }
}
