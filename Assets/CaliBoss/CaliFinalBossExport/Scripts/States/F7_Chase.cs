using UnityEngine;
using UnityEngine.Events;

namespace Cali7
{ 
    public class F7_Chase : F7_StateBase
    {
        public F7_Chase(string nameIn) : base("Chase") { }

        public bool requestedChange;

        public override void OnStateEnter() { 
            base.OnStateEnter();
            requestedChange = false;
        }

        public override void OnStateUpdate() { 
            base.OnStateUpdate();
            if (F7_RefManager.BCNT.playerInMelee && !requestedChange) { 
                F7_RefManager.BCNT.StateChangeRequest();
                requestedChange = true;
            }
            else { 
                //F7_RefManager.BCNT.isMoving = true;
                F7_RefManager.BNMA.SetDestination(F7_RefManager.PLGS.gameObject.transform.position);
            }
        }

        public override void OnStateExit() { 
            base.OnStateExit();
        }


        public override void StopThisState() { }
    }
}
