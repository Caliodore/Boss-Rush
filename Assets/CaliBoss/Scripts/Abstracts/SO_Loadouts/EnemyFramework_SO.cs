using UnityEngine;

namespace Caliodore
{
    [CreateAssetMenu(fileName = "EnemyFramework_SO", menuName = "Scriptable Objects/EnemyFramework_SO")]
    public abstract class EnemyFramework_SO : ScriptableObject
    {
        [Header("Health Vars")]
        [SerializeField] public float MaxHealth;
        [SerializeField] public float CurrentHealth;
        public float HealthPercentage { get { return CurrentHealth / MaxHealth; } private set { HealthPercentage = CurrentHealth / MaxHealth; } }

        [Header("Movement Vars")]
        [SerializeField] public float MoveSpeed;
        [SerializeField] public float TurnSpeed;
        [SerializeField] public float RecoveryTime;

        [Header("Combat Vars")]
        [SerializeField] public float DamageDealt;
        [SerializeField] public float MeleeCooldown;
        [SerializeField] public float RangedCooldown;

        //Bools
        [SerializeField] public bool InRangeOfPlayer;
        [SerializeField] public bool IsRecovering;
        [SerializeField] public bool IsTransitioning;
        [SerializeField] public bool IsAttacking;
        [SerializeField] public bool IsTurning;

        [Header("Logic Ref Vars")]
        [SerializeField] public int IntendedPhase;
        [SerializeField] public bool AwareOfPlayer;
        [SerializeField] public bool IsEmitting;
        [SerializeField] public Color EmissionColor;

        public EnemyFramework_SO() { }

        //Concepts
        /*
        [SerializeField] public bool ActiveDamageMult;
        */
    }
}