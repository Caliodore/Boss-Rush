using UnityEngine;

namespace Cali6
{ 
    public class A6_Pursuing : A6_StateBase
    {
        public A6_Pursuing() : base("Pursuing") { }

        public static A6_Pursuing PursuingInstance;

        public override void OnDamagedDuringState()
        {
            base.OnDamagedDuringState();
        }
        
    }
}
