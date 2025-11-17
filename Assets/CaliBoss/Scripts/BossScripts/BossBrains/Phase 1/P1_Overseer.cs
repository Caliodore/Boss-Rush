using UnityEngine;
using Caliodore;
using Caliodore.States_Phase1;

namespace Caliodore
{
    public class P1_Overseer : BossBrain
    {
        private void Awake()
        {
            SetRefs(true);
        }
    }
}
