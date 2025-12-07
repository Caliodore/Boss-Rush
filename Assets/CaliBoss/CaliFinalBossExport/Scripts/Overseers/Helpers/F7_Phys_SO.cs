using UnityEngine;

namespace Cali7
{ 
    [CreateAssetMenu(fileName = "F7_Phys_SO", menuName = "Scriptable Objects/F7_Phys_SO")]
    public class F7_Phys_SO : ScriptableObject
    {
        [Header("Default Values")]
        [Header("Floats")]
        [SerializeField] public float moveSpeed = 3f;
        [SerializeField] public float meleeRange = 4.5f;
        [SerializeField] public float comboDecayTime = 8f;
        [SerializeField] public float punishDecayTime = 10f;
        [SerializeField] public float brokenBarrierRecoveryTime = 16f;
        [SerializeField] public float reelingBackRecoveryTime = 10f;
        [SerializeField] public float enragedExitRecoveryTime = 14f;

        [Header("Ints")]
        [SerializeField] public int currentPhase;
        [SerializeField] public int bossDamage;
        [SerializeField] public int bossKnockback;
        [SerializeField] public int midCombo = 3;
        [SerializeField] public int maxCombo = 5;
        [SerializeField] public int hitsUntilPunish = 5;
        [SerializeField] public int maxHealth = 20;

        [Header("Alternate Values")]
        [Header("Floats")]
        [SerializeField] public float enragedMoveMod = 1.5f;

        [Header("Ints")]
        [SerializeField] public int enragedDmgMod = 2;
        [SerializeField] public int enragedKnockMod = 2;
        //[SerializeField] public 

        [Header("Misc")]
        public Damage bossDamageDealt { get { return CalcDamage(); } }

        private Damage CalcDamage() {
            int knockMod = 1;
            int dmgMod = 1;
            Damage dmgOut = new Damage();

            if (F7_Central.Instance.isEnraged) { 
                dmgMod = enragedDmgMod;
                knockMod = enragedKnockMod;
            }

            dmgOut.amount = bossDamage * dmgMod;
            dmgOut.direction = F7_RefManager.BGOJ.transform.forward;
            dmgOut.knockbackForce = bossKnockback * knockMod;

            return dmgOut;
        }
    }
}
