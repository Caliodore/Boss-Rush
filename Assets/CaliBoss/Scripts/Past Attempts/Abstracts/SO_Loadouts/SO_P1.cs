using UnityEngine;

namespace Caliodore
{
    [CreateAssetMenu(fileName = "SO_P1", menuName = "Scriptable Objects/SO_P1")]
    public class SO_P1 : EnemyFramework_SO
    {
        [Header("Phase 1 Specific Vars")]
        [SerializeField] public float BuffMod;
        [SerializeField] public float BodyScale;
        [SerializeField] public float FadeAwaySpeed;
        [SerializeField] public float TotalHealthImpact;

        public SO_P1() : base() { }
    }
}