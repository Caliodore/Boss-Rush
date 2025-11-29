using UnityEngine;

namespace Cali_4
{ 
    public class ToAttackPunish : Transition
    {
        public ToAttackPunish() : base("ToAttack") { }

        public override void OnAwake()
        { 
            transitionDuration = C4_HostBrain.Buster.punishAttackTransitionTimer;    
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
        }
    }
}
