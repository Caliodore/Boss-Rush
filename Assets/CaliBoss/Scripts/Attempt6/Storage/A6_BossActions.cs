using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Cali6
{ 
    public class A6_BossActions : MonoBehaviour
    {
        public bool printDebugLogs = true;
        public A6_BossActions() { }
        public static A6_BossActions Instance;

        [Header("Cooldown Timers")]
        public float meleeCooldown = 5f;
        public float rangedCooldown = 6f;
        public float defenseCooldown = 4f;
        public float punishCooldown = 12f;
        public float movementCooldown = 10f;

        public List<BossAction> actionTypes = new List<BossAction>();
        //Melee
        public BossAction slamAttack = new();
        public BossAction swipeAttack = new();
        public BossAction comboMixup = new();
        public BossAction comboFinisher = new();
        //Ranged
        public BossAction shardSpray = new();
        public BossAction pillarSpread = new();
        public BossAction closingRing = new();
        public BossAction leapSwipe = new();
        //Defenses
        public BossAction bloodBarrier = new();
        public BossAction bloodWall = new();
        public BossAction enragedMode = new();
        public BossAction aoePunish = new();
        //Movement
        public BossAction leapMove = new();
        public BossAction dashMove = new();
        //Recovery
        public BossAction reelingBackRecover = new();
        public BossAction barrierBrokenRecover = new();
        public BossAction exitEnragedRecover = new();


        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        private void Start()
        {
            slamAttack = new BossAction(1,(() => Instance.RegularSlam()), Instance.meleeCooldown);
            swipeAttack = new BossAction(1, (() => Instance.RegularSwipe()), Instance.meleeCooldown);
            comboMixup = new BossAction(1, (() => Instance.ComboMixup()), Instance.meleeCooldown);
            comboFinisher = new BossAction(1, (() => Instance.ComboFinisher()), Instance.meleeCooldown);
            //Ranged
            shardSpray = new BossAction(2, (() => Instance.ShardSpray()), Instance.rangedCooldown);
            pillarSpread = new BossAction(2, (() => Instance.PillarSpread()), Instance.rangedCooldown);
            closingRing = new BossAction(2, (() => Instance.ClosingRing()), Instance.rangedCooldown);
            leapSwipe = new BossAction(2, (() => Instance.LeapSwipe()), Instance.rangedCooldown);
            //Defenses
            bloodBarrier = new BossAction(3, (() => Instance.BloodBarrier()), Instance.defenseCooldown);
            bloodWall = new BossAction(3, (() => Instance.BloodWall()), Instance.defenseCooldown);
            enragedMode = new BossAction(3, (() => Instance.EnragedMode()), Instance.punishCooldown);
            aoePunish = new BossAction(3, (() => Instance.AoEPunish()), Instance.punishCooldown);
            //Movement
            leapMove = new BossAction(4, (() => Instance.LeapMove()), Instance.movementCooldown);
            dashMove = new BossAction(4, (() => Instance.DashMove()), Instance.movementCooldown);
            //Recovery
            reelingBackRecover = new BossAction(5, (() => Instance.ReelingBackRecovery()));
            barrierBrokenRecover = new BossAction(5, (() => Instance.BrokenBarrierDazeRecovery()));
            exitEnragedRecover = new BossAction(5, (() => Instance.EnragedExitRecovery()));
            //MakeList
            actionTypes = new List<BossAction>() { slamAttack, swipeAttack, comboMixup, comboFinisher, shardSpray, pillarSpread, closingRing, leapSwipe, bloodBarrier, bloodWall, enragedMode, aoePunish, leapMove, dashMove, reelingBackRecover, barrierBrokenRecover, exitEnragedRecover };
            
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------------
//      Public Method To Fetch Choices

        public BossAction RandomActionChoice(int actionInt) { 
            BossAction outputAction = null;

            switch (actionInt) { 
                case(1):
                    MeleeAttackDeterminer();
                    break;
                    
                case(2):
                    RangedAttackDeterminer();
                    break;
                    
                case(3):
                    DefenseDeterminer();
                    break;
                    
                case(4):
                    MovementDeterminer();
                    break;
                    
                case(5):
                    RecoveryDeterminer();
                    break;

            }

            return outputAction;
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------------
//      Determinants Per State

        private BossAction MeleeAttackDeterminer() { 
            BossAction outputAction = null;

            var availableChoices = FetchAvailableChoices(1);
            if(A6_Brain.Instance.currentCombo < A6_Brain.Instance.maxCombo){ 
                availableChoices.Remove(availableChoices.Find(actChoice => actChoice == comboFinisher));
                if(A6_Brain.Instance.currentCombo < 3)
                    availableChoices.Remove(availableChoices.Find(actChoice => actChoice == comboMixup));

                if(availableChoices.Count < 1 && comboMixup.isReady) { 
                    outputAction = comboMixup;
                }
                else if(availableChoices.Count > 0) { 
                    int randInd = UnityEngine.Random.Range(0,availableChoices.Count - 1);
                    outputAction = availableChoices[randInd];
                }

            }
            else
                outputAction = comboFinisher;

            return outputAction;
        }
        private BossAction RangedAttackDeterminer() { 
            BossAction outputAction = null;
            
            var availableChoices = FetchAvailableChoices(2);
            if(availableChoices.Count < 1) { 
                A6_Help.DebugPrint(printDebugLogs, "No available ranged attacks could be found.");
                return MovementDeterminer();
            }
            else { 
                int randInd = UnityEngine.Random.Range(0,availableChoices.Count - 1);
                outputAction = availableChoices[randInd];
            }

            return outputAction;
        }

        private BossAction DefenseDeterminer() { 
            BossAction outputAction = null;
            
            var availableChoices = FetchAvailableChoices(3);

            return outputAction;
        }
        private BossAction MovementDeterminer() { 
            BossAction outputAction = null;
            
            var availableChoices = FetchAvailableChoices(4);

            return outputAction;
        }
        private BossAction RecoveryDeterminer() { 
            BossAction outputAction = null;
            
            var availableChoices = FetchAvailableChoices(5);

            return outputAction;
        }

        private List<BossAction> FetchAvailableChoices(int actionInt) { 
            List<BossAction> outputList = null;

            outputList = actionTypes.FindAll(actionSearch => ((int)actionSearch.ActionType == actionInt) && (actionSearch.isReady));

            return outputList;
        }
        
//--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Melee
        public void RegularSlam() { 
            A6_Help.DebugPrint(printDebugLogs, "RegularSlam invoked.");
            A6_AnimManager.Instance.attInt.SetIntVal(1);
            A6_AnimManager.Instance.meleeBool.SetBool(true);
            A6_AnimManager.Instance.attTrig.SetTrig();
        }
        public void RegularSwipe() { 
            A6_Help.DebugPrint(printDebugLogs, "RegularSwipe invoked.");
            A6_AnimManager.Instance.attInt.SetIntVal(2);
            A6_AnimManager.Instance.meleeBool.SetBool(true);
            A6_AnimManager.Instance.attTrig.SetTrig();
        }
        public void ComboMixup() { 
            A6_Help.DebugPrint(printDebugLogs, "ComboMixup invoked.");
            A6_AnimManager.Instance.attInt.SetIntVal(3);
            A6_AnimManager.Instance.meleeBool.SetBool(true);
            A6_AnimManager.Instance.attTrig.SetTrig();
        }
        public void ComboFinisher() { 
            A6_Help.DebugPrint(printDebugLogs, "ComboFinisher invoked.");
            A6_AnimManager.Instance.attInt.SetIntVal(4);
            A6_AnimManager.Instance.meleeBool.SetBool(true);
            A6_AnimManager.Instance.attTrig.SetTrig();
        }
//-----------------------------------------------------------------------------------
        //Ranged
        public void ShardSpray() { 
            A6_Help.DebugPrint(printDebugLogs, "ShardSpray invoked.");
            A6_AnimManager.Instance.attInt.SetIntVal(1);
            A6_AnimManager.Instance.meleeBool.SetBool(false);
            A6_AnimManager.Instance.attTrig.SetTrig();
        }
        public void PillarSpread() { 
            A6_Help.DebugPrint(printDebugLogs, "PillarsAttack invoked.");
            A6_AnimManager.Instance.attInt.SetIntVal(2);
            A6_AnimManager.Instance.meleeBool.SetBool(false);
            A6_AnimManager.Instance.attTrig.SetTrig();
        }
        public void ClosingRing() { 
            A6_Help.DebugPrint(printDebugLogs, "ClosingRing invoked.");
            A6_AnimManager.Instance.attInt.SetIntVal(3);
            A6_AnimManager.Instance.meleeBool.SetBool(false);
            A6_AnimManager.Instance.attTrig.SetTrig();
        }
        public void LeapSwipe() { 
            A6_Help.DebugPrint(printDebugLogs, "LeapAttack invoked.");
            A6_AnimManager.Instance.attInt.SetIntVal(4);
            A6_AnimManager.Instance.meleeBool.SetBool(false);
            A6_AnimManager.Instance.attTrig.SetTrig();
        }
//-----------------------------------------------------------------------------------
//      |> Defenses/Punishments
        public void BloodBarrier() { 
            A6_Help.DebugPrint(printDebugLogs, "BloodBarrier invoked.");
            A6_AnimManager.Instance.defInt.SetIntVal(1);
            A6_AnimManager.Instance.defTrig.SetTrig();
        }
        public void BloodWall() { 
            A6_Help.DebugPrint(printDebugLogs, "BloodWall invoked.");
            A6_AnimManager.Instance.defInt.SetIntVal(2);
            A6_AnimManager.Instance.defTrig.SetTrig();
        }
        public void EnragedMode() { 
            A6_Help.DebugPrint(printDebugLogs, "EnragedMode invoked.");
            A6_AnimManager.Instance.defInt.SetIntVal(3);
            A6_AnimManager.Instance.defTrig.SetTrig();
        }
        public void AoEPunish() { 
            A6_Help.DebugPrint(printDebugLogs, "AoEPunish invoked.");
            A6_AnimManager.Instance.defInt.SetIntVal(4);
            A6_AnimManager.Instance.defTrig.SetTrig();
        }
//-----------------------------------------------------------------------------------
//      |> Movement
        public void LeapMove() { 
            A6_Help.DebugPrint(printDebugLogs, "LeapMove invoked.");
            A6_AnimManager.Instance.moveInt.SetIntVal(1);
            A6_AnimManager.Instance.moveTrig.SetTrig();
        }
        public void DashMove() { 
            A6_Help.DebugPrint(printDebugLogs, "DashMove invoked.");
            A6_AnimManager.Instance.moveInt.SetIntVal(2);
            A6_AnimManager.Instance.moveTrig.SetTrig();
        }
//-----------------------------------------------------------------------------------
//      |> Recovery
        public void ReelingBackRecovery() { 
            A6_Help.DebugPrint(printDebugLogs, "ReelingBack invoked.");
            A6_AnimManager.Instance.recovInt.SetIntVal(1);
            A6_AnimManager.Instance.recovTrig.SetTrig();
        }
        public void BrokenBarrierDazeRecovery() { 
            A6_Help.DebugPrint(printDebugLogs, "BrokenBarrier invoked.");
            A6_AnimManager.Instance.recovInt.SetIntVal(2);
            A6_AnimManager.Instance.recovTrig.SetTrig();
        }
        public void EnragedExitRecovery() { 
            A6_Help.DebugPrint(printDebugLogs, "EnragedExit invoked.");
            A6_AnimManager.Instance.recovInt.SetIntVal(3);
            A6_AnimManager.Instance.recovTrig.SetTrig();
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public void StartCooldown(BossAction actionIn) { StartCoroutine(InteractionCooldown(actionIn)); }

        IEnumerator InteractionCooldown(BossAction actionIn) { 
            actionIn.isReady = false;
            yield return new WaitForSeconds(actionIn.cooldownTime);
            actionIn.isReady = true;
        }

    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public class BossAction { 
        public enum ActionChoice { 
            Default = 0,
            Melee = 1,
            Ranged = 2,
            Defense = 3,
            Movement = 4,
            Recovery = 5
        }

        public ActionChoice ActionType;
        public Delegate assignedCall;
        public bool isReady = true;
        public float cooldownTime = 0.1f;

        public BossAction() { }
        public BossAction(int attackInt, Action attackCall) { ActionType = (ActionChoice)attackInt; assignedCall = attackCall; }
        public BossAction(int attackInt, Action attackCall, float cooldownTimer) { ActionType = (ActionChoice)attackInt; assignedCall = attackCall; cooldownTime = cooldownTimer; }
    }
}
