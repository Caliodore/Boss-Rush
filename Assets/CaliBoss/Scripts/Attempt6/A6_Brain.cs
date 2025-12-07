using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

namespace Cali6
{ 
    public class A6_Brain : MonoBehaviour
    {
        [Header("Serialized Refs")]
        public bool printDebugLogs = true;
        [Header("Cali Components")]
        [SerializeField] public A6_StateMachine BossSM;
        //[SerializeField] 

        [Header("Cali States")]
        [SerializeField] public A6_Attacking AttackingState;
        [SerializeField] public A6_Defending DefendingState;
        [SerializeField] public A6_Idling IdlingState;
        [SerializeField] public A6_Pursuing PursuingState;
        [SerializeField] public A6_Recovering RecoveringState;

        [Header("Brolive Components")]
        [SerializeField] public Actor BossActor;
        [SerializeField] public Damageable BossDamageable;
        [SerializeField] public Damager BossDamager;
        [SerializeField] public Navigator BossNavigator;
        [SerializeField] public Sensor BossMeleeSensor;

        [Header("Base Unity Components")]
        [SerializeField] public Animator BossAnimator;
        [SerializeField] public Collider BossMeleeSensorCollider;
        [SerializeField] public Collider BossDamagerCollider;
        [SerializeField] public NavMeshAgent BossNMAgent;

        [Header("Player-Associated Refs")]
        [SerializeField] public PlayerLogic playerScript;
        [SerializeField] public Transform playerTransform;

        public UnityEvent OnStartingAttack;
        public UnityEvent OnAttackEnd;
        public UnityEvent OnStartingAnimation;
        public UnityEvent OnTakingDamage;
        public UnityEvent OnSuccessfulHit;
        public UnityEvent OnEnragedStart;
        public UnityEvent OnEnragedEnd;

        public int maxHealth;
        public int currentHealth;
        public int currentCombo;
        public int maxCombo;
        public int hitsTakenRecently;

        public bool playerInMelee = false;
        public bool bossCanAttack = true;
        public bool isEnraged = false;
        public bool canMove = true;
        public bool canTurn = true;

        [Header("Timer Times")]
        public float comboDecayTimer = 10f;
        public float comboElapsed;
        public float hitsPunishDecayTimer = 12f;
        public float hitsPunishElapsed;
        public float timeOutOfMelee;
        public float rangedTimeUntilClose = 8f;

        [Header("Important Floats")]
        public float distToPlayer;
        public float meleeRange = 10f;
        public float bossTimeScale = 1f;
        public float bossMoveSpeed = 3f;

        private Coroutine comboCoro;
        private Coroutine punishCoro;

        public static A6_Brain Instance;
        public A6_Brain() { }
        
//-----------------------------------------------------------------------------------
//Unity-Inherent Methods

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            OnStartingAttack ??= new();
            OnAttackEnd ??= new();
            OnStartingAnimation ??= new();
            OnTakingDamage ??= new();
            OnSuccessfulHit ??= new();
            OnEnragedStart ??= new();
            OnEnragedEnd ??= new();
        }

        private void Start()
        {
            BossDamageable.OnInitialize?.Invoke(maxHealth);
            currentHealth = maxHealth;
            SetAttackListeners();
            SetDamagedListeners();
            SetOtherListeners();
        }

        private void Update()
        {
            Vector3 playerFlatY = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
            Vector3 bossFlatY = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
            distToPlayer = Vector3.Distance(playerFlatY, bossFlatY);
            if(distToPlayer <= meleeRange)
                playerInMelee = true;
            else
                playerInMelee = false;
        }
//-----------------------------------------------------------------------------------
//Event-Related Methods

        private void SetAttackListeners() {
            OnStartingAttack?.AddListener(() => StartAttackBrain());
            OnAttackEnd?.AddListener(() => EndAttackBrain());
            BossDamager.OnSuccessfulHit?.AddListener(() => UpdateCombo());
        }

        private void SetDamagedListeners() { 
            OnTakingDamage?.AddListener(() => A6_UIManager.Instance.EnableBossDisplay());
            OnTakingDamage?.AddListener(() => OnTakingDamage?.RemoveListener(() => A6_UIManager.Instance.EnableBossDisplay()));
            BossDamageable.OnHit?.AddListener(dmgIn => OnTakingDamage.Invoke());
            //BossDamageable.OnHit?.AddListener(dmgIn => StartAttackBrain());
            foreach(A6_StateBase indexState in BossSM.BossStates) { 
                OnTakingDamage?.AddListener(() => indexState?.OnDamagedDuringState());
            }
            BossDamageable.OnHit?.AddListener(dmgIn => UpdateHitsTakenRecently());
            BossDamageable.OnHealthChanged?.AddListener((dmgIn, healthResult) => HealthCheck(healthResult));
        }

        private void SetOtherListeners() { 
            OnEnragedStart?.AddListener(() => EnragedStart());
            OnEnragedEnd?.AddListener(() => EnragedEnd());
        }

        private void EnragedStart() { 
            isEnraged = true;
            A6_Help.DebugPrint(printDebugLogs, "Boss entered enraged mode.");
            bossTimeScale = 1.5f;
        }

        private void EnragedEnd() { 
            isEnraged = false;
            A6_Help.DebugPrint(printDebugLogs, "Boss exited enraged mode.");
            bossTimeScale = 1f;
        }

        private void HealthCheck(int resultHealth) { 
            currentHealth = resultHealth;
            if(currentHealth <= 0)
                BossDeath();
        }

        private void BossDeath() { 
            A6_Help.DebugPrint(printDebugLogs, "Boss dead.");
            GameManager.instance.GoToNextLevel();
        }

//-----------------------------------------------------------------------------------
//Attack-Related Methods

        private void StartAttackBrain() { 
            bossCanAttack = false;
            A6_Help.DebugPrint(printDebugLogs, "Brain start attack.");
            AttackingState.StartAttackInState();
        }

        private void EndAttackBrain() { 
            A6_Help.DebugPrint(printDebugLogs, "Brain end attack.");
            bossCanAttack = true;    
        }
//-----------------------------------------------------------------------------------
//Coroutines and Timers

        private void UpdateHitsTakenRecently() { if(punishCoro == null) punishCoro = StartCoroutine(HitsTakenTimer()); hitsTakenRecently++; }
        private void UpdateCombo() { if(comboCoro == null) comboCoro = StartCoroutine(ComboTimer()); currentCombo++; }

        IEnumerator HitsTakenTimer() { 
            hitsPunishElapsed = 0;
            while(hitsPunishElapsed < hitsPunishDecayTimer) {
                hitsPunishElapsed += Time.deltaTime * bossTimeScale;
                yield return null;
            }
            if(hitsPunishElapsed >= hitsPunishDecayTimer)
                hitsTakenRecently = 0;
            yield return null;
        }

        IEnumerator ComboTimer() { 
            comboElapsed = 0;
            while(comboElapsed < comboDecayTimer) {
                comboElapsed += Time.deltaTime * bossTimeScale;
                if(currentCombo >= maxCombo) {
                    //Set up combo attack
                    currentCombo = 0;
                    yield break;
                }
                yield return null;
            }
            if(comboElapsed >= comboDecayTimer) {
                currentCombo = 0;
            }
            yield return null;
        }
    }
}
