using UnityEngine;

namespace Caliodore
{
    public class EmptyState : State
    {
        public override void OnUpdate() { }
        public override void OnStateEnter() { }
        public override void OnStateExit() { }
        public override void StateDurationElapsed() { }
        public override void DamageTaken() { }
        public override void PlayerDamaged() { }
        public override void SetSMRef<T>(BossStateMachine inputBSM) { }
    }
}
