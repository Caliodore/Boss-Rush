using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Cali7
{ 
    public class F7_StateDeterminer : MonoBehaviour
    {
        public static F7_StateDeterminer Instance;

        private F7_StateBase stateFrom;
        private F7_StateBase stateTo;

        private List<F7_StateBase> possibleStates = new();

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        public F7_StateBase DetermineNextState() {
            return F7_RefManager.BSTC;
        }
    }
}
