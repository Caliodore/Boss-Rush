using UnityEngine;
using UnityEngine.Events;

namespace Caliodore
{
    namespace States_Phase1
    {
        public class Dying : Phase1
        {
            public Dying() : base("Dying") { }

            public override void OnStateEnter()
            {
                base.OnStateEnter();
                clergyBrain.OnThisDeath.Invoke();
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
            }
        } 
    }
}
