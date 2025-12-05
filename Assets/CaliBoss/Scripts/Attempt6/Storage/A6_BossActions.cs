using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using DG.Tweening;

namespace Cali6
{ 
    public class A6_BossActions : MonoBehaviour
    {
        public bool printDebugLogs = true;
        public A6_BossActions() { }
        public static A6_BossActions Instance;

        [Header("Physical Variables")]
        public float pillarRaiseSpeed = 2.5f;
        public float pillarLifetime = 18f;
        public float bloodWallLifetime = 10f;

        public int normalPillarAmount = 5;
        public int enragedPillarAmount = 8;
        public int normalShardAmount = 4;
        public int enragedShardAmount = 9;

        [Header("Cooldown Timers")]
        public float meleeCooldown = 5f;
        public float rangedCooldown = 6f;
        public float defenseCooldown = 4f;
        public float punishCooldown = 12f;
        public float movementCooldown = 10f;

        [Header("Action Prefabs")]
        [SerializeField] public GameObject pillarPrefab;
        [SerializeField] public GameObject shardPrefab;
        [SerializeField] public GameObject ringPrefab;
        [SerializeField] public GameObject barrierPrefab;
        [SerializeField] public GameObject wallPrefab;
        [SerializeField] public GameObject swipeEffectPrefab;
        [SerializeField] public GameObject slamEffectPrefab;

        [Header("Starting Points of Attacks")]
        [SerializeField] public GameObject shardInstantiationPoint;

        public GameObject pillarEmpty;
        private List<GameObject> activePillars = new();

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
            pillarEmpty = new GameObject();

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
                    outputAction = MeleeAttackDeterminer();
                    break;
                    
                case(2):
                    outputAction = RangedAttackDeterminer();
                    break;
                    
                case(3):
                    outputAction = DefenseDeterminer();
                    break;
                    
                case(4):
                    outputAction = MovementDeterminer();
                    break;
                    
                case(5):
                    outputAction = RecoveryDeterminer();
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
            
            if(availableChoices.Count < 1) { 
                A6_Help.DebugPrint(printDebugLogs, "No available defensive moves could be found.");
                return MovementDeterminer();
            }
            else {
                if(A6_Brain.Instance.isEnraged)
                    availableChoices.Remove(enragedMode);
                int randInd = UnityEngine.Random.Range(0,availableChoices.Count - 1);
                outputAction = availableChoices[randInd];                                  
            }

