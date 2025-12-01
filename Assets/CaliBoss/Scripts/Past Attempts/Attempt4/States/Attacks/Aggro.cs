using UnityEngine;

namespace Cali_4
{ 
    public abstract class Aggro : StC4
    {
        public Aggro(string nameIn) : base("Aggro") { stateName += nameIn; }

        public string attackAnimName;

        public static Aggro StateInstance;
        /*
         * State intended for the majority of attacks, this will be a wide-reaching grouping, but will normally only be active for a short period of time.
         * Intended to help encapsulate the basis of any attack we might try to do, from ranged to melee to AoE.
         */
        private void Awake()
        {
            print(base.ToString());
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
