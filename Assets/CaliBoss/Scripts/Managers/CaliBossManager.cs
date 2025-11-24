using Unity.VisualScripting;
using UnityEngine;

namespace Caliodore
{
    public class CaliBossManager : MonoBehaviour
    {
        /* 
         * Hostellus, the Eternal Martyr
         * and The Sacerdotes Cleridatus
    --------------------------------------------------------------------------------------------------------------------------------------------------------
         * Necessities for Assignment:
         * } Makes use of a state machine or behaviour tree
         *  - DONT USE ENUM-BASED STATE MACHINE
         *  - Can use: inheritance, component, or animator for example
         * 
         * } 3 Phases (Initial, Mid-Battle, Enraged)
         *  - Simple first phase
         *  - Adds more attacks, 2nd stage
         *  - Ultimate attack/faster moveset
         * 
         * } Attack variety
         *  - At least 1 melee, 1 ranged
         *  - AoE/Multi-stage to make the player move
         *  - Special mechanic/environmental interaction/ultimate attack
         *  - Defensive manuever/punishment
         * 
         * } Telegraphed movements
         *  - Make sure there is a visual component to communicate to the player when and where an attack is happening
         * 
    --------------------------------------------------------------------------------------------------------------------------------------------------------
         * Script Purposes and Intended Functionalities:
         *  } Keep track of boss health in relation to phases.
         *  } Managing swapping between phases.
         *  } Communicating between the scripts as a whole*
         *      - Some lower scripts will directly connect and interact, this is moreso a central repository/exchange to help monitor and relay info.
         *  } Centralizing references for other scripts.
         *  } Communication with other managers.
         *      - Objects will still most likely interact directly, but for most larger events and changes, this script will be the means of propagation.
    --------------------------------------------------------------------------------------------------------------------------------------------------------
         *
         */

        //As a central reference point, this script can have a host of static variables for others to reference to lessen confusion.
        [Header("Static Refs")]
        //Manager Refs
        public static CaliBossManager MainManager;
        public static SpawnManager_Cali SpawnManager;
        public static SoundEffectsManager SoundFXManager;
        public static GameManager GameManager;
        public static ClergyOverseer ClergyOverseer;

        //Physical Refs
        public static GameObject PlayerObj;
        public static GameObject ClergyPrefab;
        public static GameObject HostellusP2Prefab;
        public static GameObject HostellusP3Prefab;
        public static Transform PlayerTransform;

        //Logic Refs
        public static PlayerLogic PlayerScript;

        //Scriptable Objects
        public static SO_P1 ClergySO;
        public static SO_P1 ChosenSO;
        public static SO_P2 Hostellus_P2_SO;
        public static SO_P3 Hostellus_P3_SO;

//  --------------------------------------------------------------------------------------------------------------------------------------------------------

        [Header("Properties to Reference")]
        //Floats
        [SerializeField] public float CurrentHealth { get { return currentHealth; } }
        [SerializeField] public float TotalHealth { get { return totalHealth; } }

        //Ints
        [SerializeField] public int CurrentPhase { get { return currentPhase; } }
        [SerializeField] public int TotalEnemies { get { return totalEnemies; } }
        [SerializeField] public int MaxArenaEnemies { get { return maxArenaEnemies; } }

        //Bools
        [SerializeField] public bool IsTransitioning { get { return isTransitioning; } }
        [SerializeField] public bool IsSpawningAdds { get { return isSpawningAdds; } }

//  ---------------------------------------------------------------
        //Backing Fields
        //Floats
        private float currentHealth;
        private float totalHealth;
        //Ints
        private int currentPhase;
        private int totalEnemies;
        private int maxArenaEnemies;
        //Bools
        private bool isTransitioning;
        private bool isSpawningAdds;

//  --------------------------------------------------------------------------------------------------------------------------------------------------------
        //Begin logic

        private void Awake()
        {
            CheckStaticReferences();
            CheckGameObjRefs();
            SetProperties();
        }

