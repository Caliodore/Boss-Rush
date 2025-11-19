using Caliodore.States_Phase1;
using Caliodore.States_Phase2;
using Caliodore.States_Phase3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caliodore
{
    /// <summary>
    /// General manager for handling enemy spawns throughout all phases.
    /// </summary>
    public class SpawnManager_Cali : MonoBehaviour
    {
        [Header("Collections")]
        public List<GameObject> currentlyActiveEnemies = new List<GameObject>();     //For storing the singular chosen and all the regular clergy.
        public Queue<GameObject> dyingEnemies = new Queue<GameObject>();                                        //Queue that provides a buffer for the player.
        public Queue<GameObject> inactiveEnemyQueue = new Queue<GameObject>();                                  //Enemies that are currently waiting to be called to the arena.
        public List<GameObject> startSpawnLocations = new List<GameObject>();                                   //GameObjects of invisible spawn points IN the arena.
        public List<GameObject> upperSpawnPoints = new List<GameObject>();                                      //GameObjects of invis spawn points ABOVE arena for runtime spawns.

        [Header("ObjRefs")]
        [SerializeField] P1_Overseer bossBrainP1;
        [SerializeField] GameObject clergyPrefab;

        [Header("Properties")]
        private int enemiesActive;
        private int queuedEnemies;
        private int maxAmountOfEnemies;
        private int maxActiveEnemies;

        public int QueuedEnemies {  get { return queuedEnemies; } }
        public int EnemiesActive {  get { return enemiesActive; } }
        public int MaxTotalEnemies {  get { return maxAmountOfEnemies; } }
        public int MaxActiveEnemies {  get { return maxActiveEnemies; } }

        private void Awake()
        {
            bossBrainP1 = GetComponent<P1_Overseer>();
        }

        private void Start()
        {
            GenerateCollections();
        }

        private void GenerateCollections()
        { 
            GenerateEnemyPool();
            GenerateStartingLocations();
            GenerateSpawnPoints();
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
                    currentlyActiveEnemies.Add(thisEnemy);

                    if(i == chosenIndex)
                    {
                        enemyScript.OnBeingChosen.Invoke();
                        bossBrainP1.currentChosen = thisEnemy;
                    }
                }
                else if(i > maxActiveEnemies)
                { 
                    //Instantiate in the upper pews and add to queue. Can either have them out of view from the arena, or actually in the pews.
                    int randomSpawnIndex = UnityEngine.Random.Range(0, upperSpawnPoints.Count - 1);
                    Vector3 currentUpperSpawn = upperSpawnPoints[i].transform.position;
                    GameObject thisEnemy = Instantiate(clergyPrefab, currentUpperSpawn, Quaternion.identity);
                    P1_Clergy enemyScript = thisEnemy.GetComponent<P1_Clergy>();
                    inactiveEnemyQueue.Enqueue(thisEnemy);
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

    }
}