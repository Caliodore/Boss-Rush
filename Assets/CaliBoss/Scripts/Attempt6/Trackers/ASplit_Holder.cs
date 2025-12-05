using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Cali6
{ 
    public class ASplit_Holder : MonoBehaviour
    {
        [SerializeField] public List<ArenaSplitTracker> arenaScripts = new();
        public static ArenaSplitTracker playerCurrentCollider;

        public UnityEvent NewSplitEntered;

        private void Start()
        {
            NewSplitEntered ??= new();
            NewSplitEntered.AddListener(() => ChangeCurrentCollider());
        }

        public void ChangeCurrentCollider() { 
            foreach(ArenaSplitTracker indScript in arenaScripts) {
                if (indScript.playerInArea) {
                    playerCurrentCollider = indScript;
                }
            }
        }

        public void SetPillarLocale() { }

    }
}
