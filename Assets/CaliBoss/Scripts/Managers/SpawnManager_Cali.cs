using Caliodore.States_Phase1;
using Caliodore.States_Phase2;
using Caliodore.States_Phase3;
using Mono.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

namespace Caliodore
{
    /// <summary>
    /// General manager for handling enemy spawns throughout all phases.
    /// </summary>
    public class SpawnManager_Cali : MonoBehaviour
    {
        [Header("Collections")]
        public List<GameObject> currentlyActiveEnemies = new List<GameObject>();                                //For storing the singular chosen and all the regular clergy.
        public Queue<GameObject> dyingEnemies = new Queue<GameObject>();                                        //Queue that provides a buffer for the player.
        public Queue<GameObject> inactiveEnemyQueue = new Queue<GameObject>();                                  //Enemies that are currently waiting to be called to the arena.
        public List<GameObject> startSpawnLocations = new List<GameObject>();                                   //GameObjects of invisible spawn points IN the arena.
        public List<GameObject> upperSpawnPoints = new List<GameObject>();                                      //GameObjects of invis spawn points ABOVE arena for runtime spawns.

        [Header("ObjRefs")]
        [SerializeField] ClergyOverseer bossBrainP1;
        [SerializeField] GameObject clergyPrefab;
        [SerializeField] QueueParent queueHolder;
        [SerializeField] ActiveClergyParent clergyHolder;

        [Header("Properties")]
        public int enemiesActive;
        public int queuedEnemies;
        public int maxAmountOfEnemies;
        public int maxActiveEnemies;
        public float clergyDyingSpeed = 3f;

        [Header("Collection Checks")]
        public bool currentlyGenerating = false;
        public bool generatedSpawns = false;
        public bool generatedStarts = false;
        public bool generatedEnemyLists = false;
        public float generationElapsedTime = 0f;

        //---------Public Methods---------//

        public bool CheckIfActive(GameObject inputEnemy)
        {
            if(inputEnemy.GetComponent<ClergyBrain>() == null)
            { 
                print("This object is not handled by SpawnManager. Make sure it is at the correct scope and that this object has the P1_Clergy script attached.");
                return false;
            }
            else
            { 
                var checkOut = CheckCollectionsFor(inputEnemy);
                if(checkOut.collectionName != "currentlyActiveEnemies")
                { 
                    print("This object is not in the currently active enemy list.");
                    return false;
                }
                else
                { 
                    return true;    
                }
            }
        }

        public void ClergyQueueAccessor(GameObject dyingClergy)
        {
            if(dyingClergy.GetComponent<ClergyBrain>() == null)
            { 
                print("This object does not have the correct script (P1_Clergy) attached to be tracked by the SpawnManager.");
                return;
            }
            else
            { 
                QueueEnterExit(dyingClergy);
            }
        }

        /*public void CollectionTopOff()
        {
            CheckIfCollectionsFull();
        }*/

        //--------Internal Methods--------//

        private void Awake()
        {
            bossBrainP1 = GetComponent<ClergyOverseer>();
        }

        private void Start()
        {
            GenerateCollections();
        }

        private void QueueEnterExit(GameObject inputObject)
        {
            ClergyBrain inputScript = inputObject.GetComponent<ClergyBrain>();
            var checkOut = CheckCollectionsFor(inputObject);
            if(checkOut.isCollected)
            { 
                switch(checkOut.collectionName)
                { 
                    case("currentlyActiveEnemies"):     //Should happen when a clergy member invokes OnThisDeath.
                        { 
                            currentlyActiveEnemies.Remove(inputObject);
                            dyingEnemies.Enqueue(inputObject);
                        }
                        break;

                    case("dyingEnemies"):               //Should happen when a clergy member finishes dying.
                        { 
                            inactiveEnemyQueue.Enqueue(dyingEnemies.Dequeue());
                        }
                        break;

                    case("inactiveEnemyQueue"):         //Should happen when a queued enemy is called to the arena.
                        { 
                            currentlyActiveEnemies.Add(inactiveEnemyQueue.Dequeue());
                        }
                        break;

                }
            }
        }

