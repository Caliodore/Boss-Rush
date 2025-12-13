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
        public bool refsSet = false;
        [SerializeField] public UnityEvent<ActionChoice> actionToTest;

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
        public ActionChoice genericRecovery;
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
            if(!F7_RefManager.Instance.gotRefs)
                F7_RefManager.OnRefsLoaded?.AddListener(() => SetReferences());
            else
                SetReferences();
        }

        public void SetReferences() { 
            testChoice = new ActionChoice(0, (() => Instance.PrintHello()), "TestCall");
            actionToTest = new();
            readyActions.Clear();
            readyActions.Add(testChoice);
            SetActions();
            GenerateCollection();
            refsSet = true;
        }

        private void Update()
        {
        if(refsSet) { 
            readyCount = readyActions.Count;
            cooldownCount = actionsOnCooldown.Count;
            readyAttacks = readyActions.FindAll(actTry => actTry.choiceType == ActionType.Attack).Count;

            if(testCall) { 
                testCall = false;
                StartAction(testChoice.actionName);
            }
        }
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public ActionChoice DecideAction(ActionType typeIn) { 
            F7_Help.DebugPrint(printDebugLogs, $"DecideAction called for {typeIn.ToString()} type of action.");
            ActionChoice choiceOut = null;
            
            bool isMelee = F7_RefManager.BCNT.playerInMelee;
            bool wantDefend = F7_RefManager.BCNT.CheckIfWantDefense();
            bool wantPunish = F7_RefManager.BCNT.CheckIfPunishReady();
            bool reactReady = CheckActionAvailable(ActionType.Defense.ToString(), isMelee) || CheckActionAvailable(ActionType.Punish.ToString(), isMelee);

            if(((wantDefend || wantPunish) && reactReady) || typeIn == ActionType.Defense) {
                F7_Help.DebugPrint(printDebugLogs, $"DecideAction determined boss wanted to defend or punish.");
                return DetermineReaction(wantDefend, wantPunish);
            }

            if(typeIn == ActionType.Recovery) { 
                return genericRecovery;
            }

            F7_Help.DebugPrint(printDebugLogs, $"DecideAction determined boss did NOT want to defend or punish.");

            List<ActionChoice> actOps = new();

            if(typeIn == ActionType.Attack)
                actOps = readyActions.FindAll(actTry => (actTry.isMelee == isMelee && actTry.choiceType == typeIn));
            else
                actOps = readyActions.FindAll(actTry => (actTry.choiceType == typeIn));

            var tryCombo = actOps.Find(actTry => actTry.actionName == comboFinish.actionName);
            if(F7_RefManager.BCNT.CheckIfComboReady() && tryCombo != null)
                actOps.Remove(tryCombo);

            F7_Help.DebugPrint(printDebugLogs, $"There are {actOps.Count} actions of the type {typeIn} available at this moment.");

            if(actOps.Count <= 0) {
                if(typeIn == ActionType.Attack && isMelee) { 
                    actOps = readyActions.FindAll(actTry => (actTry.isMelee == false && actTry.choiceType == typeIn));
                    if(actOps.Count <= 0) {
                        F7_Help.DebugPrint(printDebugLogs, $"There are no attacks ready at this moment.");
                        F7_RefManager.BEVM.OnForceIdle?.Invoke();
                    }
                    else { 
                        int randInd = UnityEngine.Random.Range(0,actOps.Count);
                        choiceOut = actOps[randInd];
                    }
                }
                if(typeIn == ActionType.Attack && !isMelee) { 
                    actOps = readyActions.FindAll(actTry => (actTry.isMelee == true && actTry.choiceType == typeIn));
                    if(actOps.Count <= 0) {
                        F7_Help.DebugPrint(printDebugLogs, $"There are no attacks ready at this moment.");
                        F7_RefManager.BEVM.OnForceIdle?.Invoke();
                    }
                    else { 
                        int randInd = UnityEngine.Random.Range(0,actOps.Count);
                        choiceOut = actOps[randInd];
                    }
                }
            }
            else { 
                int randInd = UnityEngine.Random.Range(0,actOps.Count);
                choiceOut = actOps[randInd];
            }

            return choiceOut;
        }

        private ActionChoice DetermineReaction(bool defBool, bool punBool) { 
            ActionChoice choiceOut = null;
            bool meleeCheck = F7_RefManager.BCNT.playerInMelee;

            if((punBool && !defBool) || (punBool && defBool)) {
                if(F7_RefManager.BCNT.currentHealth <= (F7_RefManager.BPSO.maxHealth/2)) { 
                    if(enragedMode.isReady) { 
                        choiceOut = enragedMode;
                    }
                    else if(aoePunish.isReady){ 
                        choiceOut = aoePunish;
                    }
                }
                else {
                    if(meleeCheck) {
                        if(bloodBarrier.isReady) { 
                            choiceOut = bloodBarrier;
                        }
                        else if(bloodWall.isReady) { 
                            choiceOut = bloodWall;
                        }
                    }
                }
            }
            else if(defBool && !punBool) { 
                if(meleeCheck) { 
                    if(bloodBarrier.isReady)
                        choiceOut = bloodBarrier;
                }
                else
                    if(bloodWall.isReady)
                        choiceOut = bloodWall;
            }
            else if(!punBool && !defBool) { 
                if(meleeCheck) { 
                    if(bloodBarrier.isReady)
                        choiceOut = bloodBarrier;
                }
                else
                    if(bloodWall.isReady)
                        choiceOut = bloodWall;
            }

            if(choiceOut != null)
                return choiceOut;
            else
                return swipeAttack;
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        public ActionChoice GetSpecificAction(string tryName) { 
            ActionChoice actOut = readyActions.Find(actTry => (actTry.actionName == tryName && actTry.isReady == true));
            if(actOut == null){
                F7_Help.DebugPrint(printDebugLogs, $"The requested action is either on cooldown or not a possible action: {tryName}");
                return null;
            }
            else
                return actOut;
        }

        public int ActionsOnCooldown(ActionType actionIn) { 
            var actsFound = actionsOnCooldown.FindAll(actTry => actTry.choiceType == actionIn);
            return actsFound.Count;
        }
        
//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public bool CheckActionAvailable(string tryName, bool meleeState) { ActionChoice tryOut = readyActions.Find(actTry => actTry.actionName == tryName && actTry.isMelee == meleeState); return tryOut != null; }

        public void StartAction(string tryName) { 
            ActionChoice selectedAct = readyActions.Find(actTry => actTry.actionName == tryName);
            if(selectedAct == null) {
                tryName = "null";
                F7_Help.DebugPrint(printDebugLogs, $"The requested action is either on cooldown or not a possible action/null, the name is: {tryName}");
                return;
            }
            else { 
                StartCoroutine(PutActionOnCooldown(selectedAct));
            }
        }

        private void SetActions() { 
            slamAttack = new ActionChoice(1, (() => Instance.SlamAttack()), 5f, "SlamAttack");
            slamAttack.isMelee = true;
            swipeAttack = new ActionChoice(1, (() => Instance.SwipeAttack()), 5f, "SwipeAttack");
            swipeAttack.isMelee = true;
            comboFinish = new ActionChoice(6, (() => Instance.ComboFinisher()), 48f, "ComboFinisher");
            comboFinish.isMelee = true;
            shardSpray = new ActionChoice(1, (() => Instance.ShardSpray()), 18f, "ShardSpray");
            shardSpray.isMelee = false;
            pillarRise = new ActionChoice(1, (() => Instance.PillarRise()), 24f, "PillarRise");
            pillarRise.isMelee = false;
            raiseRing = new ActionChoice(1, (() => Instance.RaiseRing()), 32f, "RaiseRing");
            raiseRing.isMelee = false;
            bloodBarrier = new ActionChoice(2, (() => Instance.BloodBarrier()), 4f, "BloodBarrier");
            bloodBarrier.isMelee = true;
            aoePunish = new ActionChoice(3, (() => Instance.AoEPunish()), 12f, "AoEPunish");
            aoePunish.isMelee = true;
            enragedMode = new ActionChoice(3, (() => Instance.EnragedMode()), 12f, "EnragedMode");
            enragedMode.isMelee = true;
            leapSwipe = new ActionChoice(3, (() => Instance.LeapSwipe()), 16f, "LeapSwipe");
            leapSwipe.isMelee = false;
            bloodWall = new ActionChoice(2, (() => Instance.BloodWall()), 8f, "BloodWall");
            bloodWall.isMelee = false;
            genericRecovery = new ActionChoice(5, (() => Instance.GenericRecovery()), 1f, "GenericRecovery");
            barrierBroken = new ActionChoice(5, (() => Instance.BarrierBrokenRecover()), 1f, "BarrierBroken");
            reelingBack = new ActionChoice(5, (() => Instance.ReelingBackRecover()), 1f, "ReelingBack");
            enragedExit = new ActionChoice(5, (() => Instance.EnragedExitRecover()), 1f, "EnragedExit");

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
                    F7_Help.DebugPrint(printDebugLogs, $"{choiceSelect.actionName} came off cooldown.");
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

        private void GenericRecovery() { F7_EventManager.Instance.OnRecoveryStart?.Invoke(0); }
        private void BarrierBrokenRecover() { F7_EventManager.Instance.OnRecoveryStart?.Invoke(1); }
        private void ReelingBackRecover() { F7_EventManager.Instance.OnRecoveryStart?.Invoke(2); }
        private void EnragedExitRecover() { F7_EventManager.Instance.OnRecoveryStart?.Invoke(3); }

//----------------------------------------------------------------------------------------------------------------

        [ContextMenu("InvokeActionTest")]
        public void InvokeActionTest(ActionChoice actionChoiceIn) { actionToTest?.Invoke(actionChoiceIn); }

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
        Recovery = 5,
        Special = 6
    }
}
