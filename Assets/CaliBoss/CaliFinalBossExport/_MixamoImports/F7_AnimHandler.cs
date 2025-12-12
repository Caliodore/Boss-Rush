using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Cali7
{ 
    public class F7_AnimHandler : MonoBehaviour
    {
        public bool printDebugLogs = true;
        public static F7_AnimHandler Instance;
        public Transform meleePivotTransform;
        public Quaternion meleePivotDefaultRotation;
        private void Start()
        {
            if(!F7_RefManager.Instance.gotRefs)
                F7_RefManager.OnRefsLoaded?.AddListener(() => SetReferences());
            else
                SetReferences();
        }

        public void SetReferences() { 
            meleePivotDefaultRotation = meleePivotTransform.localRotation;
            F7_RefManager.BEVM.OnStartAttack?.AddListener(() => ResetPivotRotation());
        }

        public void AnimationLineEnded() { F7_RefManager.BANM.EndOfAnimationLine(); }
        public void EnableClaws() { F7_RefManager.GOSW.SetActive(true); }
        public void DisableClaws() { F7_RefManager.GOSW.SetActive(false); }
        public void EnableSlammer() { F7_RefManager.GOSL.SetActive(true); }
        public void DisableSlammer() { F7_RefManager.GOSL.SetActive(false); }
        public void EnableFists() { F7_RefManager.GORP.SetActive(true); F7_RefManager.GOLP.SetActive(true); }
        public void DisableFists() { F7_RefManager.GORP.SetActive(false); F7_RefManager.GOLP.SetActive(false); }
        public void StartRotatingSlammer() { F7_RefManager.GOMP.transform.DOLocalRotate(new Vector3(89,0,0), 0.37f); }
        public void StartRotatingClaws() { F7_RefManager.GOMP.transform.DOLocalRotate(new Vector3(0,-179,0), 0.35f); }
        public void StartThisShard(int shardNum) { F7_RefManager.GOSA[shardNum].GetComponent<F7_SIP>().EnableShard(); }
        public void ShardsReady() { F7_EventManager.Instance.OnShardsReady?.Invoke(); F7_Help.DebugPrint(printDebugLogs, "ShardsReady Invoked by AnimHandler."); }
        public void ResetPivotRotation() { meleePivotTransform.localRotation = meleePivotDefaultRotation; }
    }
}