        private void CheckStaticReferences()
        { 
            NullCheck(MainManager);
            NullCheck(SpawnManager);
            NullCheck(SoundFXManager);
            NullCheck(GameManager);
            NullCheck(ClergyOverseer);
            //NullCheck(UIManager);
            NullCheck(PlayerScript);
        }

        private void NullCheck<T>(T inputObj) where T : MonoBehaviour
        { 
            if(inputObj == null)
            { 
                inputObj = FindAnyObjectByType<T>();
                if(inputObj != null)
                    return;
                else
                {
                    print("Didn't find any instances of the associated script, adding component to CaliBossManager's GameObject.");
                    inputObj = gameObject.AddComponent<T>();
                }
            }
        }

        private void CheckGameObjRefs()
        { 
            if(PlayerObj == null)
                PlayerObj = FindAnyObjectByType<PlayerLogic>().gameObject;
            if(PlayerTransform == null)
                PlayerTransform = PlayerObj.transform;
            if(ClergyPrefab == null)
                ClergyPrefab = FindAnyObjectByType<ClergyBrain>().gameObject;
            /*if(HostellusP2Prefab == null)
                HostellusP2Prefab = FindAnyObjectByType<HostP2>().gameObject;
            if(HostellusP3Prefab == null)
                HostellusP3Prefab = FindAnyObjectByType<HostP3>().gameObject;*/
        }

        private void SetProperties()
        {
            totalHealth = 100;
            currentHealth = 0;
            currentPhase = 0;
            totalEnemies = SpawnManager.maxAmountOfEnemies;
            maxArenaEnemies = SpawnManager.maxActiveEnemies;
            isTransitioning = false;
            isSpawningAdds = false;
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            
        }

        /// <summary>
        /// Public method other objects can call to make sure a phase change isn't missed.<br/>
        /// Accounts for cutscenes using IsTransitioning as reference.
        /// </summary>
        /// <returns>Returns true after calling PhaseChange, otherwise returns false.</returns>
        public bool ChangePhaseCheck()
        {
            print("ChangePhaseCheck called.");
            if(!IsTransitioning)
            {
                print("ChangePhaseCheck read IsTransitioning as false.");
                if(currentHealth <= 0)
                {
                    print("ChangePhaseCheck calls PhaseChange since currentHealth <= 0.");
                    PhaseChange();
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// Mainly a signpost method to call other methods after determining what phase it is.
        /// </summary>
        private void PhaseChange()
        { 
            switch(currentPhase)
            { 
                case(0):
                    currentPhase++;
                    InitializeP1();
                    break;

                case(1):
                    currentPhase++;
                    InitializeP2();
                    break;

                case(2):
                    currentPhase++;
                    InitializeP3();
                    break;

                case(3):
                    currentPhase = 3;
                    P3Ending();
                    break;

                case(4):
                default:
                    print($"PhaseChange reads CurrentPhase as {currentPhase} which is out of the range of 0-3 (inclusive).");
                    break;
            }

        }
        private void InitializeP1()
        { 
            print("Phase 1 Starting.");
            /*
             *  Triggered by: Player entering arena.
             *  Needs to:
             *      Make sure enemy collections are generated. (Should already be done, but can run a check to make sure the phase variables are set proper.
             *      Prepare UI Manager but don't display health bar yet.
             *      
             */
        }

        private void InitializeP2()
        { 
            print("Phase 1 Ending triggered.");
            /*
             * 
             */
        }

        private void InitializeP3()
        { 
            print("Phase 2 Ending triggered.");
            /*
             * 
             */
        }

        private void P3Ending()
        { 
            print("Phase 3 Ending triggered.");
            /*
             * 
             */
        }

        /// <summary>
        /// Called at the start of phases to make sure enemy collections are filled and assigned properly.
        /// </summary>
        private void ArenaEntryEnemyCheck()
        { 
            
        }
    }
}
