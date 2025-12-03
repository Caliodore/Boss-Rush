using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Cali6
{ 
    public class A6_Brain : MonoBehaviour
    {
        [Header("Serialized Refs")]
        [Header("Cali Components")]
        [SerializeField] A6_StateMachine BossSM;
        //[SerializeField] 

        [Header("Cali States")]
        [SerializeField] public A6_Attacking AttackingState;
        [SerializeField] public A6_Defending DefendingState;
        [SerializeField] public A6_Idling IdlingState;
        [SerializeField] public A6_Pursuing PursuingState;
        [SerializeField] public A6_Recovering RecoveringState;

        [Header("Brolive Components")]


        [Header("Base Unity Components")]
        [SerializeField] public Animator BossAnimator;
        [SerializeField] public Collider BossMeleeSensorCollider;
        [SerializeField] public Collider BossDamagerCollider;

        public UnityEvent OnStartingAttack;

        public int currentHealth;

        public bool playerInMelee;
        public bool bossCanAttack;

        public static A6_Brain Instance;
        public A6_Brain() { }

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            OnStartingAttack ??= new();
        }

        private void Start()
        {
            
        }

        private void DamageTracking() { }
    }
}
