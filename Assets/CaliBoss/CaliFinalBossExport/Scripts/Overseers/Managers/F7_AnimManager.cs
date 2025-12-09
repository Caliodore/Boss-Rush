using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace Cali7
{ 
    public class F7_AnimManager : MonoBehaviour
    {
        public bool printDebugLogs = true;
        public static F7_AnimManager Instance;
        private Animator bossAnim;

        private Transform bossEmptyTransform;
        private Transform animatorTransform;
        private Transform playerTransform;

        private Vector3 dirToPlayer;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        private void Start()
        {
            bossAnim = F7_RefManager.BAIM;
            bossEmptyTransform = F7_RefManager.BGOJ.transform;
            animatorTransform = bossAnim.gameObject.transform;
            playerTransform = F7_RefManager.PLGS.gameObject.transform;
            SetEvents();
        }

        private void Update()
        {
            
        }

        private void SetEvents() { 
            F7_RefManager.BEVM.OnStartMoving?.AddListener(() => StartMoving());
            F7_RefManager.BEVM.OnStopMoving?.AddListener(() => StopMoving());
            F7_RefManager.BEVM.OnSwipeStart?.AddListener(() => StartSwipe());
            F7_RefManager.BEVM.OnSlamStart?.AddListener(() => StartSlam());
            F7_RefManager.BEVM.OnShardStart?.AddListener(() => StartShard());
            F7_RefManager.BEVM.OnPillarStart?.AddListener(() => PillarStart());
            F7_RefManager.BEVM.OnRingStart?.AddListener(() => RingStart());
            F7_RefManager.BEVM.OnRecoveryStart?.AddListener(empty => StartRecovery());
            F7_RefManager.BEVM.OnEnrageStart?.AddListener(() => StartEnraged());
            F7_RefManager.BEVM.OnStartAttack?.AddListener(() => AttackStart());
            F7_RefManager.BEVM.OnArenaEntered?.AddListener(() => StartFight());
            F7_RefManager.BEVM.OnForceIdle?.AddListener(() => IdleForced());
        }

        public void StartFight() { SetATrigger("StartEncounter"); PrintTriggerSet("StartEncounter"); }
        public void StartMoving() { SetABool("IsWalking", true); }
        public void StopMoving() { SetABool("IsWalking", false); }
        public void StartSwipe() { SetATrigger("SwipeStart"); PrintTriggerSet("SwipeStart"); }
        public void StartSlam() { SetATrigger("SlamStart"); PrintTriggerSet("SlamStart"); }
        public void StartEnraged() { SetATrigger("EnrageStart"); PrintTriggerSet("EnrageStart"); }
        public void StartRecovery() { SetATrigger("RecoveryStart"); PrintTriggerSet("RecoveryStart"); }
        public void StartShard() { SetATrigger("ShardStart"); PrintTriggerSet("ShardStart"); }
        public void PillarStart() { SetATrigger("PillarStart"); PrintTriggerSet("PillarStart"); }
        public void RingStart() { SetATrigger("RingStart"); PrintTriggerSet("RingStart"); }
        public void AttackStart() { SetATrigger("StartAttack"); PrintTriggerSet("StartAttack"); }
        public void EndOfAnimationLine() { SetATrigger("FinishAnimationLine"); PrintTriggerSet("FinishAnimationLine"); }
        public void IdleForced() { SetATrigger("ForceIdle"); PrintTriggerSet("ForceIdle"); }

        private void SetABool(string boolName, bool boolState) { bossAnim.SetBool(boolName, boolState); }
        private void SetATrigger(string trigName) { bossAnim.SetTrigger(trigName); }

        private void PrintTriggerSet(string trigName) { F7_Help.DebugPrint(printDebugLogs, $"{trigName} trigger ON."); }
        
    }
}
