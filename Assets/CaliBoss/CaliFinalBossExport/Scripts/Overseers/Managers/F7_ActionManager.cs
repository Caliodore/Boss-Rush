using Caliodore.States_Phase2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

namespace Cali7
{ 
    public class F7_ActionManager : MonoBehaviour
    {
        [Header("Testing")]
        public bool testCall;
        public bool printDebugLogs = true;
        [SerializeField] public ActionChoice actionToTest;

        [Header("ActualVars")]
        public static F7_ActionManager Instance;
        public List<ActionChoice> readyActions = new();
        public List<ActionChoice> actionsOnCooldown = new();
        public int readyCount = 0;
        public int cooldownCount = 0;
        public int readyAttacks = 0;

        [Header("ActionChoices")]
        public ActionChoice testChoice;
        public ActionChoice slamAttack;
        public ActionChoice swipeAttack;
        public ActionChoice comboFinish;
        public ActionChoice shardSpray;
        public ActionChoice pillarRise;
        public ActionChoice raiseRing;
        public ActionChoice bloodBarrier;
        public ActionChoice aoePunish;
        public ActionChoice enragedMode;
        public ActionChoice leapSwipe;
        public ActionChoice bloodWall;
        public ActionChoice barrierBroken;
        public ActionChoice reelingBack;
        public ActionChoice enragedExit;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        private void Start()
        {
            testChoice = new ActionChoice(0, (() => Instance.PrintHello()), "TestCall");
            actionToTest = new();
            readyActions.Clear();
            readyActions.Add(testChoice);
            SetActions();
            GenerateCollection();
        }

