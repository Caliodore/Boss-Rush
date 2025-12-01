using UnityEngine;

namespace Cali_4
{
    public class AnimStateDeterminer : StateMachineBehaviour
    {
        public int stateType;
        public bool printDebugs = true;
        // OnStateMachineEnter is called when entering a state machine via its Entry Node
        override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            stateType = C4_HostBrain.BossAnimManager.bossAnimator.GetInteger("CurrentState");
            Helpers.DebugPrint($"Direct animator property reference of CurrentState: {stateType}\nStateMachinePathHash output: {stateMachinePathHash}",printDebugs);
            switch(stateType)
            { 
                case(1):
                    //Go to aggro
                    break;
                    
                case(2):
                    //Go to movement
                    break;

                case(3):
                    //Go to defending
                    break;

                case(0):
                default:
                    //Go to transition
                    break;

            }
        }

        // OnStateMachineExit is called when exiting a state machine via its Exit Node
        //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        //{
        //    
        //}
    }
}