            return outputAction;
        }
        private BossAction MovementDeterminer() { 
            BossAction outputAction = null;
            
            var availableChoices = FetchAvailableChoices(4);

            if(availableChoices.Count < 1) { 
                A6_Help.DebugPrint(printDebugLogs, "No available movement actions could be found.");
                return outputAction;
            }
            else {
                int randInd = UnityEngine.Random.Range(0,availableChoices.Count - 1);
                outputAction = availableChoices[randInd];                                  
            }

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
            //Animation Values
            A6_Help.DebugPrint(printDebugLogs, "RegularSlam invoked.");
            A6_AnimManager.Instance.attInt.SetIntVal(1);
            A6_AnimManager.Instance.meleeBool.SetBool(true);
            A6_AnimManager.Instance.attTrig.SetTrig();
            //Physical Aspects

        }
        public void RegularSwipe() { 
            //Animation Values
            A6_Help.DebugPrint(printDebugLogs, "RegularSwipe invoked.");
            A6_AnimManager.Instance.attInt.SetIntVal(2);
            A6_AnimManager.Instance.meleeBool.SetBool(true);
            A6_AnimManager.Instance.attTrig.SetTrig();
            //Physical Aspects

        }
        public void ComboMixup() { 
            //Animation Values
            A6_Help.DebugPrint(printDebugLogs, "ComboMixup invoked.");
            A6_AnimManager.Instance.attInt.SetIntVal(3);
            A6_AnimManager.Instance.meleeBool.SetBool(true);
            A6_AnimManager.Instance.attTrig.SetTrig();
            //Physical Aspects

        }
        public void ComboFinisher() { 
            //Animation Values
            A6_Help.DebugPrint(printDebugLogs, "ComboFinisher invoked.");
            A6_AnimManager.Instance.attInt.SetIntVal(4);
            A6_AnimManager.Instance.meleeBool.SetBool(true);
            A6_AnimManager.Instance.attTrig.SetTrig();
            //Physical Aspects

        }
//-----------------------------------------------------------------------------------
        //Ranged
        public void ShardSpray() { 
            //Animation Values
            A6_Help.DebugPrint(printDebugLogs, "ShardSpray invoked.");
            A6_AnimManager.Instance.attInt.SetIntVal(1);
            A6_AnimManager.Instance.meleeBool.SetBool(false);
            A6_AnimManager.Instance.attTrig.SetTrig();
            //Physical Aspects

        }
        public void PillarSpread() { 
            //Animation Values
            A6_Help.DebugPrint(printDebugLogs, "PillarsAttack invoked.");
            A6_AnimManager.Instance.attInt.SetIntVal(2);
            A6_AnimManager.Instance.meleeBool.SetBool(false);
            A6_AnimManager.Instance.attTrig.SetTrig();
            //Physical Aspects
            PillarSpawnHandler();
        }
        public void ClosingRing() { 
            //Animation Values
            A6_Help.DebugPrint(printDebugLogs, "ClosingRing invoked.");
            A6_AnimManager.Instance.attInt.SetIntVal(3);
            A6_AnimManager.Instance.meleeBool.SetBool(false);
            A6_AnimManager.Instance.attTrig.SetTrig();
            //Physical Aspects
            ClosingRingHandler();
        }
        public void LeapSwipe() { 
            //Animation Values
            A6_Help.DebugPrint(printDebugLogs, "LeapAttack invoked.");
            A6_AnimManager.Instance.attInt.SetIntVal(4);
            A6_AnimManager.Instance.meleeBool.SetBool(false);
            A6_AnimManager.Instance.attTrig.SetTrig();
            //Physical Aspects

        }
//-----------------------------------------------------------------------------------
//      |> Defenses/Punishments
        public void BloodBarrier() { 
            //Animation Values
            A6_Help.DebugPrint(printDebugLogs, "BloodBarrier invoked.");
            A6_AnimManager.Instance.defInt.SetIntVal(1);
            A6_AnimManager.Instance.defTrig.SetTrig();
            //Physical Aspects

        }
        public void BloodWall() { 
            //Animation Values
            A6_Help.DebugPrint(printDebugLogs, "BloodWall invoked.");
            A6_AnimManager.Instance.defInt.SetIntVal(2);
            A6_AnimManager.Instance.defTrig.SetTrig();
            //Physical Aspects
            BloodWallHandler();
        }
        public void EnragedMode() { 
            //Animation Values
            A6_Help.DebugPrint(printDebugLogs, "EnragedMode invoked.");
            A6_AnimManager.Instance.defInt.SetIntVal(3);
            A6_AnimManager.Instance.defTrig.SetTrig();
            //Physical Aspects

        }
        public void AoEPunish() { 
            //Animation Values
            A6_Help.DebugPrint(printDebugLogs, "AoEPunish invoked.");
            A6_AnimManager.Instance.defInt.SetIntVal(4);
            A6_AnimManager.Instance.defTrig.SetTrig();
            //Physical Aspects

        }
//-----------------------------------------------------------------------------------
//      |> Movement
        public void LeapMove() { 
            //Animation Values
            A6_Help.DebugPrint(printDebugLogs, "LeapMove invoked.");
            A6_AnimManager.Instance.moveInt.SetIntVal(1);
            A6_AnimManager.Instance.moveTrig.SetTrig();
            //Physical Aspects

        }
        public void DashMove() { 
            //Animation Values
            A6_Help.DebugPrint(printDebugLogs, "DashMove invoked.");
            A6_AnimManager.Instance.moveInt.SetIntVal(2);
            A6_AnimManager.Instance.moveTrig.SetTrig();
            //Physical Aspects

        }
//-----------------------------------------------------------------------------------
//      |> Recovery
        public void ReelingBackRecovery() { 
            //Animation Values
            A6_Help.DebugPrint(printDebugLogs, "ReelingBack invoked.");
            A6_AnimManager.Instance.recovInt.SetIntVal(1);
            A6_AnimManager.Instance.recovTrig.SetTrig();
            //Physical Aspects

        }
        public void BrokenBarrierDazeRecovery() { 
            //Animation Values
            A6_Help.DebugPrint(printDebugLogs, "BrokenBarrier invoked.");
            A6_AnimManager.Instance.recovInt.SetIntVal(2);
            A6_AnimManager.Instance.recovTrig.SetTrig();
            //Physical Aspects

        }
        public void EnragedExitRecovery() { 
            //Animation Values
            A6_Help.DebugPrint(printDebugLogs, "EnragedExit invoked.");
            A6_AnimManager.Instance.recovInt.SetIntVal(3);
            A6_AnimManager.Instance.recovTrig.SetTrig();
            //Physical Aspects

        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public void StartCooldown(BossAction actionIn) { StartCoroutine(InteractionCooldown(actionIn)); }

        IEnumerator InteractionCooldown(BossAction actionIn) { 
            actionIn.isReady = false;
            yield return new WaitForSeconds(actionIn.cooldownTime * A6_Brain.Instance.bossTimeScale);
            actionIn.isReady = true;
        }
//-----------------------------------------------------------------------------------
//Individual Action Handlers

        private void ShardSprayHandler() { 
            Vector3 storedPlayerPos = A6_Brain.Instance.playerTransform.position;
            Vector3 directionVector = (storedPlayerPos - A6_Brain.Instance.gameObject.transform.position).normalized;
            //Might want to use a spline to do the shards so they could float around the hand or something while charging up.

            int shardsOut;

            if(A6_Brain.Instance.isEnraged)
                shardsOut = enragedShardAmount;
            else
                shardsOut = normalShardAmount;

//      ||-------|| NEED TO REPLACE WITH OBJECT POOLING ||-------||

            for(int i = 0; i < shardsOut; i++) {

            }

        }

        private void PillarSpawnHandler() { 
            Vector3 storedPlayerPos = A6_Brain.Instance.playerTransform.position;
            activePillars.Clear();
            pillarEmpty.transform.position = storedPlayerPos;
            int pillarsOut;

            if(A6_Brain.Instance.isEnraged)
                pillarsOut = enragedPillarAmount;
            else
                pillarsOut = normalPillarAmount;
            
//      ||-------|| NEED TO REPLACE WITH OBJECT POOLING ||-------||

            for(int i = 0; i < pillarsOut; i++) { 
                float randX = storedPlayerPos.x + UnityEngine.Random.Range(-15, 15);
                float randZ = storedPlayerPos.z + UnityEngine.Random.Range(-15, 15);
                float randY = storedPlayerPos.y - UnityEngine.Random.Range(8, 16);
                Vector3 pillarSpawn = new Vector3(randX, randY, randZ);
                activePillars.Add(Instantiate(pillarPrefab,pillarEmpty.transform));
            }

            foreach(GameObject currentPillar in activePillars) { 
                currentPillar.transform.DOMoveY(currentPillar.transform.position.y + 10, pillarLifetime);
                Destroy(currentPillar, pillarLifetime);
            }
        }

        private void BloodWallHandler() { 
            Vector3 bossFlatY = new Vector3(A6_Brain.Instance.gameObject.transform.position.x, 0, A6_Brain.Instance.gameObject.transform.position.z);
            Vector3 playerFlatY = new Vector3(A6_Brain.Instance.playerTransform.position.x, 0, A6_Brain.Instance.playerTransform.position.z);

            Vector3 directionVec = playerFlatY - bossFlatY;
            directionVec.Normalize();

            float distToPlayer = Vector3.Distance(bossFlatY, playerFlatY);
            if(distToPlayer > 1)
                distToPlayer = distToPlayer * 0.5f;
            else
                distToPlayer = 0.5f;

            Vector3 spawnPos = A6_Brain.Instance.gameObject.transform.position + (directionVec * distToPlayer);

            Destroy(Instantiate(wallPrefab, spawnPos, A6_Brain.Instance.gameObject.transform.rotation), bloodWallLifetime);
        }

        private void ClosingRingHandler() { 
            StartCoroutine(RingUpThenDown());
        }

        IEnumerator RingUpThenDown() { 
            ringPrefab.SetActive(true);
            ringPrefab.transform.DOMoveY(ringPrefab.transform.position.y + 15, 20f);
            yield return new WaitForSeconds(20f * A6_Brain.Instance.bossTimeScale);
            ringPrefab.transform.DOMoveY(ringPrefab.transform.position.y - 15, 10f);
            yield return new WaitForSeconds(10f * A6_Brain.Instance.bossTimeScale);
            ringPrefab.SetActive(false);
            yield return null;
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
