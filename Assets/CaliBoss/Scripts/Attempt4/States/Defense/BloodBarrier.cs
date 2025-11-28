using UnityEngine;

namespace Cali_4
{ 
    public class BloodBarrier : Defending
    {
        public BloodBarrier() : base("BloodBarrier") { }

        private void Awake()
        {
            base.OnAwake();
        }
    }
}
