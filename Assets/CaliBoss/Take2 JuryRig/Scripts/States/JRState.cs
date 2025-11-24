using Caliodore;
using UnityEngine;
using UnityEngine.Events;

namespace CaliJR
{ 
    public class JRState : MonoBehaviour
    {
        [Header("Consistent Vars")]
        public string stateName = "";
        public bool stateComplete;

        [Header("Protected Vars")]
        public float currentStateDuration;

        public UnityEvent HitPlayerSuccess;
        public UnityEvent DamagedByPlayer;

        public JRState(string name) { stateName += name; }

        private void Awake()
        {
            HitPlayerSuccess.AddListener(() => C2JR_StateMachine.BossSM.InvokePlayerHitSuccess(this));
            DamagedByPlayer.AddListener(() => C2JR_StateMachine.BossSM.InvokeDamagedByPlayer(this));
        }

        public virtual void OnStateEnter() 
        {
            stateComplete = false;
            print($"Entered state: {stateName}");
            currentStateDuration = 0;
        }
        public virtual void OnUpdate() 
        { 
            currentStateDuration += Time.deltaTime;
        }
        public virtual void OnStateExit() 
        { 
            stateComplete = true;    
        }

        public virtual void DamageTaken() 
        { 
            
        }

        public virtual void PlayerDamaged() 
        { 
            
        }

        public override string ToString()
        {
            return stateName;
        }
    }
}
