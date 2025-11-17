using UnityEngine;
using Caliodore;
using Caliodore.States_Phase1;

namespace Caliodore
{
    public class P1_Clergy : BossBrain
    {
        [SerializeField] public EnemyFramework_SO clergySO;
        [SerializeField] public EnemyFramework_SO chosenSO;

        public bool isChosen;

        private EnemyFramework_SO currentSO;
        public override EnemyFramework_SO AttachedSO { get => currentSO; set => currentSO = value; }

        private void Awake()
        {
            SetRefs(false);
            isChosen = false;
            clergySO = Instantiate(clergySO);
            chosenSO = Instantiate(chosenSO);
        }

        private void Start()
        {
            CheckSO();
        }

        private void CheckSO()
        { 
            if(isChosen)
            { 
                currentSO = chosenSO;    
            }
            else
            { 
                currentSO = clergySO;    
            }
        }

        //private void OnDestroy()
        //{
        //    ScriptableObject.Destroy(inputSO);
        //}
    }
}
