using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

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

        [Header("Logic Vars")]
        public bool playerInMelee;
        public bool canAttack;
        private Coroutine comboCoro;

        [Header("Complicated Shit")]
        private float comboTimer = 0f;
        public float comboDecayTime = 0f;
        public int comboCounter = 0;
        public bool isComboing = false;
        public float meleeCooldown;
        public float rangedCooldown;

        private void Start()
        {
            var GUH = GetComponentsInChildren<JRState>();
            foreach(JRState bluh in GUH)
                BossStates.Add(bluh);
        }

        private void Update()
        {
            
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

        public void UpdateHealth()
        { 
            
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
    }
}
