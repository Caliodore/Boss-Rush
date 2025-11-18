using UnityEngine;
using Caliodore;
using Caliodore.States_Phase1;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Caliodore
{
    /// <summary>
    /// Empty object that communicates with spawner and individual enemies to keep track of progress. <br/>
    /// Also communicates with UI and holds data on boss phase progress.
    /// </summary>
    public class P1_Overseer : BossBrain
    {
        [Header("State Refs")]
        public bool bossAlerted;            //Bool that is associated with showing the health bar and whether or not enemies are in the aggro state.
        public GameObject currentChosen;    //Reference for other scripts to know which enemy is Chosen at a glance.

        [Header("Spawning Vars")]
        public GameObject clergyPrefab;
        public Queue<GameObject> queuedClergy = new Queue<GameObject>();
        public List<GameObject> enemiesInArena = new List<GameObject>();
        public List<GameObject> startSpawnLocations = new List<GameObject>();
        public List<GameObject> upperSpawnPoints = new List<GameObject>();
        public int maxAmountOfEnemies;
        public int maxActiveEnemies;

        [Header("Events and Actions")]
        public UnityEvent ChosenDiedEvent;
        public UnityEvent ClergyDiedEvent;

        [Header("Boss Progression")]
        public float maxTotalBossHealth;
        public float currentTotalBossHealth;
        private float chosenHealthTotalImpact;
        private float clergyHealthTotalImpact;

        private void Awake()
        {
            SetRefs(true);
            CurrentPhase = 1;
            GenerateCollections();
            GenerateEvents();
        }

        /// <summary>
        /// To update the public variable currentChosen.
        /// </summary>
        /// <returns>The Chosen that is within enemiesInArena.</returns>
        private GameObject FindChosen()
        { 
            GameObject chosenObjRef = enemiesInArena.Find(x => x.GetComponent<P1_Chosen>().isChosen == true);
            return chosenObjRef;
        }

        /// <summary>
        /// To be called when enemies are added or removed from the arena, and when a new Chosen is decided. <br/>
        /// Adds and removes listeners from ChosenDied and ClergyDied as needed. <br/>
        /// Intended to be called AFTER all other events/updates when something dies.
        /// </summary>
        private void UpdateEvents(GameObject targetObject, bool addOrRemove)
        {
            bool objInArena = enemiesInArena.Contains(targetObject);
            P1_Clergy targetScript = targetObject.GetComponent<P1_Clergy>();
            bool isTargetChosen = targetScript.isChosen;

            if(objInArena)
            { 
                if(addOrRemove)
                { 
                    print("Can't add an object that is already active in the arena.");
                    return;
                }
                else if(!addOrRemove)
                {
                    if(isTargetChosen)
                        ChosenDiedEvent.AddListener(() => OnChosenDeath(targetObject));
                    else
                        ClergyDiedEvent.AddListener(() => OnClergyDeath(targetObject));
                }
            }
            else if(!objInArena)
            { 
                if(!addOrRemove)
                { 
                    print("Can't remove an object that isn't active in the arena.");
                    return;
                }
                else if(addOrRemove)
                { 
                    if(isTargetChosen)
                        ChosenDiedEvent.RemoveListener(() => OnChosenDeath(targetObject));
                    else
                        ClergyDiedEvent.RemoveListener(() => OnClergyDeath(targetObject));
                }
            }
        }

        private void GenerateEvents() 
        { 
            if(ChosenDiedEvent == null)
                ChosenDiedEvent = new UnityEvent();
            if(ClergyDiedEvent == null)
                ClergyDiedEvent = new UnityEvent();

            foreach(GameObject currentEnemy in enemiesInArena)
            { 
                UpdateEvents(currentEnemy, true);
            }
        }

        /// <summary>
        /// To only be called at the beginning of the scene to first instantiate all the enemies. <br/>
        /// Also separates the queued enemy pool and the ones active in the arena at start.
        /// --This will require a handful of spawn points within the arena for the initial spawn.
        /// </summary>
        private void GenerateEnemyPool()
        {
            int chosenIndex = UnityEngine.Random.Range(0, maxActiveEnemies);
            for(int i = 0; i < maxAmountOfEnemies; i++)     //Runs until the total amount of enemies are generated in both collections.
            { 
                if(i <= maxActiveEnemies)                    //Runs until filling the spots in the arena.
                { 
                    //Instantiate enemies and add them to enemiesInArena.
                    Vector3 currentStartSpawn = startSpawnLocations[i].transform.position;
                    GameObject thisEnemy = Instantiate(clergyPrefab, currentStartSpawn, Quaternion.identity);
                    P1_Clergy enemyScript = thisEnemy.GetComponent<P1_Clergy>();
                    enemiesInArena.Add(thisEnemy);

                    if(i == chosenIndex)
                    {
                        enemyScript.OnBeingChosen.Invoke();
                        currentChosen = thisEnemy;
                    }
                }
                else if(i > maxActiveEnemies)
                { 
                    //Instantiate in the upper pews and add to queue. Can either have them out of view from the arena, or actually in the pews.
                    int randomSpawnIndex = UnityEngine.Random.Range(0, upperSpawnPoints.Count - 1);
                    Vector3 currentUpperSpawn = upperSpawnPoints[i].transform.position;
                    GameObject thisEnemy = Instantiate(clergyPrefab, currentUpperSpawn, Quaternion.identity);
                    P1_Clergy enemyScript = thisEnemy.GetComponent<P1_Clergy>();
                    queuedClergy.Enqueue(thisEnemy);
                }
            }
        }

        /// <summary>
        /// Gathers all GameObjects with the starting spawn target scripts.
        /// </summary>
        private void GenerateStartingLocations()
        {
            var scriptArray = FindObjectsByType<StartingLocation>(FindObjectsSortMode.None);
            foreach(StartingLocation currentRef in scriptArray)
                startSpawnLocations.Add(currentRef.gameObject);
        }

        /// <summary>
        /// Gathers all GameObjects that have the recurring spawn points (upper pew spawns) target scripts.
        /// </summary>
        private void GenerateSpawnPoints()
        { 
            var scriptArray = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
            foreach(SpawnPoint currentRef in scriptArray)
                upperSpawnPoints.Add(currentRef.gameObject);
        }

        /// <summary>
        /// Called at start to generate collections for: StartingLocations, SpawnPoints, EnemiesInArena, QueuedEnemies
        /// </summary>
        private void GenerateCollections()
        { 
            GenerateEnemyPool();
            GenerateStartingLocations();
            GenerateSpawnPoints();
        }

        /// <summary>
        /// To be invoked by ChosenDied. Handles choosing a new Chosen, removing the old listener, and requeuing the clergy.
        /// </summary>
        /// <param name="deadChosen">GameObj reference to the Chosen that is dying</param>
        private void OnChosenDeath(GameObject deadChosen) 
        {
            UpdateBossHealth(chosenHealthTotalImpact);
            enemiesInArena.Remove(deadChosen);
            ChooseNewChosen();
        }

        /// <summary>
        /// To be invoked by ClergyDied. Handles requeuing this member and signaling a new one to spawn.
        /// </summary>
        private void OnClergyDeath(GameObject deadClergy) 
        { 
            UpdateBossHealth(clergyHealthTotalImpact);
            enemiesInArena.Remove(deadClergy);
        }

        /// <summary>
        /// Method to be called when a Chosen dies, or during the beginning generation. <br/>
        /// Selects from enemiesInArena randomly and signifies them as Chosen.
        /// </summary>
        private void ChooseNewChosen() 
        { 
            int randomIndex = UnityEngine.Random.Range(0, enemiesInArena.Count);
            GameObject newChosen = enemiesInArena[randomIndex];
            //Call whatever method is attached to the corresponding gameObj to signify them as chosen.
            ClergyToChosenEvents(newChosen);
        }

        /// <summary>
        /// To be called by ChooseNewChosen() to update the events to the change in Chosen.
        /// </summary>
        /// <param name="clergyChanging">GameObjRef to the newly selected Chosen.</param>
        private void ClergyToChosenEvents(GameObject clergyChanging)
        { 
            ClergyDiedEvent.RemoveListener(() => OnClergyDeath(clergyChanging));
            ChosenDiedEvent.AddListener(() => OnChosenDeath(clergyChanging));
        }

        /// <summary>
        /// Will interact with UI script to feed it the amount it should affect the health bar by when an enemy takes damage.
        /// </summary>
        /// <param name="percentDealt">Decimal form of the percentage of the health bar that should be deducted.</param>
        private void UpdateBossHealth(float percentDealt) 
        { 
                
        }

    }
}
