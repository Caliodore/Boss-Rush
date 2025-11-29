using UnityEngine;

namespace Cali_4
{ 
    public class ToAttackRegular : Transition
    {
        public ToAttackRegular() : base("ToAttack") { }

        public override void OnAwake()
        { 
            transitionDuration = C4_HostBrain.Buster.regularAttackTransitionTimer;    
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
        }
    }
}
