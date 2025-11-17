using UnityEngine;
using Caliodore;
using Caliodore.States_Phase1;
using UnityEditor;

namespace Caliodore
{
    public class P1_Chosen : BossBrain
    {
        public bool isChosen;

        [SerializeField] public EnemyFramework_SO inputSO;
        public override EnemyFramework_SO AttachedSO { get => inputSO; set => inputSO = value; }
        private void Awake()
        {
            SetRefs(false);
        }
    }
}
