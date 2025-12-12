using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Cali7 { 
    public class F7_PillarHolder : MonoBehaviour
    {
        public bool printDebugLogs = true;
        public static F7_PillarHolder Instance;
        [SerializeField] List<GameObject> pillarsHeld;
        [SerializeField] List<F7_PillarScript> pillarScripts;
        public int pillarsDoneRising;
        public float pillarsUpperLimit = -5f;
        public float pillarsLowerLimit = -20f;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        private void Start()
        {
            SetChildrenRefs();
        }

        private void SetChildrenRefs() { 
            foreach(F7_PillarScript indSc in pillarScripts) { 
                indSc.SetParentRef(this);
                indSc.gameObject.transform.position = new Vector3(0, pillarsLowerLimit, 0);
                indSc.gameObject.SetActive(false);
            }
        }

        public void ActivatePillars() {
            pillarsDoneRising = 0;
            StartCoroutine(PillarRaiseLimiter());
            //StartCoroutine(WaitToStartCooldown());
        }

        IEnumerator PillarRaiseLimiter() { 
            int currentIndex = 0;
            while(currentIndex < pillarsHeld.Count) { 
                pillarsHeld[currentIndex].SetActive(true);
                pillarScripts[currentIndex].StartRising();
                currentIndex++;
                yield return new WaitForSeconds(F7_RefManager.BPSO.pillarFirerate);
            }
            if(currentIndex >= pillarsHeld.Count)
                F7_Help.DebugPrint(printDebugLogs, "Each pillar has been signalled to start raising");
        }

        /*IEnumerator WaitToStartCooldown() { 
            bool allPillarsDone = pillarsDoneRising == pillarsHeld.Count;
            while(!allPillarsDone) { 
                yield return null;
            }
            if(allPillarsDone)
                F7_EventManager.Instance.OnLastPillarDone?.Invoke();
        }*/

        public void SetPillarPos(int pillInd, Vector3 toPos) {
            pillarsHeld[pillInd].transform.position = toPos;
        }
    }
}