using UnityEngine;

namespace Cali6
{ 
    public class A6_Recovering : A6_StateBase
    {
        public bool printDebugLogs = true;
        public A6_Recovering() : base("Recovering") { }

        public static A6_Recovering RecoveringInstance;

        public override void OnDamagedDuringState()
        {
            base.OnDamagedDuringState();
        }
        
    }
}
