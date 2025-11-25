using CaliJR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Cali3
{ 
    public class T3_Brain : MonoBehaviour
    {
        static T3_Brain() { }

        //Just basic component refs using what was provided to us.
        [Header("Basic Comp Refs")]
        [SerializeField] public T3_StateMachine BossSM;
        [SerializeField] public Navigator BossNavigator;
        [SerializeField] public NavMeshAgent BossNavMeshAgent;
        [SerializeField] public Damager BossDamager;
        [SerializeField] public Damageable BossDamageable;
        [SerializeField] public Actor BossActor;
        [SerializeField] public Sensor BossSensor;

        [SerializeField] public PlayerLogic playerScript;
        [SerializeField] public GameObject playerObj;

        [SerializeField] public Animator SwingAnimCtrl;

        [SerializeField] public List<T3_State> BossStates = new List<T3_State>();

        public static T3_Brain MainBrain;

        [Header("Basic Movement Vars")]
        public float moveSpeed = 2f;
        public float attackRange = 2f;
        public float attackCooldown = 2f;
        public float damageDealt = 1f;
        public float timeUntilAttemptRanged = 2.5f;
        public Vector3 globalTarget = Vector3.zero;

        [Header("Basic Logic Vars")]
        public bool attackRecovering = false;
        public bool playerInMeleeRange = false;
        public bool movingToTarget = false;
        public bool isTurningToPlayer = false;

        [Header("Events")]
        public UnityEvent OnPlayerKeepingDistance;

        //Extra stuff
        Coroutine movementUpdateCoRo;
        Coroutine turningCoRo;

        //Debugging
        public static string debugTestMsg = new string("T3_BRAIN DEBUG TEST");

        private void Awake()
        {
            if(MainBrain == null)
                MainBrain = this;

            T3_State[] stateArray = GetComponentsInChildren<T3_State>();
            foreach(T3_State iState in stateArray)
            { 
                BossStates.Add(iState);
                print("Moved from array to list: " + iState.ToString());
            }

            //print("BossStates.Count: " + BossStates.Count);

            //BossSM = gameObject.GetComponent<T3_StateMachine>();

            if(BossSM != null)
                BossSM.ChangeState("Idle");
            /*else
                gameObject.GetComponent<T3_StateMachine>().ChangeState("Idle");*/

            playerScript = FindAnyObjectByType<PlayerLogic>();
            playerObj = playerScript.gameObject;

            //var predicateTest = BossStates.Find(stName => stName.stateName == "Idle");
            //print($"PredicateTest: {predicateTest} \nPTest.ToString(): {predicateTest.ToString()}");

            print("T3_Brain Awake Finished");
        }

        private void Start()
        {
            BossSensor.OnEnter?.AddListener(() => PlayerInMeleeToggle(true));
            BossSensor.OnExit?.AddListener(() => PlayerInMeleeToggle(false));

            //if(BossSM != null)
            //    print("BossSM isn't null.");
            //if(BossStates != null)
            //    print("BossStates isn't null.");

            //BossSM.ChangeState("Idle");

            print("T3_Brain Start Finished");
        }

        //Helper/General Methods
        public void MoveTo(Vector3 targetPosition)
        {
            print("MoveTo called");
            Vector3 outputPos = transform.position;
            if(BossNavigator.CalculatePathToPosition(targetPosition))
            {
                outputPos = targetPosition;
            }
            else
            {
                print("BossNavMeshAgent or Navigator cannot reach that location.");
            }
            BossNavMeshAgent.SetDestination(outputPos);
        }

        public void InterruptStateForAttack()
        { 
            BossSM.ChangeState("Attacking");
        }

        //Player-Focused methods
        public void PursuePlayer()
        {
            globalTarget = playerObj.transform.position;
            movementUpdateCoRo ??= StartCoroutine(UpdateMovementTarget());
        }

        public void TurnTowardsPlayer()
        { 
            //turningCoRo ??= StartCoroutine(TurningToPlayer());
            transform.LookAt(playerObj.transform);
        }

        public bool FacingPlayerCheck()
        {
            Vector3 distVec = playerObj.transform.position - transform.position;
            float dotRatio = Vector3.Dot(distVec.normalized,transform.forward);
            if(!(dotRatio >= 0.9))
            {
                return false;
            }
            else
                return true;
        }


        //Combat methods
        public void SwingSword()
        { 
            if(!attackRecovering)
            { 
                SwingAnimCtrl.SetTrigger("DoAttack");
            }
            else
            { 
                print("Attempting to swing again before attack animation finishes.");    
            }
        }

        public void ShardSpray()
        { 
            print("Attempted ranged shotgun attack.");    
        }

        //Coroutines to throttle resource usage
        IEnumerator UpdateMovementTarget()
        {
            movingToTarget = true;
            while(movingToTarget)
            { 
                MoveTo(globalTarget);
                yield return null;
                if(Vector3.Distance(gameObject.transform.position, BossNavMeshAgent.destination) <= 1) { movingToTarget = false; yield break; }
            }
            yield return null;
        }

        /*IEnumerator TurningToPlayer()
        {
            isTurningToPlayer = true;
            while(isTurningToPlayer) 
            {
                Vector3 distVec = playerObj.transform.position - transform.position;
                float dotRatio = Vector3.Dot(distVec.normalized,transform.forward);
                if(!(dotRatio >= 0.9))
                { 
                    TurnTowardsPlayer();
                }
                yield return null;
            }
            yield return null;    
        }*/

        //Toggle methods for events to interact with
        public void AnimEventTarget(bool startOrEnd) { attackRecovering = startOrEnd; }
        public void PlayerInMeleeToggle(bool inOrOut) { playerInMeleeRange = inOrOut; }
    }
}
