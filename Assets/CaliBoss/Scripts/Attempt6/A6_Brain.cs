using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

namespace Cali6
{ 
    public class A6_Brain : MonoBehaviour
    {
        [Header("Serialized Refs")]
        [Header("Cali Components")]
        [SerializeField] public A6_StateMachine BossSM;
        //[SerializeField] 

        [Header("Cali States")]
        [SerializeField] public A6_Attacking AttackingState;
        [SerializeField] public A6_Defending DefendingState;
        [SerializeField] public A6_Idling IdlingState;
        [SerializeField] public A6_Pursuing PursuingState;
        [SerializeField] public A6_Recovering RecoveringState;

        [Header("Brolive Components")]
        [SerializeField] public Actor BossActor;
        [SerializeField] public Damageable BossDamageable;
        [SerializeField] public Damager BossDamager;
        [SerializeField] public Navigator BossNavigator;
        [SerializeField] public Sensor BossMeleeSensor;

        [Header("Base Unity Components")]
        [SerializeField] public Animator BossAnimator;
        [SerializeField] public Collider BossMeleeSensorCollider;
        [SerializeField] public Collider BossDamagerCollider;
        [SerializeField] public NavMeshAgent BossNMAgent;

        public UnityEvent OnStartingAttack;
        public UnityEvent OnAttackEnd;
        public UnityEvent OnStartingAnimation;
        public UnityEvent OnTakingDamage;

        public int maxHealth;
        public int currentHealth;

        public bool playerInMelee = false;
        public bool bossCanAttack = true;

        public static A6_Brain Instance;
        public A6_Brain() { }

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            OnStartingAttack ??= new();
            OnAttackEnd ??= new();
            OnStartingAnimation ??= new();
            OnTakingDamage ??= new();
        }

        private void Start()
        {
            BossDamageable.OnInitialize?.Invoke(maxHealth);
            currentHealth = maxHealth;
            SetAttackListeners();
            SetDamagedListeners();
        }

        private void SetAttackListeners() {
            OnStartingAttack?.AddListener(() => StartAttackBrain());
            OnAttackEnd?.AddListener(() => EndAttackBrain());
        }

        private void SetDamagedListeners() { 
            BossDamageable.OnHit?.AddListener(dmgIn => OnTakingDamage.Invoke());
            //BossDamageable.OnHit?.AddListener(dmgIn => StartAttackBrain());
            foreach(A6_StateBase indexState in BossSM.BossStates) { 
                OnTakingDamage?.AddListener(() => indexState?.OnDamagedDuringState());
            }
        }

        private void StartAttackBrain() { 
            bossCanAttack = false;
            print("Brain start attack.");
            AttackingState.StartAttackInState();
        }

        private void EndAttackBrain() { 
            print("Brain end attack.");
            bossCanAttack = true;    
        }
    }
}
