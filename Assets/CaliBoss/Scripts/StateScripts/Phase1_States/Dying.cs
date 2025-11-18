using UnityEngine;
using UnityEngine.Events;

namespace Caliodore
{
    namespace States_Phase1
    {
        public class Dying : Phase1
        {
            public override bool IsAlerted { get => isAlerted; set => isAlerted = value; }
            public override bool IsChosen { get => isChosen; set => isChosen = value; }

            public override void OnStateEnter()
            {
                stateName += "Dying";
            }
        } 
    }
}
