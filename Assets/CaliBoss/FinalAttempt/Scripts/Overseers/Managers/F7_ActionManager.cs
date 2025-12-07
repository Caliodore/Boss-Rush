using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Cali7
{ 
    public class F7_ActionManager : MonoBehaviour
    {
        public bool testCall;
        public bool printDebugLogs = true;
        public static F7_ActionManager Instance;
        public List<ActionChoice> readyActions = new();
        public List<ActionChoice> actionsOnCooldown = new();
        public ActionChoice testChoice;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        private void Start()
        {
            testChoice = new ActionChoice(0, (() => Instance.PrintHello()), "TestCall");
            readyActions.Clear();
            readyActions.Add(testChoice);
        }

        private void Update()
        {
            if(testCall) { 
                testCall = false;
                TryAction(testChoice.actionName);
            }
        }

        public void TryAction(string tryName) { 
            ActionChoice selectedAct = readyActions.Find(actTry => actTry.actionName == tryName);
            if(selectedAct == null) {
                F7_Help.DebugPrint(printDebugLogs, $"The requested action is either on cooldown or not a possible action: {tryName}");
                return;
            }
            else { 
                StartCoroutine(PutActionOnCooldown(selectedAct));
            }

        }

        IEnumerator PutActionOnCooldown(ActionChoice choiceCooling) { 
            var choiceSelect = readyActions.Find(actTry => actTry.actionName == choiceCooling.actionName);
            if(choiceSelect == null) { 
                F7_Help.DebugPrint(printDebugLogs, "Cannot put a method already on cooldown back on cooldown.");
                yield break;
            }
            else { 
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

        public void PrintHello() { print("Heya!"); }
    }

    public class ActionChoice { 
        public string actionName;
        public ActionType choiceType;
        public Action choiceCall;
        public bool isReady = true;
        public bool isMelee = false;
        public float cooldownTime = 0f;

        public ActionChoice() { }
        public ActionChoice(int attackInt, Action attackCall, string nameIn) { choiceType = (ActionType)attackInt; choiceCall = attackCall; actionName = nameIn; }
        public ActionChoice(int attackInt, Action attackCall, float cooldownTimer, string nameIn) { actionName = nameIn; choiceType = (ActionType)attackInt; choiceCall = attackCall; cooldownTime = cooldownTimer; }
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
