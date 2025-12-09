using UnityEngine;
using UnityEngine.Events;

namespace Cali7
{ 
    public class F7_Chase : F7_StateBase
    {
        public F7_Chase() : base("Chase") { }

        public bool requestedChange;

        public override void OnStateEnter() { 
            base.OnStateEnter();
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
            }
        }

        public override void OnStateExit() { 
            base.OnStateExit();
        }


        public override void StopThisState() { }
    }
}
