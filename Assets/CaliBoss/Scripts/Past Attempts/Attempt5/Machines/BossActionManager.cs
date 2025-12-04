using Cali_4;
using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static CaliBoss.DefenseMethodStorage;
using static CaliBoss.MeleeAttackMethodStorage;
using static CaliBoss.RangedAttackMethodStorage;
using static CaliBoss.RecoveryMethodStorage;
using static CaliBoss.MoveMethodStorage;

namespace CaliBoss
{ 
    public class BossActionManager : MonoBehaviour
    {
        public static BossActionManager Instance;
        public BossActionManager() { }

        public List<Delegate> availableBossMoves = new();
        public List<Delegate> movesOnCooldown = new();
        //public Dictionary<Type,Delegate> bossMoveDict = new();

        public MeleeAttackMethodStorage meleeAttacks;
        public RangedAttackMethodStorage rangedAttacks;
        public DefenseMethodStorage defenseTypes;
        public MoveMethodStorage movementTypes;
        public RecoveryMethodStorage recoveryTypes;

        public delegate void guhGoo();

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            meleeAttacks ??= new();
            rangedAttacks ??= new();
            defenseTypes ??= new();
            movementTypes ??= new();
            recoveryTypes ??= new();
        }

        private void Start()
        {
            GenerateLists();
        }

        private void GenerateLists()
        { 
            foreach(MeleeAttack attackCall in meleeAttacks.MeleeAttackChoices) { 
                //print(attackCall.Method.Name + " and " + (typeof(MeleeAttack).ToString()));
                availableBossMoves.Add(attackCall);
                //bossMoveDict.Add(typeof(MeleeAttack), attackCall);
            }
            foreach(RangedAttack attackCall in rangedAttacks.RangedAttackChoices) { 
                availableBossMoves.Add(attackCall);
                //bossMoveDict.Add(typeof(RangedAttack), attackCall);
            }
            foreach(DefenseMove attackCall in defenseTypes.DefenseMoveChoices) { 
                availableBossMoves.Add(attackCall);
                //bossMoveDict.Add(typeof(DefenseMove), attackCall);
            }
            foreach(MovementType attackCall in movementTypes.MovementChoices) { 
                availableBossMoves.Add(attackCall);
                //bossMoveDict.Add(typeof(MovementType), attackCall);
            }
            foreach(RecoveryType attackCall in recoveryTypes.RecoveryTypeChoices) { 
                availableBossMoves.Add(attackCall);
                //bossMoveDict.Add(typeof(RecoveryType), attackCall);
            }
        }
        
        //Recovering and Pursuing states in here might be redundant but overall the structure is important and useful.

        public (bool movesAvailable, Delegate selectedMove) CheckForAvailableMoves() { 

            bool isMelee = BossBrainBase.MainBrain.PlayerIsInMelee;
            var correspondingMoves = availableBossMoves.FindAll(x => x.GetType().GetProperty("CorrespondingState").Equals(CSR.Instance.BossSM.CurrentState));
            Delegate delOut = null;

            print($"Found {correspondingMoves.Count} entries available for calling for {CSR.Instance.BossSM.CurrentState} as the current state.");

            List<Delegate> possibleReactions = new();

            if(correspondingMoves.Count <= 0) { return (false, delOut); }
            else{ 
                switch(CSR.Instance.BossSM.CurrentState) { 
                    case(Attacking):
                        if(isMelee) { 
                            print("Melee Attacking");
                            possibleReactions = correspondingMoves.FindAll(x => x.GetType().Equals(typeof(MeleeAttack)));
                            delOut = ChooseRandomInList(possibleReactions);
                        }
                        else {
                            print("Ranged Attacking");
                            possibleReactions = correspondingMoves.FindAll(x => x.GetType().Equals(typeof(RangedAttack)));
                            delOut = ChooseRandomInList(possibleReactions);
                        }
                        break;
                        
                    case(Defending):
                            possibleReactions = correspondingMoves.FindAll(x => x.GetType().Equals(typeof(DefenseMove)));
                            delOut = ChooseRandomInList(possibleReactions);
                        break;

                    case(Pursuing):
                        if(isMelee) { 
                            print("Melee Pursuing");
                            goto default;
                        }
                        else {
                            print("Ranged Pursuing");
                            possibleReactions = correspondingMoves.FindAll(x => x.GetType().Equals(typeof(MovementType)));
                            delOut = ChooseRandomInList(possibleReactions);
                        }
                        break;

                    case(Recovering):
                            possibleReactions = correspondingMoves.FindAll(x => x.GetType().Equals(typeof(RecoveryType)));
                            delOut = ChooseRandomInList(possibleReactions);
                        break;
                        
                    case(Entering):
                    default:
                        if(isMelee) { 
                            print("Melee Entering");
                            delOut = AttemptChangingState(CSR.Instance.BossSM.GetStateRef<Attacking>());
                        }
                        else {
                            print("Ranged Entering");
                            delOut = AttemptChangingState(CSR.Instance.BossSM.GetStateRef<Pursuing>());
                        }
                        break;

                }
                PutMoveOnCooldown(delOut);
                return (true,delOut);
            }
        }

