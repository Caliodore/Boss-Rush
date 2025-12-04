using UnityEngine;

namespace Cali6
{ 
    public class A6_Defending : A6_StateBase
    {
        public bool printDebugLogs = true;
        public A6_Defending() : base("Defending") { }

        public static A6_Defending DefendingInstance;

        public override void OnDamagedDuringState()
        {
            base.OnDamagedDuringState();
        }
        
    }
}
