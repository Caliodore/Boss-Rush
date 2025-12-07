using Cali6;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Cali7
{ 
    public class F7_Central : MonoBehaviour
    {
        public static F7_Central Instance;

        [Header("Vars Used Externally and Internally")]
        public bool isEnraged = false;
        public bool isMoving = false;
        public bool playerInMelee = false;
        public bool encounterStarted = false;

        public float distToPlayer;

        public int currentHealth;

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
            SetEvents();
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
        
//------[ Checking Methods ]-----------------------------------------------------------------------------------------------------------------------------------------

        public void CheckHitsTakenRecently() { if(punishCoro == null) punishCoro = StartCoroutine(HitsTakenTimer()); hitsElapsed = 0; hitsTakenRecently++; }
        public void CheckCombo() { if(comboCoro == null) comboCoro = StartCoroutine(ComboTimer()); comboElapsed = 0; currentCombo++; }

        public bool CheckIfComboReady() { return (currentCombo >= F7_RefManager.BPSO.maxCombo); }
        public bool CheckIfPunishReady() { return (hitsTakenRecently >= F7_RefManager.BPSO.hitsUntilPunish); }

        public void CheckHealthAndPhase() { }

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
        
//------[ Action Methods ]------------------------------------------------------------------------------------------------------------------------------------------

        public void SlamAttackPhys() { 
            
        }

        public void SwipeAttackPhys() { 
            
        }

        public void ComboFinisherPhys() { 
            
        }

        public void ShardSprayPhys() { 
            
        }

        public void PillarRisePhys() { 
            
        }

        public void RaiseRingPhys() { 
            
        }

        public void BloodBarrierPhys() { 
            
        }

        public void AoEPunishPhys() { 
            
        }

        public void EnragedModePhys() { 
            
        }

        public void LeapSwipePhys() { 
            
        }

        public void BloodWallPhys() { 
            
        }

        public void DetermineRecoveryType(int recType) { F7_RefManager.BSTR.recoveryType = recType; }

        //public void BarrierBrokenPhys() { }
        //public void ReelingBackPhys() { }
        //public void EnragedExitPhys() { }


//------[ Event Methods ]-------------------------------------------------------------------------------------------------------------------------------------------

        private void SetEvents() { 
            F7_EventManager.Instance.OnBossTakesDamage?.AddListener(dmgDone => TakeDamage(dmgDone));

            F7_EventManager.Instance.OnSlamStart?.AddListener(() => SlamAttackPhys());
            F7_EventManager.Instance.OnSwipeStart?.AddListener(() => SwipeAttackPhys());
            F7_EventManager.Instance.OnReachMaxCombo?.AddListener(() => ComboFinisherPhys());

            F7_EventManager.Instance.OnShardStart?.AddListener(() => ShardSprayPhys());
            F7_EventManager.Instance.OnPillarStart?.AddListener(() => PillarRisePhys());
            F7_EventManager.Instance.OnRingStart?.AddListener(() => RaiseRingPhys());

            F7_EventManager.Instance.OnBarrierStart?.AddListener(() => BloodBarrierPhys());
            F7_EventManager.Instance.OnAoEStart?.AddListener(() => AoEPunishPhys());
            F7_EventManager.Instance.OnEnrageStart?.AddListener(() => EnragedModePhys());

            F7_EventManager.Instance.OnLeapSwipeStart?.AddListener(() => LeapSwipePhys());
            F7_EventManager.Instance.OnWallStart?.AddListener(() => BloodWallPhys());

            F7_EventManager.Instance.OnRecoveryStart?.AddListener(typeIn => DetermineRecoveryType(typeIn));
        }

        public void TakeDamage(int dmgTaken) { currentHealth -= dmgTaken; CheckHealthAndPhase(); }


        
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
                if(CheckIfComboReady()) {
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

        IEnumerator SpawnShardCircle(int shardAmount) { 
            float randX = UnityEngine.Random.Range(-1.25f, 1.25f);
            float randY = UnityEngine.Random.Range(-1.25f, 1.25f);
            Vector3 relSpawnVec = F7_RefManager.BPSP.transform.position;
            relSpawnVec.x += randX;
            relSpawnVec.y += randY;
            GameObject thisShard = Instantiate(F7_RefManager.GOSS, relSpawnVec, Quaternion.identity);
            thisShard.transform.forward = F7_RefManager.PLGS.gameObject.transform.position - thisShard.transform.position;
            yield return null;
        }

        
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------

    }
}