        private void GenerateCollections()
        {
            GenerateSpawnPoints();
            GenerateStartingLocations();
            GenerateEnemyPool();
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
                if(i > startSpawnLocations.Count)           //Checks once enough have been generated for the starting locations to select a chosen if it hasn't been done yet.
                { 
                    if(bossBrainP1.currentChosen == null)
                        bossBrainP1.GetComponent<ClergyOverseer>().FindChosen();
                }

                if(i <= maxActiveEnemies)                   //Runs until filling the spots in the arena.
                {
                    //Instantiate enemies and add them to enemiesInArena.
                    Vector3 currentStartSpawn = startSpawnLocations[i].transform.position;
                    GameObject thisEnemy = Instantiate(clergyPrefab, currentStartSpawn, Quaternion.identity, clergyHolder.transform);
                    ClergyBrain enemyScript = thisEnemy.GetComponent<ClergyBrain>();
                    currentlyActiveEnemies.Add(thisEnemy);
                    thisEnemy.SetActive(true);

                    if(i == chosenIndex)
                    {
                        enemyScript.isChosen = true;
                        bossBrainP1.currentChosen = thisEnemy;
                    }
                    enemyScript.EventUpdater();
                }
                else if(i > maxActiveEnemies)               //Generates the backqueue of enemies to put into the inactiveEnemyQueue.
                {
                    //Instantiate in the upper pews and add to queue. Can either have them out of view from the arena, or actually in the pews.
                    int randomSpawnIndex = UnityEngine.Random.Range(0, upperSpawnPoints.Count - 1);
                    Vector3 currentUpperSpawn = upperSpawnPoints[randomSpawnIndex].transform.position;
                    GameObject thisEnemy = Instantiate(clergyPrefab, currentUpperSpawn, Quaternion.identity, queueHolder.transform);
                    ClergyBrain enemyScript = thisEnemy.GetComponent<ClergyBrain>();
                    inactiveEnemyQueue.Enqueue(thisEnemy);
                    thisEnemy.SetActive(true);
                    enemyScript.isActive = false;
                    enemyScript.EventUpdater();
                }
            }
            int combinedListCount = currentlyActiveEnemies.Count + inactiveEnemyQueue.Count;
            generatedEnemyLists = true;

            print($"EnemyPool generated with: {currentlyActiveEnemies.Count} currently active enemies, {inactiveEnemyQueue.Count} queued inactive enemies, and {combinedListCount} total enemies.");
            print($"Done in {generationElapsedTime} seconds.");
        }

        /// <summary>
        /// Gathers all GameObjects with the starting spawn target scripts.
        /// </summary>
        private void GenerateStartingLocations()
        {
            var scriptArray = FindObjectsByType<StartingLocation>(FindObjectsSortMode.None);
            foreach(StartingLocation currentRef in scriptArray)
            { 
                startSpawnLocations.Add(currentRef.gameObject);
            }
            generatedStarts = true;
            print($"Finished generating starting locations with {startSpawnLocations.Count} references.");
            print($"Done in {generationElapsedTime} seconds.");
        }

        /// <summary>
        /// Gathers all GameObjects that have the recurring spawn points (upper pew spawns) target scripts.
        /// </summary>
        private void GenerateSpawnPoints()
        { 
            var scriptArray = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
            foreach(SpawnPoint currentRef in scriptArray)
            { 
                upperSpawnPoints.Add(currentRef.gameObject);
            }
            generatedSpawns = true;
            print($"Finished generating spawn points with {upperSpawnPoints.Count} references.");
            print($"Done in {generationElapsedTime} seconds.");
        }

