using UnityEngine;
using UnityEngine.Events;

namespace Cali7
{ 
    public class F7_Chase : F7_StateBase
    {
        public F7_Chase(string nameIn) : base("Chase") { }


        public override void OnStateEnter() { 
            base.OnStateEnter();
        }

        public override void OnStateUpdate() { 
            base.OnStateUpdate();
            if (F7_RefManager.BCNT.playerInMelee) { 
                F7_RefManager.BCNT.StateChangeRequest();
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
