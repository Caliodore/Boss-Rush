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
    public abstract class BossBrain : MonoBehaviour
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

        public Damager AttachedDamager {  get { return attDamager; } set { attDamager = value;} }
        //Methods: { public void SetDamageAmount(int amount) } 
        //UnityEvents: OnContact, OnSuccessfulHit

        public Damageable AttachedDamageable {  get { return attDamageable; } set { attDamageable = value;} }
        //Methods: bool Hit(Damage damage), void ResetIFrames(), void StartHitFlash(), void TestHit(), 
        //UnityEvent: <int>OnInitialize, <Damage>OnHit, OnDeath, <int, int>OnHealthChanged
         

        public Navigator AttachedNavigator {  get { return attNavigator; } set { attNavigator = value;} }
        //Methods: { public bool CalculatePathToPosition(Vector3 targetPosition) }

        public Sensor AttachedSensor {  get { return attSensor; } set { attSensor = value;} }
        //UnityEvents: OnEnter, OnExit


        //~~Ints~~//
        public int CurrentPhase { get { return currentPhase; } set { currentPhase = value;} }
        
        //Begin logic
        private void Start()
        {
            SetRefs();
        }
        private void SetRefs() 
        { 
            attStateMachine = gameObject.GetComponent<BossStateMachine>();
            attDamager = gameObject.GetComponent<Damager>();
            attDamageable = gameObject.GetComponent<Damageable>();
            attNavigator = gameObject.GetComponent<Navigator>();
            attSensor = gameObject.GetComponent<Sensor>();
        }

        private void MoveTo()
        { 
            
        }

    }
}
