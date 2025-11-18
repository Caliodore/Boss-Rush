using UnityEngine;
using Caliodore;
using Caliodore.States_Phase1;
using UnityEngine.Events;

namespace Caliodore
{
    /// <summary>
    /// Brain scripts for the individual enemies that make up the entirety of the first phase. <br/>
    /// Communicates with Overseer through events and public methods.
    /// </summary>
    public class P1_Clergy : BossBrain
    {
        [SerializeField] public EnemyFramework_SO clergySO;
        [SerializeField] public EnemyFramework_SO chosenSO;

        public bool isChosen;

        private EnemyFramework_SO currentSO;
        public override EnemyFramework_SO AttachedSO { get => currentSO; set => currentSO = value; }

        [Header("Events")]
        public UnityEvent OnBeingChosen;

        private void Awake()
        {
            SetRefs(false);
            isChosen = false;
            clergySO = Instantiate(clergySO);
            chosenSO = Instantiate(chosenSO);
            if(OnBeingChosen == null)
                OnBeingChosen = new UnityEvent();
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
