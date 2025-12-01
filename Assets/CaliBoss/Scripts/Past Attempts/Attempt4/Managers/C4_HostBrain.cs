using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.AI;
using System;
using DG.Tweening;
using UnityEngine.Animations;

namespace Cali_4
{ 
    public class C4_HostBrain : MonoBehaviour
    {
        static C4_HostBrain() { }

        [Header("Manager Refs")]
        public static C4_HostBrain Buster;
        public static C4_StateMachine BossSM;
        public static C4_StateDeterminant BossStateDeterminant;
        public static C4_UIManager BossUIManager;
        public static C4_AnimManager BossAnimManager;

        [Header("Script Component Refs")]
        public static Actor BossActor;
        public static Damager BossDamager;
        public static Damageable BossDamageable;
        public static Navigator BossNavigator;
        public static NavMeshAgent BossNMAgent;
        public static Sensor BossMeleeSensor;

        [Header("Physical Component Refs")]
        public static Rigidbody BossRigidbody;
        public static Collider BossMeleeCollider;
        public static Collider BossDamagerCollider;

        [Header("GameObject Refs")]
        public static GameObject PlayerObj;
        public static GameObject SpawnerParentObj;
        public static GameObject DamageParentObj;

        [Header("Logic Vars")]
        protected int currentPhase;
        protected int currentHealth;
        protected int maxHealth;
        public bool canTurn = true;

        [Header("Gameplay Events")]
        public UnityEvent OnBossAlerted;

        [Header("Structuring Events")]
        public UnityEvent OnMultipleRecentHits;
        public UnityEvent OnHostBrainValidated;
        public UnityEvent OnAllRefsAssigned;

        [Header("Exposed Fields")]
        public float TurnSpeed { get { return turnSpeed; } }
        public int CurrentCombo { get { return comboCounter; } }
        public int MaxCombo { get { return comboThreshold; } }

        [Header("Backing Fields")]
        private float turnSpeed = 3f;
        private int comboCounter = 0;
        private int comboThreshold = 5;

        [Header("Messy Public Vars to Change")]
        public float turnSpeedFucked = 0.5f;
        public float entranceWaitDuration = 4f;
        public float playerMeleeRange = 3f;
        public float distanceToPlayer;
        public bool isPlayerClose;
        public bool isPlayerInMeleeSensor;
        public float punishAttackTransitionTimer;
        public float regularAttackTransitionTimer;
        [SerializeField] GameObject BloodBarrierObj;

        private void Awake()
        {
            //On awake, needs to collect references, then help each script set their references to make sure no null references occur.
            //Might make some helper functions for this that Brain can call per script.
            if(Buster == null)
                Buster = this;
            DeclareEvents();
            SetRefs();
            DOTween.SetTweensCapacity(1250,50);
        }

        private void Start()
        {
            DamageParentObj.SetActive(false);
            //BossSM.ChangeState(C4_StateMachine.AllStates.Find(startState => startState.GetType().Equals(typeof(EntryWait))));
        }

        private void Update()
        {
            float turnSpeedCurrent;
            distanceToPlayer = Mathf.Abs(Vector3.Distance(PlayerObj.transform.position, transform.position));
            if(distanceToPlayer <= playerMeleeRange)
            { 
                turnSpeedCurrent = turnSpeedFucked;
                isPlayerClose = true;
            }
            else
            { 
                turnSpeedCurrent = turnSpeed;
                isPlayerClose = false;
            }

            if(canTurn)
                transform.DOLookAt(PlayerObj.transform.position, turnSpeedCurrent, AxisConstraint.Y);
        }

//----------------------------------------------------------------------------------------------------------------------------------------------
        //      Internal Methods
        /*
         * Methods generally intended for use within this script or for other scripts handling exchanging/updating info and references.
         */
        /// <summary>
        /// Method I made out of sheer spite and annoyance at trying to make sure SerializeField references were correctly referenced within the script.
        /// </summary>
        private void SetRefs()
        { 
            BossSM = C4_StateMachine.Instance;
            BossStateDeterminant = C4_StateDeterminant.Instance;
            BossUIManager = C4_UIManager.Instance;
            BossAnimManager = C4_AnimManager.Instance;
            BossActor = GetComponentInChildren<Actor>();
            BossDamager = GetComponentInChildren<Damager>();
            BossDamageable = GetComponentInChildren<Damageable>();
            BossNavigator = GetComponentInChildren<Navigator>();
            BossNMAgent = GetComponentInChildren<NavMeshAgent>();
            BossMeleeSensor = GetComponentInChildren<Sensor>();
            BossRigidbody = GetComponentInChildren<Rigidbody>();
            BossMeleeCollider = BossMeleeSensor.gameObject.GetComponent<SphereCollider>();
            BossDamagerCollider = BossDamager.gameObject.GetComponent<BoxCollider>();
            PlayerObj = FindAnyObjectByType<PlayerLogic>().gameObject;
            SpawnerParentObj = FindAnyObjectByType<QueueParent>().gameObject;
            DamageParentObj = BossDamager.gameObject.GetComponentInParent<Transform>().gameObject;
        }

        private void DeclareEvents()
        { 
            OnAllRefsAssigned ??= new UnityEvent();
            OnMultipleRecentHits ??= new UnityEvent();
            OnHostBrainValidated ??= new UnityEvent();
        }

//----------------------------------------------------------------------------------------------------------------------------------------------
//      Player-Focused Methods
        /*
         * Methods focused around interactions with the player, and the variants per type of interactions.
         * To be called by BossSM by states and their sub-states.
         */
//      => Movement
        public void WalkTo() { }
        public void LeapAt() {
                
        }
        public void DashTo() { }
//      => Targeting
        public void TurnToPlayer(bool turningAllowed){
            canTurn = turningAllowed;
        }

        //      => Combat
        //      |> Offense
        //          |> Melee
        public void SlamAttack() { }
        public void SwipeAttack() { }
        public void SweepAttack() { }
//          |> Ranged
        public void ShardSpray() { }
        public void StartPillars() { }
        public void StopPillars() { }
        public void StartClosingRing() { }
        public void StopClosingRing() { }
//      |> Defense
        public void StartBloodBarrier() { }
        public void StopBloodBarrier() { }
//      |> Punishment
        public void BloodBurstAOE() { }
//----------------------------------------------------------------------------------------------------------------------------------------------
        //Animation Event Helpers
        public void EnableDamagerCollider()
        { 
            BossAnimManager.DamagerColliderToggle(true);
        }

        public void DisableDamagerCollider() 
        {
            BossAnimManager.DamagerColliderToggle(false);
        }

        public void EnableBloodBarrier(bool onOrOff)
        { 
            BloodBarrierObj.GetComponent<MeshRenderer>().enabled = onOrOff;
            BloodBarrierObj.SetActive(onOrOff);
        }

        public void BreakBloodBarrier()
        { 
            BloodBarrierObj.GetComponent<MeshRenderer>().enabled = false;
            BloodBarrierObj.GetComponent<ParticleSystem>().Play();
            EnableBloodBarrier(false);
        }
    }
}
