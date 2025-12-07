using DG.Tweening;
using UnityEngine;

namespace Cali7
{ 
    public class F7_AnimHandler : MonoBehaviour
    {
        public void AnimationLineEnded() { F7_RefManager.BANM.EndOfAnimationLine(); }
        public void EnableClaws() { F7_RefManager.GOSW.SetActive(true); }
        public void DisableClaws() { F7_RefManager.GOSW.SetActive(false); }
        public void EnableSlammer() { F7_RefManager.GOSL.SetActive(true); }
        public void DisableSlammer() { F7_RefManager.GOSL.SetActive(false); }
        public void StartRotatingSlammer() { F7_RefManager.GOLP.transform.DORotate(new Vector3(90,0,0), 11f); }
    }
}
