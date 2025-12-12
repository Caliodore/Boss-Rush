using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Cali7
{ 
    public class F7_PillarScript : MonoBehaviour
    {
        public bool doneRaising;
        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void StartRising() { transform.DOMoveY(3,F7_RefManager.BPSO.pillarRaiseSpeed); StartCoroutine(WaitForTop()); }
        public void StartLowering() { transform.DOMoveY(-9,F7_RefManager.BPSO.pillarLoweringSpeed); StartCoroutine(WaitToDisable()); }

        IEnumerator WaitForTop() { 
            bool isRaising = true;
            doneRaising = false;
            while(isRaising) {
                if(transform.position.y >= 3) { 
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
                if(transform.position.y <= -9) { 
                    isLowering = false;
                }
                yield return null;
            }
            if(!isLowering)
                gameObject.SetActive(false);
        }
    }
}