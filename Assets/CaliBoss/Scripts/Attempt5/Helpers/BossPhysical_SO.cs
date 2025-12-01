using UnityEngine;

namespace CaliBoss
{ 
    [CreateAssetMenu(fileName = "BossPhysical_SO", menuName = "Scriptable Objects/BossPhysical_SO")]
    public class BossPhysical_SO : ScriptableObject
    {
        [Header("Body Vars")]
        public float moveSpeed;
        public float turnSpeed;
        public int maxHealth;

        [Header("Damage Vars")]
        public float meleeRange;
        public int damageDealt;
        public int knockbackDealt;

        [Header("Combo/Rebuke Vars")]
        public int maxCombo;
        public int hitsUntilRebuke;
        public int hitsUntilBarrierBreak;
    }
}
