using UnityEngine;
using System.Collections;

namespace Cali7 { 
    public class F7_BloodBarrier : MonoBehaviour
    {
        public bool refsLoaded = false;
        public bool printDebugLogs = true;
        public bool barrierActive = false;
        private ParticleSystem attPS;
        public Damageable barrierDmg;
        public int timesHit;
        public Vector3 defaultScale;

        private void Start()
        {
            if(!F7_RefManager.Instance.gotRefs)
                F7_RefManager.OnRefsLoaded?.AddListener(() => SetReferences());
            else
                SetReferences();
        }

        public void SetReferences() {
            timesHit = 0;
            defaultScale = transform.localScale;
            barrierDmg = GetComponent<Damageable>();
            attPS = GetComponent<ParticleSystem>();
            barrierDmg.OnHit?.AddListener(dmgIn => BarrierHitWhileActive(dmgIn));
            F7_RefManager.BEVM.OnBarrierBreak?.AddListener(() => BarrierBreak()); 
            refsLoaded = true;
        }

        private void Update()
        {
            if(refsLoaded) { 
                if(barrierActive) { 
                    Vector3 rotOut = Vector3.zero;
                    rotOut.y += F7_RefManager.BPSO.barrierSpinSpeed * Time.deltaTime;
                    transform.localEulerAngles = rotOut;
                }
            }
        }

        public void ActivateBarrier() { 
            timesHit = 0;
            barrierActive = true;
            gameObject.SetActive(true);
        }

        public void BarrierHitWhileActive(Damage dmgIn) { 
            if((F7_RefManager.BSTM.CurrentState == F7_RefManager.BSTD) && barrierActive) { 
                Damage dmgOut = dmgIn;
                int amtOut = 0;
                timesHit++;
                if(timesHit < F7_RefManager.BPSO.hitsUntilBarrierBreak) { 
                    amtOut = dmgIn.amount/2;
                    dmgOut.amount = amtOut;
                    F7_RefManager.BDGL.OnHit?.Invoke(dmgOut);
                }
                else { 
                    amtOut = dmgIn.amount * 3;
                    dmgOut.amount = amtOut;
                    F7_RefManager.BDGL.OnHit?.Invoke(dmgOut);
                    F7_RefManager.BEVM.OnBarrierBreak?.Invoke();
                }
            }
            else { 
                F7_Help.DebugPrint(printDebugLogs, "BloodBarrier registered a hit while not in defending or barrier not active.");
            }
        }

        public void BarrierBreak() {
            attPS.Play();
            StartCoroutine(PopBarrier());
        }

        IEnumerator PopBarrier() { 
            bool barrierBreaking = true;
            Vector3 newScale = defaultScale;
            float scaleAmt = 1f;

            while(barrierBreaking) { 
                scaleAmt += Time.deltaTime;
                newScale *= scaleAmt;
                transform.localScale = newScale;
                if(scaleAmt >= 1.25)
                    barrierBreaking = false;
                yield return null;
            }

            if(!barrierBreaking) { 
                gameObject.SetActive(false);
                transform.localEulerAngles = Vector3.zero;
                F7_RefManager.BDGL.gameObject.SetActive(true);
            }
        }

    }
}