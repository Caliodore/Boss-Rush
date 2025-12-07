using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Cali7
{ 
    public class F7_Central : MonoBehaviour
    {
        public static F7_Central Instance;

        [Header("Vars Used Externally and Internally")]
        public bool isEnraged = false;
        public bool isMoving = false;
        public bool playerInMelee = false;

        public float distToPlayer;

        [Header("Vars Used Internally")]
        public int currentCombo;
        public int currentHitsTaken;
        public int hitsTakenRecently;

        private float hitsElapsed;
        private float comboElapsed;

        Coroutine comboCoro;
        Coroutine punishCoro;

        
//------[ Unity-Inherent Methods ]-----------------------------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            CheckIfMoving();
            CheckIfMelee();
        }


//------[ Determinant Methods ]--------------------------------------------------------------------------------------------------------------------------------------

        public void StateChangeRequest() { 
            F7_RefManager.BSTM.ChangeState(F7_RefManager.BSDR.DetermineNextState());
        }

        public void MeleeAttackDecider() { }
        public void RangedAttackDecider() { }
        public void PunishTypeDecider() { }

        
//------[ Checking Methods ]-----------------------------------------------------------------------------------------------------------------------------------------

        public void CheckHitsTakenRecently() { if(punishCoro == null) punishCoro = StartCoroutine(HitsTakenTimer()); hitsElapsed = 0; hitsTakenRecently++; }
        public void CheckCombo() { if(comboCoro == null) comboCoro = StartCoroutine(ComboTimer()); comboElapsed = 0; currentCombo++; }

        private void CheckIfMoving() { 
            if(F7_RefManager.BSTM.CurrentState == F7_RefManager.BSTC) { 
                isMoving = true;
                F7_RefManager.BEVM.OnStartMoving?.Invoke();
            }
            else { 
                isMoving = false;
                F7_RefManager.BEVM.OnStopMoving?.Invoke();
            }
        }

        private void CheckIfMelee() {
            Vector3 bossFlatY = F7_RefManager.BGOJ.transform.position;
            Vector3 playerFlatY = F7_RefManager.PLGS.gameObject.transform.position;

            playerFlatY.y = 0;
            bossFlatY.y = 0;

            distToPlayer = Vector3.Distance(playerFlatY,bossFlatY);

            if(distToPlayer < F7_RefManager.BPSO.meleeRange) { 
                if(!playerInMelee)
                    F7_RefManager.BEVM.OnPlayerEnterMelee?.Invoke();
                playerInMelee = true;
            }
            else { 
                if(playerInMelee)
                    F7_RefManager.BEVM.OnPlayerExitMelee?.Invoke();
                playerInMelee = false;
            }
        }


//------[ Attack Methods ]-------------------------------------------------------------------------------------------------------------------------------------------




        
//------[ Coroutines and Timers ]------------------------------------------------------------------------------------------------------------------------------------

        

        IEnumerator HitsTakenTimer() { 
            while(hitsElapsed < F7_RefManager.BPSO.punishDecayTime) {
                hitsElapsed += Time.deltaTime;
                if(hitsTakenRecently >= F7_RefManager.BPSO.hitsUntilPunish) { 
                    F7_RefManager.BEVM.OnStartPunish?.Invoke();
                    hitsTakenRecently = 0;
                    yield break;
                }
                yield return null;
            }
            if(hitsElapsed >= F7_RefManager.BPSO.punishDecayTime) { 
                hitsTakenRecently = 0;
                yield break;
            }
            yield return null;
        }

        IEnumerator ComboTimer() { 
            while(comboElapsed < F7_RefManager.BPSO.comboDecayTime) {
                comboElapsed += Time.deltaTime;
                if(currentCombo >= F7_RefManager.BPSO.maxCombo) {
                    F7_RefManager.BEVM.OnReachMaxCombo?.Invoke();
                    currentCombo = 0;
                    yield break;
                }
                yield return null;
            }
            if(comboElapsed >= F7_RefManager.BPSO.comboDecayTime) {
                currentCombo = 0;
            }
            yield return null;
        }

        
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------

    }
}
