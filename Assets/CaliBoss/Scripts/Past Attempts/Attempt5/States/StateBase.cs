using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace CaliBoss
{ 
    public abstract class StateBase : MonoBehaviour
    {
        public StateBase(string nameIn){ stateName = nameIn; }

        public static StateBase Instance;

        //Floats
        protected float currentStateDuration = 0f;

        //Bools
        protected bool isStateStopping;
        protected bool stateComplete;

        //Strings
        public string StateName { get { return stateName; } }
        private string stateName;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            //OnAwake();
        }

        public virtual void OnAwake() { gameObject.SetActive(false); }
        public virtual void OnStateEnter() { currentStateDuration = 0f; stateComplete = false; gameObject.SetActive(true); CSR.Instance.BossBrain.DetermineNextMove(); }
        public virtual void OnUpdate() { currentStateDuration += Time.deltaTime; }
        public virtual void OnStateExit() { stateComplete = true; gameObject.SetActive(false); }
    }
}
