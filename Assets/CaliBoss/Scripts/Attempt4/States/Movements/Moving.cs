using UnityEngine;

namespace Cali_4
{ 
    public class Moving : StC4
    {
        public Moving(string nameIn) : base("Moving") { stateName += nameIn; }

        public static Moving StateInstance;

        /*
         * State intended for handling general movement, both targeted like chasing the player, and also general wandering, and even aggressive movement like leaps/dashes.
         * State needs:
         *      - Definite: TargetPosition, MoveSpeed, MovementType, StopCondition
         *      - Potential: NextIntendedState, CalculatedPath
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
