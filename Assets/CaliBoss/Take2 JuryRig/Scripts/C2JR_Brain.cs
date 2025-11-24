using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace CaliJR
{ 
    public class C2JR_Brain : MonoBehaviour
    {
        [Header("Component Refs")]
        public static C2JR_Brain BossBrain;
        public static C2JR_StateMachine BossSM;
        public static Navigator BossNavigator;
        public static NavMeshAgent BossNavMeshAgent;
        public static Damager BossDamager;
        public static Damageable BossDamageable;
        public static Actor BossActor;
        public static Sensor BossSensor;
        public static List<JRState> BossStates;

        [Header("Object Refs")]
        public GameObject playerObj;
        public GameObject barrierPrefab;
        public GameObject projectilePrefab;

        [Header("Physical Vars")]
        public float moveSpeed;
        public float totalHealth;
        public float currentHealth;
        public float attackCooldown;
        public float turnSpeed;
        public float damageDealt;
        public float distanceToPlayer;
        public float meleeRange;

        [Header("Logic Vars")]
        public bool playerInMelee = false;
        public bool canAttack = true;
        public bool pursuingPlayer = false;
        public bool canMove = true;
        private Coroutine comboCoro;
        private Coroutine recentHitCoro;
        public UnityEvent OnPlayerEntersMelee;
        public UnityEvent OnPlayerExitsMelee;
        public UnityEvent<Damage> OnHitPlayerSuccess;
        public UnityEvent OnDamagedByPlayer;

        [Header("Complicated Stuff")]
        //Defense
        public int hitsUntilRetaliation = 5;
        public int hitsTakenRecently = 0;
        public int hitsUntilBarrierBreak = 5;
        public float hitDecayTime = 1.5f;
        public float defenseIncreaseRate = 0.15f;

        //Aggression
        private float comboTimer = 0f;
        public float comboDecayTime = 0f;
        public int comboCounter = 0;
        public bool isComboing = false;
        public float meleeCooldown;
        public float rangedCooldown;

        //Damage Variants
        private Damage clergyDefaultDmg;
        private Damage normalHostDmg;
        private Damage enragedHostDmg;
        private Damage bloodPillarDmg;
        private Damage bloodShardDmg;

        private void Start()
        {
            var GUH = GetComponentsInChildren<JRState>();
            foreach(JRState bluh in GUH)
                BossStates.Add(bluh);

            BossSensor.OnEnter.AddListener(() => ToggleMeleeBool(true));
            BossSensor.OnExit.AddListener(() => ToggleMeleeBool(false));
            BossDamageable.OnHit.AddListener(dmgIn => DamagedByPlayer(dmgIn));
        }

        private void Update()
        {
            
        }

        public void PursuePlayer()
        {
            canMove = canMove && (moveSpeed > 0);
            if(canMove)
            { 
                Vector3 playerFlatY = new Vector3(playerObj.transform.position.x, 0, playerObj.transform.position.z);
                bool pathValid = BossNavigator.CalculatePathToPosition(playerFlatY);
                if(pathValid)
                { 
                    BossNavMeshAgent.SetDestination(playerFlatY);
                }
                else
                { 
                    BossNavMeshAgent.FindClosestEdge(out NavMeshHit closeEdge);
                    BossNavMeshAgent.SetDestination(closeEdge.position);
                }
                pursuingPlayer = true;
            }
            else
            { 
                print("Unable to move at the current time.");
                pursuingPlayer = false;
            }
        }

        private void ToggleMeleeBool(bool toggleMode)
        {
            bool meleeDistCheck = CheckIfInMelee();
            if((meleeDistCheck && toggleMode) || (!toggleMode && meleeDistCheck) || (toggleMode && !meleeDistCheck))
            { 
                playerInMelee = true;
                OnPlayerEntersMelee.Invoke();
            }
            else
            { 
                playerInMelee = false;
                OnPlayerExitsMelee.Invoke();
            }
        }

        public void TryAttack()
        { 
            if(canAttack)
            { 
                switch(comboCounter)
                { 
                    case(0):
                    case(1):
                    case(2):
                        RegularAttack();
                        break;

                    case(3):
                    case(4):
                        ComboAttack();
                        break;

                    case(5):
                        FinisherAttack();
                        break;
                }
            }
        }

        public void RangedAttack()
        { 
            
        }

        public void RegularAttack()
        { 
            
        }

        public void ComboAttack()
        { 
            
        }

        public void FinisherAttack()
        { 
            
        }

        public void UpdateHealth(int dmgAmount, int newHealth)
        {
            currentHealth = (float)newHealth;
            float percentLost = (dmgAmount / totalHealth);
            //Call UIManager method to update health bar.
        }

        IEnumerator AttackCooldown(float attackTimer)
        { 
            yield return null;    
        }

        public void ComboCheck()
        { 
            comboTimer = 0f;
            if(!isComboing && comboCoro == null)
            { 
                isComboing = true;
                comboCoro = StartCoroutine(ComboTimer());
            }
            else if(isComboing)
            {
                comboCounter = (comboCounter + 1) % 5;
            }
        }

        IEnumerator ComboTimer()
        {
            while(isComboing)
            {
                comboTimer += Time.deltaTime;
                if(comboTimer >= comboDecayTime)
                { 
                    comboCounter = 0;
                    isComboing = false;
                }
                yield return null;    
            }
            yield return null;    
        }

        public bool CheckIfInMelee()
        { 
            distanceToPlayer = Vector3.Distance(transform.position, playerObj.transform.position);
            
            if(distanceToPlayer <= meleeRange)
                return true;
            else
                return false;
        }

        /*
         * Not entirely sure if I need this right now but was a thought that popped into my head that I might want.
         */
        public void DetermineAttackVariant()
        {
            playerInMelee = CheckIfInMelee();
            switch(BossSM.currentState.ToString())
            { 
                case("Attacking"):
                    if(playerInMelee)
                    { 
                        
                    }
                    break;

                case("Pursue"):
                    if(playerInMelee)
                    { 
                        
                    }

                    break;

                case("Defending"):
                    if(playerInMelee)
                    { 
                        
                    }

                    break;

                default:
                    
                    break;
            }
        }

        /*
         * All-purpose methods for communicating back and forth with states when damaged by a player.
         *              --- Non-Defending Flow ---
         *              (Using Idle as reference)
         * OnDamagedByPlayer has a Listener for the current state added during OnStateEnter()
         * When OnDamagedByPlayer is invoked, ChangeState is called to go to Attacking.
         * Simultaneously, BossDamager.OnHit should be invoked, calling DamagedByPlayer.
         * DamagedByPlayer feeds the damage to the currentState.DamageTaken method.
         * currentState.ModifyIncomingDamage returns a new Damage directly into TakeHit.
         * TakeHit applies the damage through UpdateHealth and 
         * 
         */

        public void DamagedByPlayer(Damage incomingDamage)
        { 
            if(recentHitCoro == null)
                StartHitTimer();
            BossSM.currentState.ModifyIncomingDamageByState(incomingDamage);
        }

        public void TakeHit(float healthDamage)
        {
            hitsTakenRecently++;
            hitsTakenRecently %= 5;

        }

        public void DamageBloodBarrier(float dmgIn)
        { 
            
        }

        public void ShatterBloodBarrier()
        { 
            hitsTakenRecently = 0;
            //Enable expanding sphere collider with Damager script for a half second 
        }

        public void StartHitTimer()
        { 
            recentHitCoro = StartCoroutine(RecentlyHitTimer());
        }

        IEnumerator RecentlyHitTimer()
        {
            float timeSinceLastHit = 0f;
            hitsTakenRecently = 0;
            while(gameObject.activeSelf == true)
            {
                timeSinceLastHit += Time.deltaTime;
                if((timeSinceLastHit > hitDecayTime) && (hitsTakenRecently != 0))
                { 
                    hitsTakenRecently = 0;
                    timeSinceLastHit = 0f;
                    yield break;
                }
                else if(hitsTakenRecently >= hitsUntilRetaliation)
                { 
                    
                }
                yield return null;
            }
        }
    }
}
