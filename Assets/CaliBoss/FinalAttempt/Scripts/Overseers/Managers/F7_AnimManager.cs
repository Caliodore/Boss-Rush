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
        }

        public void StartMoving() { SetABool("IsWalking", true); }
        public void StopMoving() { SetABool("IsWalking", false); }

        private void SetABool(string boolName, bool boolState) { bossAnim.SetBool(boolName, boolState); }
        private void SetATrigger(string trigName) { bossAnim.SetTrigger(trigName); }
        
    }
}
