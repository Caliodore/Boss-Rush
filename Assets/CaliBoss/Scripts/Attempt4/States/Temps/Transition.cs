using UnityEngine;

namespace Cali_4
{ 
    public class Transition : StC4
    {
        public Transition(string nameIn) : base("Transition") { stateName += nameIn; }

        public static Transition StateInstance;

        protected float transitionDuration;

        /*
         * State intended for when: an enemy spawns in, phase transitions, or player enters arena for the first time.
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
            if(currentStateDuration >= transitionDuration)
                StopThisState?.Invoke();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
        }

        /// <summary>
        /// When swapping to transition, the state will run until the specific time has elapsed.
        /// </summary>
        /// <param name="transitionTime">A positive float that controls the duration of the state.</param>
        public void SetTransitionDuration(float transitionTime) 
        {
            if(transitionTime < 0)
                transitionTime = 0;
            transitionDuration = transitionTime;
        }
    }
}
