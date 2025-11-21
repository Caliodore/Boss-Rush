using UnityEngine;
using UnityEngine.Events;

namespace Caliodore
{
    namespace States_Phase1
    {
        public class Aggro : Phase1
        {
            public override bool IsChosen { get => isChosen; set => isChosen = value; }

            public override void OnStateEnter()
            {
                stateName += "Aggro";
            }
        } 
    }
}
