using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Cali7
{ 
    public class F7_StateDeterminer : MonoBehaviour
    {
        public bool printDebugLogs = true;
        public bool refsLoaded = false;
        public static F7_StateDeterminer Instance;

        private F7_StateBase stateFrom;
        private F7_StateBase stateTo;

        public F7_StateBase NextDeterminedState { get { return DetermineNextState(); } }

        private List<F7_StateBase> possibleStates = new();


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
            possibleStates.Add(F7_RefManager.BSTA);
            possibleStates.Add(F7_RefManager.BSTC);
            possibleStates.Add(F7_RefManager.BSTD);
            possibleStates.Add(F7_RefManager.BSTI);
            possibleStates.Add(F7_RefManager.BSTR);
        }

        public F7_StateBase DetermineNextState() {
            F7_StateBase stateOut = null;
            stateOut = AwarenessCheck();
            if(stateOut == null) { 
                stateOut = F7_RefManager.BSTC;
                F7_Help.DebugPrint(printDebugLogs, "StateDeterminer returned chase because AwarenessCheck returned null");
            }
            return stateOut;
        }

        /*
         * 
         * At any moment be able to determine what action to take.
         * First priority: 
         * Did the last state have a recovery period at the end? => Yes => then go to recovery
         * => No => Then recovery is an option among the others
         * 
         * 
         * Second priority:
         * Is player in melee? => yes => rules out chase
         * player in melee? => no => attacking has to be ranged and is most likely a response
         * 
         * Starts to get more vague here, but generally here is what we need to check:
         *  } Whether or not f7_central is ready for a punishment, and whether or not any punishments are available
         *  } Boss health percentage, if below half enraged is preferred
         *  } CurrentPhase, similar to health percentage
         *  } 
         * 
         */

        private F7_StateBase AwarenessCheck() { 
            bool inMelee = F7_RefManager.BCNT.playerInMelee;
            bool comboReady = F7_RefManager.BCNT.CheckIfComboReady();
            bool punishReady = F7_RefManager.BCNT.CheckIfPunishReady();
            bool attackCheck = F7_RefManager.BACM.CheckActionAvailable(ActionType.Attack.ToString(), inMelee);
            bool defenseCheck = F7_RefManager.BCNT.CheckIfWantDefense();

            F7_StateBase outputState = null;
            
            F7_Help.DebugPrint(printDebugLogs, "StateDeterminer read CurrentState as: " + F7_RefManager.BSTM.CurrentState.ToString());
            switch(F7_RefManager.BSTM.CurrentState.ToString()) { 
                case("Attacking"):
                    //Currently attacking, should go to a recovery.
                    //F7_Help.DebugPrint(printDebugLogs, "StateDeterminer decided on Attacking to Recovering.");
                    outputState = F7_RefManager.BSTR;
                    break;
                    
                case("Chase"):
                    //Currently chasing, is ending because it either got in melee or decided to punish.
                    //F7_Help.DebugPrint(printDebugLogs, "StateDeterminer decided on Chase to Attacking.");
                    if(attackCheck)
                        outputState = F7_RefManager.BSTA;
                    else
                        outputState = F7_RefManager.BSTD;
                    break;

                case("Defending"):
                    //Currently defending, most likely recovering or chasing next.
                    //F7_Help.DebugPrint(printDebugLogs, "StateDeterminer decided on Defending to Recovering.");
                    outputState = F7_RefManager.BSTR;
                    break;

                case("Idling"):
                    //Currently idling, needs to either chase or attack.
                    if(F7_RefManager.BCNT.playerInMelee || attackCheck) { 
                        //F7_Help.DebugPrint(printDebugLogs, "StateDeterminer decided on Idling to Attacking.");
                        outputState = F7_RefManager.BSTA;
                    }
                    else { 
                        //F7_Help.DebugPrint(printDebugLogs, "StateDeterminer decided on Idling to Chase.");
                        outputState = F7_RefManager.BSTC;
                    }
                    break;

                case("Recovering"):
                    //Currently recovering, can go to any state realistically
                    //F7_Help.DebugPrint(printDebugLogs, "StateDeterminer decided on Recovering to Idling.");
                    if(inMelee && !defenseCheck)
                        outputState = F7_RefManager.BSTA;
                    else if(defenseCheck)
                        outputState = F7_RefManager.BSTD;
                    else
                        outputState = F7_RefManager.BSTC;
                    break;
            }

            return outputState;
        }
    }
}
