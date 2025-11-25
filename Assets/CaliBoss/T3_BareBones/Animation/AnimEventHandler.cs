using UnityEngine;

namespace Cali3
{ 
    public class AnimEventHandler : MonoBehaviour
    {
        public void StartAttackRecovery()
        { 
            T3_Brain.MainBrain.AnimEventTarget(true);    
        }
        public void FinishAttackRecovery()
        { 
            T3_Brain.MainBrain.AnimEventTarget(false);    
        }
    }
}
