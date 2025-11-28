using Caliodore;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

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

        public virtual void ModifyIncomingDamageByState(Damage dmgIn) 
        {
            float dmgAmt = dmgIn.amount;
        }

        public virtual void CheckDamageToPlayer(Damage dmgOut) 
        { 
            
        }

        /*
         * Could have a switch statement/logic structure to determine how we want to swap states rather than having it be super piecemeal.
         * If it's a base part of the classes then we can also expand upon it easier.
         * Make sure to havea a default case as backup.
         */
        public void StateChangeOffensive()
        { 
            if(C2JR_Brain.BossBrain.playerInMelee)
                C2JR_Brain.BossSM.ChangeState("Attacking");
        }

        public void AddRemoveListeners(UnityEvent targetEvent, bool addOrRemove, UnityAction subscribingListener)
        { 
            if(addOrRemove)
                targetEvent.AddListener(subscribingListener);
            else if (!addOrRemove)
                targetEvent.RemoveListener(subscribingListener);
        }

        public override string ToString()
        {
            return stateName;
        }
    }
}
