using UnityEngine;

namespace Cali_4
{ 
    public class Defending : StC4
    {
        public Defending(string nameIn) : base("Defending") { stateName += nameIn; }

        public static Defending StateInstance;

        private void Awake()
        {
            OnAwake();
        }

        public override void OnAwake()
        {
            base.OnAwake();
        }

        /*
         * State that is the basis for defensive manouevers and dealing with punishments that are not just direct attacks.
         * Shouldn't be an overly full grouping, whereas Aggro should have lots of options.
         * State needs:
         *      - Definite: RecentHitsTaken, HitThreshold, PunishmentType, PlayerInMelee, DamageTypeTaken
         *      - Potential: TimeTillDrop, DefensiveReaction, 
         */

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
