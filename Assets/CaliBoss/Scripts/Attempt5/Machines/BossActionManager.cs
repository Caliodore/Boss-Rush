using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace CaliBoss
{ 
    public class BossActionManager : MonoBehaviour
    {
        public static BossActionManager Instance;
        public BossActionManager() { }

        delegate void ActionMethod();
        private List<ActionMethod> availableBossMoves = new();
        private List<ActionMethod> movesOnCooldown = new();
        private Dictionary<StateBase,ActionMethod> bossMoveDict = new();
        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            GenerateLists();
        }

        private void GenerateLists()
        { 
            var attackRef = StateMachineBase.Instance.GetStateRef(Attacking.AttInstance);
            var defendRef = StateMachineBase.Instance.GetStateRef(Defending.DefendInstance);
            var moveRef = StateMachineBase.Instance.GetStateRef(Pursuing.PursInstance);
            var recoveryRef = StateMachineBase.Instance.GetStateRef(Recovering.RecovInstance);

            availableBossMoves.Clear();
            availableBossMoves.Add(StartRegularSwipe);
            availableBossMoves.Add(StartRegularSlam);
            availableBossMoves.Add(StartComboMixup);
            availableBossMoves.Add(StartComboFinisher);
            availableBossMoves.Add(StartShardSpray);
            availableBossMoves.Add(StartPillarsAttack);
            availableBossMoves.Add(StartClosingRing);
            availableBossMoves.Add(StartLeapAttack);
            for(int i = 0; i < 8; i++) {
                bossMoveDict.Add(attackRef, availableBossMoves[i]);
            }

            availableBossMoves.Add(StartBloodBarrier);
            availableBossMoves.Add(StartBloodWall);
            availableBossMoves.Add(StartEnragedMode);
            availableBossMoves.Add(StartAoEPunish);
            for(int i = 8; i < 12; i++) { 
                bossMoveDict.Add(defendRef, availableBossMoves[i]);
            }

            availableBossMoves.Add(StartLeapMove);
            availableBossMoves.Add(StartDashMove);
            for(int i = 12; i < 14; i++) { 
                bossMoveDict.Add(moveRef, availableBossMoves[i]);
            }

            availableBossMoves.Add(StartReelingBackRecovery);
            availableBossMoves.Add(StartBrokenBarrierDazeRecovery);
            availableBossMoves.Add(StartEnragedExitRecovery);
            for(int i = 14; i < 17; i++) { 
                bossMoveDict.Add(recoveryRef, availableBossMoves[i]);
            }
        }

        //  Specific Action Methods
        //      |>Attacks
        public void StartRegularSwipe() { 
            
        }
        public void StartRegularSlam() { 
            
        }
        public void StartComboMixup() { 
            
        }
        public void StartComboFinisher() { 
            
        }
        public void StartShardSpray() { 
            
        }
        public void StartPillarsAttack() { 
            
        }
        public void StartClosingRing() { 
            
        }
        private void StartLeapAttack() { 
            
        }

//      |> Defenses/Punishments
        private void StartBloodBarrier() { 
            
        }
        private void StartBloodWall() { 
            
        }
        private void StartEnragedMode() { 
            
        }
        private void StartAoEPunish() { 
            
        }

//      |> Movement
        private void StartLeapMove() { 
            
        }
        private void StartDashMove() { 
            
        }

//      |> Recovery
        private void StartReelingBackRecovery() { 
            
        }
        private void StartBrokenBarrierDazeRecovery() { 
            
        }
        private void StartEnragedExitRecovery() { 
            
        }
        
        public bool CheckForAvailableMoves(StateBase moveType)
        { 
            var correspondingMoves = availableBossMoves.FindAll(x => bossMoveDict.TryGetValue(moveType, out x));
            if(correspondingMoves.Count <= 0) { return false; }
            else { 
                int randChoice = UnityEngine.Random.Range(0, correspondingMoves.Count - 1);
                correspondingMoves[randChoice].Invoke();
                PutMoveOnCooldown(moveType, correspondingMoves[randChoice]);
                return true;
            }
        }

        private void PutMoveOnCooldown(StateBase moveType, ActionMethod methodUsed) { 
            availableBossMoves.Remove(methodUsed);
            movesOnCooldown.Add(methodUsed);
            StartCoroutine(MoveCooldownTimer(moveType, methodUsed));
        }

        IEnumerator MoveCooldownTimer(StateBase moveType, ActionMethod methodCooling)
        {
            yield return new WaitForSeconds(3f);
            movesOnCooldown.Remove(methodCooling);
            availableBossMoves.Add(methodCooling);
            yield return null;
        }
    }
}
