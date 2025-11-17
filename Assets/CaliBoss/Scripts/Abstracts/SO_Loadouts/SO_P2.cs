using UnityEngine;

namespace Caliodore
{
    [CreateAssetMenu(fileName = "SO_P2", menuName = "Scriptable Objects/SO_P2")]
    public class SO_P2 : EnemyFramework_SO
    {
        [Header("Phase 2 Specific Vars")]
        [SerializeField] public float ChargeAttackArenaPercentage;

        [Header("Phase 2+3 Vars")]
        //Bools
        [SerializeField] public bool IsComboing;
        [SerializeField] public bool IsStationary;
        [SerializeField] public bool IsDefending;
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
        
        public SO_P2() : base() { }
    }
}