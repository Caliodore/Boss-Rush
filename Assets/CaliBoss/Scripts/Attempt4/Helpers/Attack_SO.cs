using UnityEngine;

namespace Cali_4
{ 
    [CreateAssetMenu(fileName = "Attack_SO", menuName = "Scriptable Objects/Attack_SO")]
    public class Attack_SO : ScriptableObject
    {
        [SerializeField] string attackType;
        [SerializeField] int damageAmount;
        [SerializeField] float knockbackValue;
        [SerializeField] Damage attackDamageRef;

        public Attack_SO() 
        { 
            attackDamageRef.amount = damageAmount;
            attackDamageRef.knockbackForce = knockbackValue;    
        }
    }
}
