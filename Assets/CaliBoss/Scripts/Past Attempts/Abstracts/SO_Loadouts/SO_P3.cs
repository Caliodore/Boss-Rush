using UnityEngine;

namespace Caliodore
{
    [CreateAssetMenu(fileName = "SO_P3", menuName = "Scriptable Objects/SO_P3")]
    public class SO_P3 : EnemyFramework_SO
    {
        [Header("Phase 3 Specific Vars")]
        //Floats
        [SerializeField] public float CatchUpTimer;
        [SerializeField] public float EnragedMovementMod;
        [SerializeField] public float EnragedAnimMod;
        [SerializeField] public float EnragedDamageMod;
        [SerializeField] public float LeapDistanceMax;
        [SerializeField] public float LeapTravelSpeed;
        //Ints
        [SerializeField] public int BloodShardAmount;

        [Header("Phase 2+3 Vars")]
        //Floats
        [SerializeField] public float ChargeBuildUpTimer;
        [SerializeField] public float ChargeAttackRadius;
        [SerializeField] public float ChargeAttackAngle;
        [SerializeField] public float AoERadius;
        [SerializeField] public float ComboDecayTimer;
        [SerializeField] public float PummelPumishDecayTimer;
        [SerializeField] public float BloodBarrierDecayTimer;
        //Ints
        [SerializeField] public int PillarSpawnAmount;
        
        public SO_P3() : base() { }
    }
}