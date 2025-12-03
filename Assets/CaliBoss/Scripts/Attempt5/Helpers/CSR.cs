using UnityEngine;
using UnityEngine.AI;

namespace CaliBoss
{ 
    /// <summary>
    /// CSR: Central Serialization Reference.<br/>
    /// Script that holds the majority of serialized references for the boss project.
    /// </summary>
    public class CSR : MonoBehaviour
    {
        public static CSR Instance;

        [Header("Cali Scripts")]
        [SerializeField] public BossBrainBase BossBrain;
        [SerializeField] public StateMachineBase BossSM;
        [SerializeField] public BossActionManager BossAM;

        [Header("Brolive Components")]
        [SerializeField] public Actor BossActor;
        [SerializeField] public Damageable BossDamageable;
        [SerializeField] public Damager BossDamager;
        [SerializeField] public Navigator BossNavigator;
        [SerializeField] public Sensor BossMeleeSensorScript;

        [Header("Player-Centric")]
        [SerializeField] public PlayerLogic PlayerScript;

        [Header("Colliders")]
        [SerializeField] public Collider BossDamagerCollider;
        [SerializeField] public Collider BossMeleeSensorCollider;

        [Header("Rigidbodies")]
        [SerializeField] public Rigidbody BossRigidbody;

        [Header("Misc Components")]
        [SerializeField] public NavMeshAgent BossNMAgent;

        [Header("Animation")]
        [SerializeField] public Animator BossAnimator;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }
    }
}
