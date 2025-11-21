using UnityEngine;
using UnityEngine.Events;

namespace Caliodore
{
    namespace States_Phase1
    {
        public class Idle : Phase1
        {
            public override bool IsChosen { get => isChosen; set => isChosen = value; }

            public override void OnStateEnter()
            {
                stateName += "Idle";
                base.OnStateEnter();
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
            }
        } 
    }
}
