using UnityEngine;
using Caliodore;
using Caliodore.States_Phase1;
using UnityEngine.Events;
using System.Collections;

namespace Caliodore
{
    /// <summary>
    /// Brain scripts for the individual enemies that make up the entirety of the first phase. <br/>
    /// Communicates with Overseer through events and public methods.
    /// </summary>
    public class ClergyBrain : BossBrain
    {
        [Header("Obj Refs")]
        [SerializeField] public EnemyFramework_SO clergySO;
        [SerializeField] public EnemyFramework_SO chosenSO;

        [Header("Physical Vars")]
        public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
        private float currentHealth;
        private Rigidbody attachedRigidbody;

        public bool isChosen = false;
        public bool isActive = false;

        private EnemyFramework_SO currentSO;
        public override EnemyFramework_SO AttachedSO { get => currentSO; set => currentSO = value; }

        [Header("Events")]
        public UnityEvent OnBeingChosen;
        public UnityEvent OnThisDeath;
        public UnityEvent OnThisSpawn;

        private void Awake()
        {
            attachedRigidbody = GetComponent<Rigidbody>();
            SetRefs(false);

            if(OnBeingChosen == null)
                OnBeingChosen = new UnityEvent();

            OnBeingChosen.AddListener(BecomeChosen);

            if(OnThisDeath == null)
                OnThisDeath = new UnityEvent();
            if(OnThisSpawn == null)
                OnThisSpawn = new UnityEvent();

            CheckSO();
        }

        private void SetDeathEvents()
        { 
            OnThisDeath.AddListener(OnDeath);
            OnThisDeath.AddListener(() => ClergyOverseer.Instance.OnClergyDeath(gameObject));
        }

        private void SetSpawnEvents()
        { 
            OnThisSpawn.AddListener(OnSpawn);
            
        }

        /// <summary>
        /// To be called after instantiating an enemy at the beginning of the scene to make sure scenes have correct listeners. <br/>
        /// Resets all events, then checks and adds listeners accordingly.
        /// </summary>
        public void EventUpdater()
        {
            OnBeingChosen.RemoveAllListeners();
            OnThisDeath.RemoveAllListeners();
            OnThisSpawn.RemoveAllListeners();

            if(isChosen) 
                OnBeingChosen.AddListener(BecomeChosen);

            if(isActive)
            { 
                SetDeathEvents();
            }
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

            currentHealth = currentSO.MaxHealth;
        }

        private void BecomeChosen()
        { 
            isChosen = true;
            OnBeingChosen.RemoveListener(BecomeChosen);
            CheckSO();
        }

        private void OnSpawn()
        {
            //Enable collision detection and gravity and other physics components.
            TogglePhysics(true);
        }

        private void OnDeath()
        { 
            //Begin dying animation, remove from active arena enemies, and add to dying queue.
            TogglePhysics(false);
        }

        /// <summary>
        /// Turns off collisions and gravity so the model can slowly descend into the ground to show it dying.
        /// </summary>
        /// <param name="onOrOff">Turn coll and grav On (T) or Off (F)?</param>
        private void TogglePhysics(bool onOrOff)
        { 
            attachedRigidbody.useGravity = onOrOff;
            attachedRigidbody.detectCollisions = onOrOff;
        }

        public void BuffedByChosen()
        { 
            
        }

        IEnumerator InBuffRangeCheck()
        {
            yield return null;
        }
    }
}
