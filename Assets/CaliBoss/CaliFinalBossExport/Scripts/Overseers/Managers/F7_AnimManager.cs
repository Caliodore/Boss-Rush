using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Cali7
{ 
    public class F7_AnimManager : MonoBehaviour
    {
        public static F7_AnimManager Instance;
        private Animator bossAnim;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        private void Start()
        {
            bossAnim = F7_RefManager.BAIM;
            SetEvents();
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
        }

        public void StartFight() { SetATrigger("StartEncounter"); }
        public void StartMoving() { SetABool("IsWalking", true); }
        public void StopMoving() { SetABool("IsWalking", false); }
        public void StartSwipe() { SetATrigger("SwipeStart"); }
        public void StartSlam() { SetATrigger("SlamStart"); }
        public void StartEnraged() { SetATrigger("EnrageStart"); }
        public void StartRecovery() { SetATrigger("RecoveryStart"); }
        public void StartShard() { SetATrigger("ShardStart"); }
        public void PillarStart() { SetATrigger("PillarStart"); }
        public void RingStart() { SetATrigger("RingStart"); }
        public void AttackStart() { SetATrigger("StartAttack"); }
        public void EndOfAnimationLine() { SetATrigger("FinishAnimationLine"); }

        private void SetABool(string boolName, bool boolState) { bossAnim.SetBool(boolName, boolState); }
        private void SetATrigger(string trigName) { bossAnim.SetTrigger(trigName); }
        
    }
}
