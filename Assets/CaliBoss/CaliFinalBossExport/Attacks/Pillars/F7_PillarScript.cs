using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Cali7
{ 
    public class F7_PillarScript : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void StartRising() { transform.DOMoveY(3,F7_RefManager.BPSO.pillarRaiseSpeed); }
        public void StartLowering() { transform.DOMoveY(-9,F7_RefManager.BPSO.pillarLoweringSpeed); StartCoroutine(WaitToDisable()); }

        IEnumerator WaitToDisable() { 
            bool isLowering = true;
            while (isLowering) {
                if(transform.position.y == -9) { 
                    isLowering = false;
                }
                yield return null;
            }
            if(!isLowering)
                gameObject.SetActive(false);
        }
    }
}