using Cali6;
using DG.Tweening;
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

        public bool printDebugLogs = true;
        public bool refsSet = false;

        [Header("Vars Used Externally and Internally")]
        public bool isEnraged = false;
        public bool isMoving = false;
        public bool playerInMelee = false;
        public bool encounterStarted = false;

        public float distToPlayer;

        public int currentHealth;

        [Header("Vars Used Internally")]
        public int currentCombo = 0;
        public int currentHitsTaken = 0;
        public int hitsTakenRecently = 0;
        private List<F7_PillarScript> pillarScripts;
        public bool settingShards;
        public bool firingShards;
        private bool raisingPillars, loweringPillars;

        private float hitsElapsed;
        private float comboElapsed;

        Coroutine comboCoro;
        Coroutine punishCoro;

        
//------[ Unity-Inherent Methods ]-----------------------------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            pillarScripts = new();
        }

        private void Start()
        {
            if(!F7_RefManager.Instance.gotRefs)
                F7_RefManager.OnRefsLoaded?.AddListener(() => SetReferences());
            else
                SetReferences();
        }

        public void SetReferences() { 
            SetEvents();
            GetPillarScripts();
            currentHealth = F7_RefManager.BPSO.maxHealth;
        }

        private void Update()
        {
            if(refsSet) { 
                CheckIfMoving();
                CheckIfMelee();
            }
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
            distToPlayer = DistToPlayerCalc();

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

        public float DistToPlayerCalc() { 
            Vector3 bossFlatY = F7_RefManager.BGOJ.transform.position;
            Vector3 playerFlatY = F7_RefManager.PLGS.gameObject.transform.position;

            playerFlatY.y = 0;
            bossFlatY.y = 0;

            return Vector3.Distance(playerFlatY,bossFlatY);
        }

        public Vector3 GetDirToPlayer() { 
            return (F7_RefManager.PLGS.gameObject.transform.position - F7_RefManager.BGOJ.transform.position).normalized;
        }

        private void GetPillarScripts() {
            foreach(GameObject currentPillar in F7_RefManager.GOPO) { 
                F7_PillarScript currentScript = currentPillar.GetComponent<F7_PillarScript>();
                pillarScripts.Add(currentScript);
            }
            F7_Help.DebugPrint(printDebugLogs,$"Stored {pillarScripts.Count} pillar scripts.");
        }
        
//------[ Action Methods ]------------------------------------------------------------------------------------------------------------------------------------------

        public void SlamAttackPhys() { 
            F7_Help.DebugPrint(printDebugLogs, "Central SlamAttack");
        }

        public void SwipeAttackPhys() { 
            F7_Help.DebugPrint(printDebugLogs, "Central SwipeAttack");
        }

        public void ComboFinisherPhys() { 
            F7_Help.DebugPrint(printDebugLogs, "Central ComboFinisher");
        }

        public void ShardSprayPhys() { 
            F7_Help.DebugPrint(printDebugLogs, "Central ShardSpray");
            //StartCoroutine(SetupShards());
            StartCoroutine(SendShards());
        }

        public void PillarHandlerPhys() { 
            F7_Help.DebugPrint(printDebugLogs, "Central PillarRise");
            for(int i = 0; i < F7_RefManager.BPSO.numberOfPillars; i++) { 
                Vector3 vecOut = F7_RefManager.PLGS.gameObject.transform.position;
                float randX = UnityEngine.Random.Range(-15f,15f);
                float randY = -9;
                float randZ = UnityEngine.Random.Range(-15f,15f);
                vecOut += new Vector3(randX, randY, randZ);
                F7_PillarHolder.Instance.SetPillarPos(i, vecOut);
            }
            F7_PillarHolder.Instance.ActivatePillars();
        }

        public void RaiseRingPhys() { 
            F7_Help.DebugPrint(printDebugLogs, "Central RaiseRing");
            F7_RefManager.GORR.transform.position = F7_RefManager.GORS.transform.position;
            StartCoroutine(RingTimer());
        }

        public void LowerRingPhys() { 
            F7_Help.DebugPrint(printDebugLogs, "Central LowerRing");
            F7_RefManager.GORR.transform.DOMoveY(F7_RefManager.GORE.transform.position.y, 6f);
        }

        public void BloodBarrierPhys() { 
            F7_Help.DebugPrint(printDebugLogs, "Central BloodBarrier");
            F7_RefManager.GOBB.GetComponent<F7_BloodBarrier>().ActivateBarrier();
            F7_RefManager.BDGL.gameObject.SetActive(false);
        }

        public void AoEPunishPhys() { 
            F7_Help.DebugPrint(printDebugLogs, "Central AoEPunish");
        }

        public void EnragedModePhys() { 
            F7_Help.DebugPrint(printDebugLogs, "Central EnragedMode");
        }

        public void LeapSwipePhys() { 
            F7_Help.DebugPrint(printDebugLogs, "Central LeapSwipe");
        }

        public void BloodWallPhys() { 
            F7_Help.DebugPrint(printDebugLogs, "Central BloodWall");
            Vector3 dirToPlayer = GetDirToPlayer();
            Vector3 halfwayPoint = dirToPlayer * (DistToPlayerCalc()/2);
            if(Physics.Raycast(halfwayPoint, Vector3.down, out RaycastHit hitOut, 20f, LayerMask.GetMask("Environment"))) { 
                F7_RefManager.GOBB.transform.position = hitOut.point;
                transform.forward = dirToPlayer;
            }
            else
                F7_Help.DebugPrint(printDebugLogs, "BloodWall can't find a halfway spot between boss and player.");
        }

        //public void DetermineRecoveryType(int recType) { F7_RefManager.BSTR.recoveryType = recType; }

        //public void BarrierBrokenPhys() { }
        //public void ReelingBackPhys() { }
        //public void EnragedExitPhys() { }


//------[ Event Methods ]-------------------------------------------------------------------------------------------------------------------------------------------

        private void SetEvents() { 
            F7_EventManager.Instance.OnBossTakesDamage?.AddListener(dmgDone => TakeDamage(dmgDone));
            F7_EventManager.Instance.OnStartAttack?.AddListener(() => CheckCombo());

            F7_EventManager.Instance.OnSlamStart?.AddListener(() => SlamAttackPhys());
            F7_EventManager.Instance.OnSwipeStart?.AddListener(() => SwipeAttackPhys());
            F7_EventManager.Instance.OnReachMaxCombo?.AddListener(() => ComboFinisherPhys());

            F7_EventManager.Instance.OnShardsReady?.AddListener(() => ShardSprayPhys());
            F7_EventManager.Instance.OnPillarStart?.AddListener(() => PillarHandlerPhys());
            F7_EventManager.Instance.OnRingStart?.AddListener(() => RaiseRingPhys());

            F7_EventManager.Instance.OnBarrierStart?.AddListener(() => BloodBarrierPhys());
            F7_EventManager.Instance.OnAoEStart?.AddListener(() => AoEPunishPhys());
            F7_EventManager.Instance.OnEnrageStart?.AddListener(() => EnragedModePhys());

            F7_EventManager.Instance.OnLeapSwipeStart?.AddListener(() => LeapSwipePhys());
            F7_EventManager.Instance.OnWallStart?.AddListener(() => BloodWallPhys());

            //F7_EventManager.Instance.OnRecoveryStart?.AddListener(typeIn => DetermineRecoveryType(typeIn));
        }

        public void TakeDamage(int dmgTaken) { currentHealth -= dmgTaken; CheckHealthAndPhase(); }


        
//------[ Coroutines and Timers ]------------------------------------------------------------------------------------------------------------------------------------

        IEnumerator RingTimer() { 
            bool raisingRing = true;
            F7_RefManager.GORR.transform.DOMoveY(F7_RefManager.GORE.transform.position.y, 8f);
            while(raisingRing) { 
                float distToDest = Vector3.Distance(F7_RefManager.GORR.transform.position, F7_RefManager.GORE.transform.position);
                if(distToDest <= 0.1f) { 
                    raisingRing = false;
                }
                yield return null;
            }
            yield return new WaitForSeconds(18f);
            LowerRingPhys();
        }

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

        IEnumerator SendShards() { 
            firingShards = true;
            int shardInd = 0;
            while(firingShards) {
                F7_RefManager.GOSA[shardInd].GetComponent<F7_SIP>().attachedShardScript.StartMoving();
                shardInd++;
                if(shardInd >= F7_RefManager.GOSA.Count) { 
                    firingShards = false;
                    yield break;
                }
                yield return new WaitForSeconds(F7_RefManager.BPSO.shardFirerate);
            }
        }
        
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------

    }
}
