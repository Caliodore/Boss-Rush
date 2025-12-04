using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

        public BossAction RandomActionChoice(int actionInt) { 
            BossAction outputAction = null;

            var actionsAvailable = actionTypes.FindAll(actionSearch => ((int)actionSearch.ActionType == actionInt) && (actionSearch.isReady));
            if(actionsAvailable.Count > 0) { 
                int randIndex = UnityEngine.Random.Range(0,actionsAvailable.Count - 1);
                outputAction = actionsAvailable[randIndex];
                StartCooldown(outputAction);
            }
            else { 
                A6_Help.DebugPrint(printDebugLogs, $"There are no actions available of the type requested: {(BossAction.ActionChoice)actionInt}");
                outputAction = null;
            }

            return outputAction;
        }

        //Melee
        public void RegularSlam() { 
            A6_Help.DebugPrint(printDebugLogs, "RegularSlam invoked.");
        }
        public void RegularSwipe() { 
            A6_Help.DebugPrint(printDebugLogs, "RegularSwipe invoked.");
        }
        public void ComboMixup() { 
            A6_Help.DebugPrint(printDebugLogs, "ComboMixup invoked.");
        }
        public void ComboFinisher() { 
            A6_Help.DebugPrint(printDebugLogs, "ComboFinisher invoked.");
        }

        //Ranged
        public void ShardSpray() { 
            A6_Help.DebugPrint(printDebugLogs, "ShardSpray invoked.");
        }
        public void PillarSpread() { 
            A6_Help.DebugPrint(printDebugLogs, "PillarsAttack invoked.");
        }
        public void ClosingRing() { 
            A6_Help.DebugPrint(printDebugLogs, "ClosingRing invoked.");
        }
        public void LeapSwipe() { 
            A6_Help.DebugPrint(printDebugLogs, "LeapAttack invoked.");
        }

//      |> Defenses/Punishments
        public void BloodBarrier() { 
            A6_Help.DebugPrint(printDebugLogs, "BloodBarrier invoked.");
        }
        public void BloodWall() { 
            A6_Help.DebugPrint(printDebugLogs, "BloodWall invoked.");
        }
        public void EnragedMode() { 
            A6_Help.DebugPrint(printDebugLogs, "EnragedMode invoked.");
        }
        public void AoEPunish() { 
            A6_Help.DebugPrint(printDebugLogs, "AoEPunish invoked.");
        }

//      |> Movement
        public void LeapMove() { 
            A6_Help.DebugPrint(printDebugLogs, "LeapMove invoked.");
        }
        public void DashMove() { 
            A6_Help.DebugPrint(printDebugLogs, "DashMove invoked.");
        }

//      |> Recovery
        public void ReelingBackRecovery() { 
            A6_Help.DebugPrint(printDebugLogs, "ReelingBack invoked.");
        }
        public void BrokenBarrierDazeRecovery() { 
            A6_Help.DebugPrint(printDebugLogs, "BrokenBarrier invoked.");
        }
        public void EnragedExitRecovery() { 
            A6_Help.DebugPrint(printDebugLogs, "EnragedExit invoked.");
        }

        public void StartCooldown(BossAction actionIn) { StartCoroutine(InteractionCooldown(actionIn)); }

        IEnumerator InteractionCooldown(BossAction actionIn) { 
            actionIn.isReady = false;
            yield return new WaitForSeconds(actionIn.cooldownTime);
            actionIn.isReady = true;
        }

    }

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