        /// <summary>
        /// Internal method for combing through collections to find whatever input and return a bool if found.
        /// </summary>
        /// <param name="inputRef"></param>
        /// <returns></returns>
        private (bool isCollected, string collectionName) CheckCollectionsFor(GameObject inputObject)
        {
            bool checkActive = currentlyActiveEnemies.Contains(inputObject);
            bool checkDying = dyingEnemies.Contains(inputObject);
            bool checkInactive = inactiveEnemyQueue.Contains(inputObject);
            bool isCollected = (checkActive || checkDying || checkInactive);
            bool dualCollection = ((checkActive && checkDying) || (checkActive && checkInactive) || (checkDying && checkInactive) || (checkActive && checkDying && checkInactive));

            if(dualCollection)
            {
                print($"Input Object is in multiple collections at once, as shown here:\nActiveEnemies: {checkActive.ToString()}\nDyingQueue: {checkDying.ToString()}\nInactiveQueue: {checkInactive.ToString()}");
                return (true,"Error");
            }
            else if(isCollected)         //Currently have this object within a singular collection
            {
                string outputCollName = new string("");
                if(checkActive)
                { 
                    outputCollName = "currentlyActiveEnemies";
                }
                else if(checkDying)
                { 
                    outputCollName = "dyingEnemies";
                }
                else if(checkInactive)
                { 
                    outputCollName = "inactiveEnemyQueue";
                }
                print($"Input Object is currently in {outputCollName}.");
                return (true, outputCollName);
            }
            else
            { 
                print("Object is not in any current collections. If generated during runtime make sure to add it to the SpawnManager collections.");
                return (false,"Error");
            }
        }

        /// <summary>
        /// Set of logic checks with bools to determine the state of the collections.
        /// </summary>
        private void CheckIfCollectionsFull()
        { 
            /*
             * GOT WAY TOO DISTRACTED BY BOOLEAN LOGIC GATES WHILE WRITING THIS. THIS MIGHT NOT BE FINISHED.
             */
            //Cached ints
            //External
            int targetTotalEnemies = CaliBossManager.MainManager.TotalEnemies;
            int targetActiveEnemies = CaliBossManager.MainManager.MaxArenaEnemies;
            int targetQueuedEnemies = targetTotalEnemies - targetActiveEnemies;
            //Internal
            int currentActive = currentlyActiveEnemies.Count;
            int currentInactive = inactiveEnemyQueue.Count;
            int currentDying = dyingEnemies.Count;

//      ----------------------------------------------------------------------------

            //CurrentlyActiveEnemies Bools
            bool arenaFull = currentActive == targetActiveEnemies;
            bool arenaOverfull = currentActive > targetActiveEnemies;
            bool arenaEmpty = currentActive < 1;
            bool arenaExpecting = !arenaOverfull && !arenaFull;

            //InactiveEnemyQueue Bools
            bool inactiveFull = currentInactive == targetQueuedEnemies;
            bool inactiveOverfull = currentInactive > targetQueuedEnemies;
            bool inactiveEmpty = currentInactive < 1;
            bool inactiveExpecting = !inactiveOverfull && !inactiveFull;

            //DyingQueue Bools
            bool dyingEmpty = currentDying < 1;

            //Foundational bools
            bool expectingDead = inactiveExpecting && !arenaExpecting;      //
            bool expectedDequeue = !inactiveExpecting && arenaExpecting;    //
            bool someDyingExpected = !dyingEmpty && expectingDead;          //

            //Combo bools
            //--Working as expected
            bool defaultStateExpectation = arenaFull && dyingEmpty && inactiveFull;

            //--Mirrored combos
            bool allEmpty = arenaEmpty && inactiveEmpty && dyingEmpty;

            //--Mixed combos
            bool lessThanTotalExpected = arenaExpecting && (inactiveEmpty || inactiveExpecting) && dyingEmpty;
            bool dyingTooSlowly = (arenaFull || arenaOverfull) && (inactiveFull || inactiveOverfull);
            bool notDyingProper = dyingTooSlowly && dyingEmpty;
        }
    }
}