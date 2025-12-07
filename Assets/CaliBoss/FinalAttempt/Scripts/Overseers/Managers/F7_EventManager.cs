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
        }

        [Header("Logic Events")]
        public UnityEvent OnStateChangeRequested;
        public UnityEvent OnReachMidCombo;
        public UnityEvent OnReachMaxCombo;
        public UnityEvent OnStartPunish;
        public UnityEvent OnPlayerEnterMelee;
        public UnityEvent OnPlayerExitMelee;

        [Header("Combat Events")]
        public UnityEvent OnBossTakesDamage;
        public UnityEvent OnHitWhileRecovering;
        public UnityEvent OnHitWhileDefending;

        [Header("Animation-Focused Events")]
        public UnityEvent OnSwipeStart;
        public UnityEvent OnSlamStart;
        public UnityEvent OnShardStart;
        public UnityEvent OnPillarStart;
        public UnityEvent OnRecoveryStart;
        public UnityEvent OnEnrageStart;
        public UnityEvent OnStartMoving;
        public UnityEvent OnStopMoving;
        public UnityEvent OnArenaEntered;
    }
}
