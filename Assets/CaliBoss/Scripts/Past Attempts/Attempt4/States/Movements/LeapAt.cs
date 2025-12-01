using UnityEngine;

namespace Cali_4
{ 
    public class LeapAt : Moving
    {
        public LeapAt(string nameIn) : base("LeapAt") { stateName += nameIn; }

        private void Awake()
        {
            StateEntering.AddListener(() => C4_HostBrain.BossAnimManager.bossAnimator.SetTrigger("StartLeap"));
            StateEntering.AddListener(() => C4_HostBrain.Buster.TurnToPlayer(false));
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

        public override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}
