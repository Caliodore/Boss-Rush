using UnityEngine;

namespace Cali_4
{ 
    public class LeapAt : Moving
    {
        public LeapAt(string nameIn) : base("LeapAt") { stateName += nameIn; }

        private void Awake()
        {
            //print(base.ToString());
            base.OnAwake();
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}
