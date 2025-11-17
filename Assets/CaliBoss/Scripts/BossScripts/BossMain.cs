using UnityEngine;
using Caliodore.States_Phase1;
using Caliodore.States_Phase2;
using Caliodore.States_Phase3;

namespace Caliodore
{
    /// <summary>
    /// To be used alongside BossStateMachine per phase to handle enacting logic. <br/>
    /// I.e.: BossStateMachine swaps states and then invokes events and BossMain does stuff accordingly.
    /// </summary>
    public abstract class BossMain : MonoBehaviour
    {
        /*
         * What kinds of things would BSM need to call, but not do itself? i.e.: influenced by states, but not directly tied.
         *  } Moving towards target.
         *      - NavMesh path generation is already handled by the Navigator script.
         *  } Doing damage to player/reacting to damage
         *      - Damageable, Sensor, Damager, 
         *  } Animations, but that will probably be handled by another script specific for interacting with the animator.
         *  } Set values for above scripts, which will be done at Start().
         *  } 
         */

        //Declare variables
        //--------------------//
        //~~xxx~~//

        //---Backing Fields---//
        //~~Refs~~//
        private BossStateMachine attStateMachine;
        private Damager attDamager;
        private Damageable attDamageable;
        private Navigator attNavigator;
        private Sensor attSensor;
        //~~Ints~~//
        private int currentPhase;        

        //-----Properties-----//
        //~~Refs~~//
        public BossStateMachine AttachedStateMachine {  get { return attStateMachine; } set { attStateMachine = value;} }
        //~~Ints~~//
        public int CurrentPhase { get { return currentPhase; } set { currentPhase = value;} }
        
        //Begin logic
        private void Start()
        {
            SetRefs();
        }

        public BossMain(int currentPhase)
        {
            this.CurrentPhase = currentPhase;
        }
        private void SetRefs() 
        { 
            attStateMachine = gameObject.GetComponent<BossStateMachine>();

        }

    }
}
