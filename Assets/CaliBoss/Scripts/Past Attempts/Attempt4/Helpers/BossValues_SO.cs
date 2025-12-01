using UnityEngine;

namespace Cali_4
{ 
    [CreateAssetMenu(fileName = "BossValues_SO", menuName = "Scriptable Objects/BossValues_SO")]
    public class BossValues_SO : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField] float BaseMoveSpeed;
        [SerializeField] float BaseTurnSpeed;
        [SerializeField] float RageSpeedMod;

        [Header("Health")]
        [SerializeField] int BaseMaxHealth;

        [Header("Combat")]
        [SerializeField] int BaseDamageAmt;
        [SerializeField] int BaseDamageKnockback;
    }
}
