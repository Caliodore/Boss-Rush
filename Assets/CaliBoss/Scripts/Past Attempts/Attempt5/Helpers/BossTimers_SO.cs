using UnityEngine;
namespace CaliBoss
{ 
    [CreateAssetMenu(fileName = "BossTimers_SO", menuName = "Scriptable Objects/BossTimers_SO")]
    public class BossTimers_SO : ScriptableObject
    {
        [Header("Decay Window Timer Amounts")]
        public float comboDecayTimer;
        public float punishDecayTimer;
        public float bloodBarrierDecayTimer;

        [Header("Cooldown Timer Amounts")]
        public float meleeCooldownTimer;
        public float rangedCooldownTimer;
        public float leapMovementCooldownTimer;
        public float leapAttackCooldownTimer;
        public float enragedCooldownTimer;
        public float bloodBarrierCooldownTimer;
        public float bloodPillarsCooldownTimer;
        public float closeRingCooldownTimer;

        [Header("Active Timer Amounts")]
        public float enragedActiveTimer;
        public float barrierActiveTimer;
        public float pillarsActiveTimer;
        public float closeRingActiveTimer;
    }
}
