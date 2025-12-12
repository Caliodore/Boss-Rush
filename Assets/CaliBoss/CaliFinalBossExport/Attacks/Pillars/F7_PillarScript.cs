using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Cali7
{ 
    public class F7_PillarScript : MonoBehaviour
    {
        public bool doneRaising;
        public F7_PillarHolder parentScript;

        public void SetParentRef(F7_PillarHolder refIn) { 
            parentScript = refIn;
        }

        public void StartRising() { transform.DOMoveY(parentScript.pillarsUpperLimit,F7_RefManager.BPSO.pillarRaiseSpeed); StartCoroutine(WaitForTop()); }
        public void StartLowering() { transform.DOMoveY(parentScript.pillarsLowerLimit,F7_RefManager.BPSO.pillarLoweringSpeed); StartCoroutine(WaitToDisable()); }

        IEnumerator WaitForTop() { 
            bool isRaising = true;
            doneRaising = false;
            while(isRaising) {
                if(transform.position.y >= parentScript.pillarsUpperLimit) { 
                    isRaising = false;
                }
                yield return null;
            }
            if(!isRaising) { 
                doneRaising = true;
                F7_PillarHolder.Instance.pillarsDoneRising++;
                yield break;
            }
        }

        IEnumerator WaitToDisable() { 
            bool isLowering = true;
            while (isLowering) {
                if(transform.position.y <= parentScript.pillarsLowerLimit) { 
                    isLowering = false;
                }
                yield return null;
            }
            if(!isLowering)
                gameObject.SetActive(false);
        }
    }
}