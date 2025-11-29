using UnityEngine;

namespace Cali_4
{ 
    public class C4_StateDeterminant : MonoBehaviour
    {
        public static C4_StateDeterminant Instance;
        private StC4 previousState;

        public bool printDebugs;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        public void RequestStateChange(StC4 requestedState)
        { 
            //previousState = C4_HostBrain.BossSM.current
        }

        public StC4 DetermineNextState(StC4 stateFrom)
        {
            StC4 outputState = null;

            switch(stateFrom.GetType().BaseType.ToString())
            { 
                case("Aggro"):
                    /*
                     * A case inheriting from Aggro, so the next intended state should be Transition, as this will be after an attack is finishing.
                     */
                    break;

                case("Defending"):
                    /*
                     * A case inheriting from Defending, so the next intended state should be Transition, as this will be after taking enough blows and retaliating or timing out.
                     */
                    break;

                case("Transition"):
                    /*
                     * A case inheriting from Transition, the intended next states are gonna be a large set of methods/switches themselves.
                     * A lot of these inherited states should have explicit exit conditions and next states, with this method being a fallback.
                     */

                    break;

                case("Moving"):
                    /*
                     * A case inheriting from Moving, so the next intended state should be Transition almost immediately into Aggro in most cases.
                     * Similar to Transition, it is unlikely that the exit conditions and next states won't be set up per state but this is a fallback.
                     */
                    break;

                case(""):
                    /*One of the generic cases which will be determined below in associated methods, if not explicitly set within the state.
                      These shouldn't be directly instantiated.*/
                default:
                    /*
                     * Fallback case
                     */
                    break;
            }

            return outputState;
        }

        public StC4 DetermineNextAttack()
        { 
            StC4 outputState = null;
            bool startOfCombo = C4_HostBrain.Buster.CurrentCombo == 0;
            bool endOfCombo = C4_HostBrain.Buster.CurrentCombo >= C4_HostBrain.Buster.MaxCombo - 1;
            int randomAttInt = Random.Range(0,2);
            bool isPunishment = false;
            string stateNameRef = "Aggro";
            string transSt = "TransitionToAttack";


            if(C4_HostBrain.Buster.isPlayerClose || (C4_HostBrain.Buster.isPlayerInMeleeSensor && C4_HostBrain.Buster.isPlayerClose))
            {
                if(randomAttInt < 2)
                            stateNameRef += "Slam";
                        else
                            stateNameRef += "Swipe";
                //outputState = C4_StateMachine.AllStates.Find(guh => guh.name);
                if(startOfCombo)
                {
                    outputState = C4_StateMachine.GetStateRef(stateNameRef);
                }
                else if(!endOfCombo)
                { 
                    if(randomAttInt < 2)
                        outputState = C4_StateMachine.GetStateRef(stateNameRef);
                    else
                        outputState = C4_StateMachine.GetStateRef(stateNameRef);
                }
                else
                { 
                    outputState = C4_StateMachine.GetStateRef(stateNameRef);
                }
            }
            else //Ranged attacks
            {
                if(randomAttInt < 2)
                            stateNameRef += "PillarsSpread";
                        else
                            stateNameRef += "ShardSpray";

                outputState = C4_StateMachine.GetStateRef(stateNameRef);
            }

            if(isPunishment)
                transSt += "Punish";
            else
                transSt += "Regular";

            C4_StateMachine.GetStateRef(transSt).SetNextState(outputState);
            outputState = C4_StateMachine.GetStateRef(transSt);

            return outputState;
        }
    }
}
