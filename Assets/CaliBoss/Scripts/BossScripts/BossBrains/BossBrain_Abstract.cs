using UnityEngine;
using Caliodore.States_Phase1;
using Caliodore.States_Phase2;
using Caliodore.States_Phase3;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using System.IO;

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
        private NavMeshAgent attNavAgent;
        private EnemyFramework_SO attachedSO;
        protected static GameObject playerRef;
        //~~Ints~~//
        private int currentPhase;
        private float currentHealth;

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
        
        public NavMeshAgent AttachedNavMeshAgent { get { return attNavAgent; } set { attNavAgent = value; } }

        public virtual EnemyFramework_SO AttachedSO { get { return attachedSO; } set { attachedSO = value; } }

        //~~Ints~~//
        public virtual int CurrentPhase { get { return currentPhase; } set { currentPhase = value;} }
        public virtual float CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
        
        //Begin logic
        private void Start()
        {
            //currentHealth = attachedSO.MaxHealth;
            //SetRefs();
        }
        protected void SetRefs(bool isOverseer) 
        {
            attStateMachine = gameObject.GetComponent<BossStateMachine>();
            if(!isOverseer)
            {
                //currentHealth = attachedSO.MaxHealth;
                attDamager = gameObject.GetComponentInChildren<Damager>();
                attDamageable = gameObject.GetComponentInChildren<Damageable>();
                attNavigator = gameObject.GetComponentInChildren<Navigator>();
                attSensor = gameObject.GetComponentInChildren<Sensor>();
                attNavAgent = gameObject.GetComponentInChildren<NavMeshAgent>();
            }
        }

        private void MoveTo(Vector3 targetLocation)
        {
            if(attNavigator == null)
            { 
                print("There is no NavMeshAgent and/or Navigator attached to this GameObject.");
                return;
            }
            else if(attNavigator.CalculatePathToPosition(targetLocation))
            { 
                attNavAgent.SetDestination(targetLocation);
            }
            else
                print("Enemy could not find path to target location.");
        }

    }
}
