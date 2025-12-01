using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CaliBoss
{ 
    public class BossBrainBase : MonoBehaviour
    {
//----------------------------------------------------------------------------------------
//  Public Fields
//      |>Bools
        public bool IsMoving { get { return isMoving; } }
        public bool IsAlerted { get { return isAlerted; } }
        public bool IsAttacking { get { return isAttacking; } }
        public bool CanTurn { get { return canTurn; } }
        public bool CanAttack { get { return canAttack; } }
        public bool CanRanged { get { return canRanged; } }
        public bool CanLeap { get { return canLeap; } }
        public bool CanMove { get { return canMove; } }

//      |>Floats
        public float ComboElapsed { get { return comboElapsed; } }
        public float PunishElapsed {  get { return punishElapsed; } }

//      |>Ints
        public int ComboCounter { get { return comboCounter; } }
        public int CurrentHealth { get { return currentHealth; } }
        public int HitsTakenRecently { get  { return hitsTakenRecently; } }

//----------------------------------------------------------------------------------------
//  Backing Fields
//      |>Bools
        private bool isMoving = false;
        private bool isAlerted = false;
        private bool isAttacking = false;
        private bool canTurn = true;
        private bool canAttack = false;
        private bool canRanged = false;
        private bool canLeap = false;
        private bool canMove = false;

//      |>Floats
        private float comboElapsed = 0f;
        private float punishElapsed = 0f;

//      |>Ints
        private int comboCounter = 0;
        private int currentHealth = 20;
        private int hitsTakenRecently = 0;
        
//----------------------------------------------------------------------------------------
//  Internals
        Coroutine punishmentCoro;
        Coroutine comboCoro;


        [SerializeField] BossTimers_SO TimerRef;
        [SerializeField] BossPhysical_SO PhysRef;


//----------------------------------------------------------------------------------------

        public BossBrainBase() { }
        public static BossBrainBase MainBrain;

        private void Awake()
        {
            if(MainBrain == null)
                MainBrain = this;
            currentHealth = PhysRef.maxHealth;
        }

        private void Update()
        {
            TurnTowardsPlayer();
        }
//----------------------------------------------------------------------------------------
//  Internal Methods
        private void SetupAttacks() { 
            
        }


//----------------------------------------------------------------------------------------
//  Event Hooks

        public void OnStateChange() { }
        public void OnTakingHit() { 
            AddToPunish();
        }

        public void BossSuccessfulHit() { 
            AddToCombo();    
        }
        
//----------------------------------------------------------------------------------------
//  Physical Manipulation

        public void MoveTo(Vector3 targetLocation) { 
            CSR.Instance.BossNMAgent.SetDestination(targetLocation);
        }

        private void TurnTowardsPlayer() { 
            if(canTurn)
                transform.DORotate((CSR.Instance.PlayerScript.gameObject.transform.position - gameObject.transform.position), PhysRef.turnSpeed);
        }
        
        private void SnapToPlayer() { 
            Vector3 directionVec = CSR.Instance.PlayerScript.gameObject.transform.position - transform.position;
            directionVec = new Vector3(directionVec.x, 0, directionVec.z);
            transform.forward = directionVec.normalized;
        }

//----------------------------------------------------------------------------------------
//  Var Updates
        
        public float GetDistanceToPlayer() {
            Vector3 bossFlatVec = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 playerFlatVec = new Vector3(CSR.Instance.PlayerScript.gameObject.transform.position.x, 0, CSR.Instance.PlayerScript.gameObject.transform.position.z);
            return Vector3.Distance(bossFlatVec, playerFlatVec);
        }

        private void AddToCombo() { 
            comboCounter++;
            if(comboCounter > PhysRef.maxCombo)
                comboCounter = 1;
            if(comboCoro == null)
                comboCoro = StartCoroutine(ComboTimer());
        }

        private void AddToPunish() {
            hitsTakenRecently++;
            punishElapsed = 0f;
            if(punishmentCoro == null)
                punishmentCoro = StartCoroutine(PunishmentTimer());
        }

        public Damage GetDamage()
        { 
            Damage dmgOut = new();
            dmgOut.amount = PhysRef.damageDealt;
            dmgOut.knockbackForce = PhysRef.knockbackDealt;
            dmgOut.direction = transform.forward;
            return dmgOut;
        }

        
//----------------------------------------------------------------------------------------
//  State-Centric
        
        public void RebukeTriggeredDeterminant() { 
            int randInt = UnityEngine.Random.Range(0, 100);
            bool isMelee = GetDistanceToPlayer() <= PhysRef.meleeRange;

            if(isMelee) { 
                if(randInt < 50)
                    StartAoEPunish();
                else
                    StartBloodBarrier();
            }
            else { 
                if(randInt < 50)
                    StartLeapAttack();
                else
                    StartBloodBarrier();
            }
        }

        public void DetermineAttackType() { 
            bool isMelee = GetDistanceToPlayer() <= PhysRef.meleeRange;

            if(isMelee) { 
                
            }
            else {
                
            }
        }
        
//----------------------------------------------------------------------------------------
//  Specific Action Methods
//  Issue: Cooldowns for moves
//  Solution?: Use coroutines for different types of interactions that these have to go through.
//          >Or only allow them to be called if canAttack and other wider bools are true.
//          >Have these be protected maybe? So others can use them in a delegate for a TryAttack function?
//          >Or simply have the brain/another script be the determinant of attacks, but not as complicated as BossActionManager.
//  SOLUTION!: Have these only called at the start of a state, so nothing will be spammed by an update.
//          >The updates can then be used to keep an eye for things like when the blood barrier would break and trigger it then.
//          >Ex: Defending has a coroutine for BloodBarrier that has a listener when its off cooldown, removed when called, then added back by timer coro.
//      |>Attacks
        public void StartRegularSwipe() { 
            
        }
        public void StartRegularSlam() { 
            
        }
        public void StartComboMixup() { 
            
        }
        public void StartComboFinisher() { 
            
        }
        public void StartShardSpray() { 
            
        }
        public void StartPillarsAttack() { 
            
        }
        public void StartClosingRing() { 
            
        }
        public void StartLeapAttack() { 
            
        }

//      |> Defenses/Punishments
        public void StartBloodBarrier() { 
            
        }
        public void StartBloodWall() { 
            
        }
        public void StartEnragedMode() { 
            
        }
        public void StartAoEPunish() { 
            
        }

//      |> Movement
        public void StartLeapMove() { 
            
        }
        public void StartDashMove() { 
            
        }

//      |> Recovery
        public void StartReelingBackRecovery() { 
            
        }
        public void StartBrokenBarrierDazeRecovery() { 
            
        }
        public void StartEnragedExitRecovery() { 
            
        }
        

        
//----------------------------------------------------------------------------------------
//  Coroutines
        IEnumerator PunishmentTimer() {
            punishElapsed = 0f;
            while(punishElapsed < TimerRef.punishDecayTimer)
            { 
                if(hitsTakenRecently >= PhysRef.hitsUntilRebuke)
                { 
                    StartEnragedMode();
                    yield break;
                }
                yield return null;    
            }
            hitsTakenRecently = 0;
            yield return null;
        }

        IEnumerator ComboTimer() { 
            comboElapsed = 0f;
            while(comboElapsed < TimerRef.comboDecayTimer)
            { 
                if(comboCounter == 3)
                { 
                    StartComboMixup();
                }
                else if(comboCounter >= PhysRef.maxCombo)
                { 
                    StartComboFinisher();
                    comboCounter = 0;
                    yield break;
                }
                yield return null;
            }
            comboCounter = 0;
            yield return null;    
        }
    }
}