        private void PutMoveOnCooldown(Delegate methodUsed) { 
            availableBossMoves.Remove(methodUsed);
            movesOnCooldown.Add(methodUsed);
            print($"Delegate {methodUsed.ToString()} is being put on cooldown.");
            StartCoroutine(MoveCooldownTimer(methodUsed));
        }

        IEnumerator MoveCooldownTimer(Delegate methodCooling) {
            yield return new WaitForSeconds(6f);
            print($"Delegate {methodCooling.ToString()} is being taken off of cooldown.");
            movesOnCooldown.Remove(methodCooling);
            availableBossMoves.Add(methodCooling);
            yield return null;
        }

        private Delegate ChooseRandomInList(List<Delegate> listIn)
        { 
            int randomIndex = UnityEngine.Random.Range(0, listIn.Count);
            return listIn[randomIndex];
        }

        private Delegate AttemptChangingState(StateBase stateTo)
        {
            guhGoo eepOut = null;
            eepOut = () => CSR.Instance.BossSM.RequestStateChange(stateTo);
            return eepOut;
        }
    }

    //Prospective Cleaning Up Plan: Store the methods within classes and use delegates to store those within lists for better specificity.
    public class MeleeAttackMethodStorage {
        public delegate void MeleeAttack();

        public Attacking CorrespondingState;

        public static MeleeAttack RegularSwipe;
        public static MeleeAttack RegularSlam;
        public static MeleeAttack ComboMixup;
        public static MeleeAttack ComboFinisher;

        public List<MeleeAttack> MeleeAttackChoices = new();

        public MeleeAttackMethodStorage() 
        {
            RegularSwipe = (() => BossBrainBase.MainBrain.StartRegularSwipe());
            RegularSlam = () => BossBrainBase.MainBrain.StartRegularSlam();
            ComboMixup = () => BossBrainBase.MainBrain.StartComboMixup();
            ComboFinisher = () => BossBrainBase.MainBrain.StartComboFinisher();
            var meleeList = new List<MeleeAttack>() { RegularSwipe, RegularSlam, ComboMixup, ComboFinisher};
            MeleeAttackChoices.AddRange(meleeList);
        }

        public override string ToString()
        {
            return nameof(MeleeAttack);
        }
    }

    public class RangedAttackMethodStorage { 
        public delegate void RangedAttack();

        public Attacking CorrespondingState;

        public RangedAttack ShardSpray;
        public RangedAttack PillarsAttack;
        public RangedAttack ClosingRing;
        public RangedAttack LeapAttack;

        public List<RangedAttack> RangedAttackChoices = new();

        public RangedAttackMethodStorage()
        {
            ShardSpray = () => BossBrainBase.MainBrain.StartShardSpray();
            PillarsAttack = () => BossBrainBase.MainBrain.StartPillarsAttack();
            ClosingRing = () => BossBrainBase.MainBrain.StartClosingRing();
            LeapAttack = () => BossBrainBase.MainBrain.StartLeapAttack();
            var inputList = new List<RangedAttack>() { ShardSpray, PillarsAttack, ClosingRing, LeapAttack };
            RangedAttackChoices.AddRange(inputList);
        }
        
    }

    public class DefenseMethodStorage { 
        public delegate void DefenseMove();
        
        public Defending CorrespondingState;

        public List<DefenseMove> DefenseMoveChoices = new();

        public DefenseMove BloodBarrier;
        public DefenseMove BloodWall;
        public DefenseMove EnragedMode;
        public DefenseMove AoEPunish;


        public DefenseMethodStorage() 
        { 
            BloodBarrier = () => BossBrainBase.MainBrain.StartBloodBarrier();
            BloodWall = () => BossBrainBase.MainBrain.StartBloodWall();
            EnragedMode = () => BossBrainBase.MainBrain.StartEnragedMode();
            AoEPunish = () => BossBrainBase.MainBrain.StartAoEPunish();
            var inputList = new List<DefenseMove>() { BloodBarrier, BloodWall, EnragedMode, AoEPunish };
            DefenseMoveChoices.AddRange(inputList);
        }
    }

    public class MoveMethodStorage
    { 
        public delegate void MovementType();
        
        public Pursuing CorrespondingState;

        public List<MovementType> MovementChoices = new();

        public MovementType LeapMove;
        public MovementType DashMove;
        
        public MoveMethodStorage() { 
            LeapMove = () => BossBrainBase.MainBrain.StartLeapMove();
            DashMove = () => BossBrainBase.MainBrain.StartDashMove();
            var inputList = new List<MovementType>() { LeapMove, DashMove };
            MovementChoices.AddRange(inputList);
        }
    }

    public class RecoveryMethodStorage
    { 
        public delegate void RecoveryType();
        
        public Recovering CorrespondingState;

        public List<RecoveryType> RecoveryTypeChoices = new();

        public RecoveryType ReelingBack;
        public RecoveryType BarrierDaze;
        public RecoveryType EnragedExit;
        
        public RecoveryMethodStorage() { 
            ReelingBack = () => BossBrainBase.MainBrain.StartReelingBackRecovery();
            BarrierDaze = () => BossBrainBase.MainBrain.StartBrokenBarrierDazeRecovery();
            EnragedExit = () => BossBrainBase.MainBrain.StartEnragedExitRecovery();
            var inputList = new List<RecoveryType>() { ReelingBack, BarrierDaze, EnragedExit };
            RecoveryTypeChoices.AddRange(inputList);
        }
    }
}
