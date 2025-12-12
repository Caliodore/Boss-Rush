using UnityEngine;
using UnityEngine.Events;

namespace Cali7
{ 
    public class F7_Chase : F7_StateBase
    {
        public F7_Chase() : base("Chase") { }

        public bool printDebugLogs = true;
        public bool requestedChange;
        public float randomTime;

        public override void OnStateEnter() { 
            base.OnStateEnter();
            F7_RefManager.BEVM.OnStartMoving?.Invoke();
            requestedChange = false;
            randomTime = UnityEngine.Random.Range(2,6);
        }

        public override void OnStateUpdate() { 
            base.OnStateUpdate();
            if (F7_RefManager.BCNT.playerInMelee) { 
                F7_RefManager.BCNT.StateChangeRequest();
            }
            else { 
                F7_RefManager.BCNT.isMoving = true;
                F7_RefManager.BNMA.SetDestination(F7_RefManager.PLGS.gameObject.transform.position);

                Vector3 dirVec = F7_RefManager.PLGS.gameObject.transform.position - gameObject.transform.position;
                dirVec.y = 0;
                dirVec.Normalize();
                F7_RefManager.BANM.gameObject.transform.rotation.SetLookRotation(dirVec);

                if(currentStateDuration >= randomTime && !requestedChange)
                    RequestChange();
            }
        }

        private void RequestChange() { 
            requestedChange = true;
            F7_RefManager.BCNT.StateChangeRequest(); 
        }

        public override void OnStateExit() { 
            base.OnStateExit();
            F7_RefManager.BEVM.OnStopMoving?.Invoke();
            F7_RefManager.BNMA.SetDestination(F7_RefManager.BGOJ.transform.position);
        }


        public override void StopThisState() { }
    }
}
