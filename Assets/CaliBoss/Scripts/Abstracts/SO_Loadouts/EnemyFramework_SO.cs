using UnityEngine;

namespace Caliodore
{
    [CreateAssetMenu(fileName = "EnemyFramework_SO", menuName = "Scriptable Objects/EnemyFramework_SO")]
    public abstract class EnemyFramework_SO : ScriptableObject
    {
        [Header("Health Vars")]
        [SerializeField] public float MaxHealth;

        [Header("Movement Vars")]
        [SerializeField] public float MoveSpeed;
        [SerializeField] public float TurnSpeed;
        [SerializeField] public float RecoveryTime;

        [Header("Combat Vars")]
        [SerializeField] public float DamageDealt;
        [SerializeField] public float MeleeCooldown;
        [SerializeField] public float RangedCooldown;

        [Header("VFX Vars")]
        [SerializeField] public Color EmissionColor;

        public EnemyFramework_SO() { }
    }

}