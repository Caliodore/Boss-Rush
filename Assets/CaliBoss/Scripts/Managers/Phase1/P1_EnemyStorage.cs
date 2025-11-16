using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Caliodore
{
    /// <summary>
    /// Centralizes references to total enemy count, including queued/inactive enemies. </br>
    /// Mainly for use with SpawnManager and BossOverseer.
    /// </summary>
    public class P1_EnemyStorage : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] P1_SpawnManager attSpawnManager;

        [Header("Collections")]
        public Dictionary<string,GameObject> currentlyActiveEnemies = new Dictionary<string, GameObject>();     //For storing the singular chosen and all the regular clergy.
        public Queue<GameObject> dyingEnemies = new Queue<GameObject>();                                        //Queue that provides a buffer for the player.
        public Queue<GameObject> inactiveEnemyQueue = new Queue<GameObject>();                                  //For SpawnManager to pull from/reference.

        [Header("Properties for Other Scripts")]
        private int enemiesActive;
        private int queuedEnemies;

        public int QueuedEnemies {  get { return queuedEnemies; } }
        public int EnemiesActive {  get { return enemiesActive; } }

        public void Start()
        {
            
        }


    }
}
