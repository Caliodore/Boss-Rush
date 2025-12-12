using UnityEngine;

namespace Cali7 { 
    public class F7_SIP : MonoBehaviour
    {
        public bool refsLoaded = false;
        [SerializeField] GameObject attachedShard;
        public F7_ShardScript attachedShardScript;
        Vector3 shardLocalPosStart;

        private void Start()
        {
            if(!F7_RefManager.Instance.gotRefs)
                F7_RefManager.OnRefsLoaded?.AddListener(() => SetReferences());
            else
                SetReferences();
        }

        public void SetReferences() { 
            if(attachedShard == null)
                attachedShard = GetComponentInChildren<F7_ShardScript>().gameObject;
            shardLocalPosStart = attachedShard.transform.localPosition;
            
            attachedShardScript.OnFinishTrajectory?.AddListener(() => DisableShard());

            //attachedShard.SetActive(false);
            refsLoaded = true;
        }

        public void EnableShard() { attachedShard.SetActive(true); }
        public void DisableShard() { attachedShard.SetActive(false); attachedShard.transform.SetParent(this.gameObject.transform); attachedShard.transform.localPosition = shardLocalPosStart; }
    }
}