using UnityEngine;
using UnityEngine.AI;

namespace Cali_4
{ 
[CreateAssetMenu(fileName = "CompRef_SO", menuName = "Scriptable Objects/CompRef_SO")]
    public class CompRef_SO : ScriptableObject
    {
        [Header("Cali Script Components")]
        [SerializeField] static C4_HostBrain InBossBrain;
        [SerializeField] static C4_StateMachine InBossStateMachine;
        [SerializeField] static C4_StateDeterminant InBossStateDeterminant;
        [SerializeField] static C4_UIManager InBossUIManager;
        [SerializeField] static C4_AnimManager InBossAnimManager;
        
        [Header("Brolive Script Components")]
        [SerializeField] static Actor InBossActor;
        [SerializeField] static Damageable InBossDamageable;
        [SerializeField] static Damager InBossDamager;
        [SerializeField] static Navigator InBossNavigator;
        [SerializeField] static NavMeshAgent InBossNavMeshAgent;
        [SerializeField] static Sensor InBossMeleeSensor;

        [Header("Misc. Comp Refs")]
        [SerializeField] static Rigidbody InBossNavRigidbody;
        [SerializeField] static Collider InBossDamagerCollider;
        [SerializeField] static Collider InBossMeleeSensorCollider;

        [Header("GameObject Refs")]
        [SerializeField] static GameObject PlayerObj;
        [SerializeField] static GameObject SpawnerParentObj;
        [SerializeField] static GameObject StateParentObj;
        [SerializeField] static GameObject DamageParentObj;
    }
}
