using UnityEngine;

namespace Cali_4
{ 
    public class BloodBarrier : Defending
    {
        public BloodBarrier() : base("BloodBarrier") { }

        private void Awake()
        {
            StateEntering.AddListener(() => C4_HostBrain.BossAnimManager.bossAnimator.SetTrigger("StartBarrier"));
            StateEntering.AddListener(() => C4_HostBrain.Buster.EnableBloodBarrier(true));

            StateExiting.AddListener(() => C4_HostBrain.BossAnimManager.bossAnimator.SetTrigger("BreakBarrier"));
            StateExiting.AddListener(() => C4_HostBrain.Buster.EnableBloodBarrier(false));
            base.OnAwake();
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}
