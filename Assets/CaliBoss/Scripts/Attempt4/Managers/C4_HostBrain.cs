using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.AI;
using System;
using DG.Tweening;

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
        public static GameObject StateParentObj;
        public static GameObject DamageParentObj;

        [Header("Logic Vars")]
        protected int currentPhase;
        protected int currentHealth;
        protected int maxHealth;

        [Header("Gameplay Events")]
        public UnityEvent OnBossAlerted;

        [Header("Structuring Events")]
        public UnityEvent OnInspectorInterfaceValidated;
        public UnityEvent OnHostBrainValidated;
        public UnityEvent OnAllRefsAssigned;

        [Header("Exposed Fields")]
        public float TurnSpeed { get { return turnSpeed; } }

        [Header("Backing Fields")]
        private float turnSpeed;

        private void Awake()
        {
            //On awake, needs to collect references, then help each script set their references to make sure no null references occur.
            //Might make some helper functions for this that Brain can call per script.
            if(Buster == null)
                Buster = this;
        }

        private void Start()
        {
            TurnToPlayer();
        }
        
//----------------------------------------------------------------------------------------------------------------------------------------------
//      Internal Methods
        /*
         * Methods generally intended for use within this script or for other scripts handling exchanging/updating info and references.
         */

        

//----------------------------------------------------------------------------------------------------------------------------------------------
//      Player-Focused Methods
        /*
         * Methods focused around interactions with the player, and the variants per type of interactions.
         * To be called by BossSM by states and their sub-states.
         */
//      => Movement
        public void WalkTo() { }
        public void LeapAt() { }
        public void DashTo() { }
//      => Targeting
        public void TurnToPlayer(){
            transform.DOLookAt(PlayerObj.transform.position, turnSpeed);
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

    }
}
