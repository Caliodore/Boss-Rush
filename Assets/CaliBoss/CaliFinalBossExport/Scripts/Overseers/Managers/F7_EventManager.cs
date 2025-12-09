using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Cali7
{ 
    public class F7_EventManager : MonoBehaviour
    {
        public static F7_EventManager Instance;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            NullCheckEvents();
        }

        [Header("Logic Events")]
        public UnityEvent OnReachMaxCombo;
        public UnityEvent OnStartPunish;
        public UnityEvent OnPlayerEnterMelee;
        public UnityEvent OnPlayerExitMelee;
        public UnityEvent OnChangePhase;

        [Header("Combat Events")]
        public UnityEvent<int> OnBossTakesDamage;
        public UnityEvent OnBarrierStart;
        public UnityEvent OnAoEStart;
        public UnityEvent OnLeapSwipeStart;
        public UnityEvent OnWallStart;

        [Header("Animation-Focused Events")]
        public UnityEvent OnSwipeStart;
        public UnityEvent OnSlamStart;
        public UnityEvent OnShardStart;
        public UnityEvent OnPillarStart;
        public UnityEvent OnRingStart;

        public UnityEvent OnEnrageStart;
        public UnityEvent OnStartAttack;
        
        public UnityEvent<int> OnRecoveryStart;
        public UnityEvent OnStartMoving;
        public UnityEvent OnStopMoving;
        public UnityEvent OnArenaEntered;
        public UnityEvent OnForceIdle;

        private void Start()
        {
            F7_RefManager.BDGL.OnHit?.AddListener(dmgIn => OnBossTakesDamage?.Invoke(dmgIn.amount));
        }

        private void NullCheckEvents() {
            Instance.OnReachMaxCombo ??= new();
            OnStartPunish ??= new();
            OnPlayerEnterMelee ??= new();
            OnPlayerExitMelee ??= new();
            OnChangePhase ??= new();

            OnBossTakesDamage ??= new();
            OnBarrierStart ??= new();
            OnAoEStart ??= new();
            OnLeapSwipeStart ??= new();
            OnWallStart ??= new();
            
            OnSwipeStart ??= new();
            OnSlamStart ??= new();
            OnShardStart ??= new();
            OnPillarStart ??= new();
            OnRingStart ??= new();
            
            OnEnrageStart ??= new();
            OnStartAttack ??= new();

            OnRecoveryStart ??= new();
            OnStartMoving ??= new();
            OnStopMoving ??= new();
            OnArenaEntered ??= new();
            OnForceIdle ??= new();
        }
    }
}
