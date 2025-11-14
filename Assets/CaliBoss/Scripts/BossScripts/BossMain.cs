using UnityEngine;
using Caliodore.States_Phase1;
using Caliodore.States_Phase2;
using Caliodore.States_Phase3;

namespace Caliodore
{
    public class BossMain : MonoBehaviour
    {
        BossStateMachine attachedSM;

        private void Start()
        {
            attachedSM = new BossStateMachine(this);
            attachedSM.ChangeState(new States_Phase1.Entry());
        }
    }
}