        private void Update()
        {
            readyCount = readyActions.Count;
            cooldownCount = actionsOnCooldown.Count;
            readyAttacks = readyActions.FindAll(actTry => actTry.choiceType == ActionType.Attack).Count;

            if(testCall) { 
                testCall = false;
                StartAction(testChoice.actionName);
            }
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public ActionChoice DecideAction(ActionType typeIn) { 
            ActionChoice choiceOut = null;
            
            bool isMelee = F7_RefManager.BCNT.playerInMelee;
            List<ActionChoice> actOps = new();

            if(typeIn == ActionType.Attack)
                actOps = readyActions.FindAll(actTry => (actTry.isMelee == isMelee && actTry.choiceType == typeIn));
            else
                actOps = readyActions.FindAll(actTry => (actTry.choiceType == typeIn));

            F7_Help.DebugPrint(printDebugLogs, $"There are {actOps.Count} actions of the type {typeIn} available at this moment.");

            if(actOps.Count <= 0) {
                F7_Help.DebugPrint(printDebugLogs, $"There are no attacks ready at this moment.");
                F7_RefManager.BEVM.OnForceIdle?.Invoke();
            }
            else { 
                int randInd = UnityEngine.Random.Range(0,actOps.Count);
                choiceOut = actOps[randInd];
            }

            return choiceOut;
        }

        public ActionChoice GetSpecificAction(string tryName) { 
            ActionChoice actOut = readyActions.Find(actTry => (actTry.actionName == tryName && actTry.isReady == true));
            if(actOut == null){
                F7_Help.DebugPrint(printDebugLogs, $"The requested action is either on cooldown or not a possible action: {tryName}");
                return null;
            }
            else
                return actOut;
        }
        
//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public bool CheckActionAvailable(string tryName) { ActionChoice tryOut = readyActions.Find(actTry => actTry.actionName == tryName); return tryOut != null; }

        public void StartAction(string tryName) { 
            ActionChoice selectedAct = readyActions.Find(actTry => actTry.actionName == tryName);
            if(selectedAct == null) {
                F7_Help.DebugPrint(printDebugLogs, $"The requested action is either on cooldown or not a possible action: {tryName}");
                return;
            }
            else { 
                StartCoroutine(PutActionOnCooldown(selectedAct));
            }

        }

        private void SetActions() { 
            slamAttack = new ActionChoice(1, (() => Instance.SlamAttack()), "SlamAttack");
            slamAttack.isMelee = true;
            swipeAttack = new ActionChoice(1, (() => Instance.SwipeAttack()), "SwipeAttack");
            swipeAttack.isMelee = true;
            comboFinish = new ActionChoice(1, (() => Instance.ComboFinisher()), "ComboFinisher");
            comboFinish.isMelee = true;
            shardSpray = new ActionChoice(1, (() => Instance.ShardSpray()), "ShardSpray");
            shardSpray.isMelee = false;
            pillarRise = new ActionChoice(1, (() => Instance.PillarRise()), "PillarRise");
            pillarRise.isMelee = false;
            raiseRing = new ActionChoice(1, (() => Instance.RaiseRing()), "RaiseRing");
            raiseRing.isMelee = false;
            bloodBarrier = new ActionChoice(2, (() => Instance.BloodBarrier()), "BloodBarrier");
            aoePunish = new ActionChoice(3, (() => Instance.AoEPunish()), "AoEPunish");
            enragedMode = new ActionChoice(3, (() => Instance.EnragedMode()), "EnragedMode");
            leapSwipe = new ActionChoice(3, (() => Instance.LeapSwipe()), "LeapSwipe");
            bloodWall = new ActionChoice(2, (() => Instance.BloodWall()), "BloodWall");
            barrierBroken = new ActionChoice(5, (() => Instance.BarrierBrokenRecover()), "BarrierBroken");
            reelingBack = new ActionChoice(5, (() => Instance.ReelingBackRecover()), "ReelingBack");
            enragedExit = new ActionChoice(5, (() => Instance.EnragedExitRecover()), "EnragedExit");

        }

        private void GenerateCollection() { 
            List<ActionChoice> tempList = new List<ActionChoice> { slamAttack, swipeAttack, comboFinish, shardSpray, pillarRise, raiseRing, 
                    bloodBarrier, aoePunish, enragedMode, leapSwipe, bloodWall, barrierBroken, reelingBack, enragedExit };
            readyActions.AddRange(tempList);
            F7_Help.DebugPrint(printDebugLogs, $"Collection generated with {readyActions.Count} items.");
        }
        
//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        IEnumerator PutActionOnCooldown(ActionChoice choiceCooling) { 
            var choiceSelect = readyActions.Find(actTry => actTry.actionName == choiceCooling.actionName);
            if(choiceSelect == null) { 
                F7_Help.DebugPrint(printDebugLogs, "Cannot put a method already on cooldown back on cooldown.");
                yield break;
            }
            else { 
                F7_Help.DebugPrint(printDebugLogs, $"Putting {choiceCooling.actionName} on cooldown for {choiceCooling.cooldownTime} seconds.");
                choiceSelect.choiceCall.Invoke();
                readyActions.Remove(choiceSelect);
                choiceSelect.isReady = false;
                actionsOnCooldown.Add(choiceSelect);
                float elapsedTime = 0f;
                while(elapsedTime < choiceCooling.cooldownTime) { 
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                if(elapsedTime >= choiceCooling.cooldownTime) { 
                    actionsOnCooldown.Remove(choiceSelect);
                    choiceSelect.isReady = true;
                    readyActions.Add(choiceSelect);
                }
            }
        }
        
        
//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//      Methods for ActionChoices
//      Misc/Debugging Methods
        public void PrintHello() { print("Heya!"); }

//--------------------------------------------------------
//      MeleeAttacks
        private void SlamAttack() { F7_EventManager.Instance.OnSlamStart?.Invoke(); }
        private void SwipeAttack() { F7_EventManager.Instance.OnSwipeStart?.Invoke(); }
        private void ComboFinisher() { F7_EventManager.Instance.OnReachMaxCombo?.Invoke(); }

//--------------------------------------------------------
//      RangedAttacks
        private void ShardSpray() { F7_EventManager.Instance.OnShardStart?.Invoke(); }
        private void PillarRise() { F7_EventManager.Instance.OnPillarStart?.Invoke(); }
        private void RaiseRing() { F7_EventManager.Instance.OnRingStart?.Invoke(); }

//--------------------------------------------------------
//      MeleePunish/Defense

        private void BloodBarrier() { F7_EventManager.Instance.OnBarrierStart?.Invoke(); }
        private void AoEPunish() { F7_EventManager.Instance.OnAoEStart?.Invoke(); }
        private void EnragedMode() { F7_EventManager.Instance.OnEnrageStart?.Invoke(); }

//--------------------------------------------------------
//      RangedPunish/Defense

        private void LeapSwipe() { F7_EventManager.Instance.OnLeapSwipeStart?.Invoke(); }
        private void BloodWall() { F7_EventManager.Instance.OnWallStart?.Invoke(); }


//--------------------------------------------------------
//      Recoveries

        private void BarrierBrokenRecover() { F7_EventManager.Instance.OnRecoveryStart?.Invoke(1); }
        private void ReelingBackRecover() { F7_EventManager.Instance.OnRecoveryStart?.Invoke(2); }
        private void EnragedExitRecover() { F7_EventManager.Instance.OnRecoveryStart?.Invoke(3); }

    }

    [Serializable]
    public class ActionChoice { 
        public string actionName;
        public ActionType choiceType;
        public Action choiceCall;
        public bool isReady = true;
        public bool isMelee = false;
        public float cooldownTime = 0f;

        public ActionChoice() { }
        public ActionChoice(int actionInt, Action actionCall, string nameIn) { choiceType = (ActionType)actionInt; choiceCall = actionCall; actionName = nameIn; }
        public ActionChoice(int actionInt, Action actionCall, float cooldownTimer, string nameIn) { actionName = nameIn; choiceType = (ActionType)actionInt; choiceCall = actionCall; cooldownTime = cooldownTimer; }
    }

    public enum ActionType { 
        Default = 0,
        Attack = 1,
        Defense = 2,
        Punish = 3,
        Movement = 4,
        Recovery = 5
    }
}
