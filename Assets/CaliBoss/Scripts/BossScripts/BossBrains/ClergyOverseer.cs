using Caliodore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Caliodore
{ 
    public class ClergyOverseer : BossBrain
    {
        [Header("State Refs")]
        public bool bossAlerted;            //Bool that is associated with showing the health bar and whether or not enemies are in the aggro state.
        public GameObject currentChosen;    //Reference for other scripts to know which enemy is Chosen at a glance.
        public float chosenBuffRange = 10f;

        [Header("Component Refs")]
        [SerializeField] public SpawnManager_Cali attachedSpawnManager;
        private ClergyBrain chosenScript;
        private ClergySM chosenSM;

        [Header("Boss Progression")]
        public float maxTotalBossHealth;
        public float currentTotalBossHealth;
        private float chosenHealthTotalImpact;
        private float clergyHealthTotalImpact;

        [Header("Collections")]
        public List<ClergyBrain> currentlyBuffedEnemies = new List<ClergyBrain>();

        public static ClergyOverseer Instance;

        private void Awake()
        {
            CurrentPhase = 1;
        }

        public GameObject FindChosen()
        { 
            GameObject chosenObjRef = attachedSpawnManager.currentlyActiveEnemies.Find(x => x.GetComponent<ClergyBrain>().isChosen == true);
            if(chosenObjRef != null)
            { 
                currentChosen = chosenObjRef;
            }
            else
            { 
                ChooseNewChosen();    
            }
            return currentChosen;
        }

        /// <summary>
        /// Method to be called when a Chosen dies, or during the beginning generation. <br/>
        /// Selects from enemiesInArena randomly and signifies them as Chosen.
        /// </summary>
        private void ChooseNewChosen() 
        {
            bool choosingNewChosen = true;
            List<GameObject> potentialChosen = attachedSpawnManager.currentlyActiveEnemies;
            while(choosingNewChosen)
            { 
                int randomIndex = UnityEngine.Random.Range(0, potentialChosen.Count);
                GameObject newChosen = potentialChosen[randomIndex];
                ClergyBrain scriptToCheck = newChosen.GetComponent<ClergyBrain>();
                if(scriptToCheck.CurrentHealth == scriptToCheck.AttachedSO.MaxHealth)
                {
                    SetNewChosenRefs(newChosen);
                    choosingNewChosen = false;
                }
                else
                { 
                    potentialChosen.Remove(newChosen);
                    if(potentialChosen.Count <= 0)
                    { 
                        print("No active enemies are currently at max health, choosing first in the index as new chosen.");
                        GameObject forcedNewChosen = attachedSpawnManager.currentlyActiveEnemies[randomIndex];
                        ClergyBrain forcedScriptToCheck = forcedNewChosen.GetComponent<ClergyBrain>();
                        SetNewChosenRefs(forcedNewChosen);
                        choosingNewChosen = false;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Space-saving method to call to set new internal references for easier and cleaner usage.
        /// </summary>
        /// <param name="newChosen">The incoming chosen GameObjRef.</param>
        private void SetNewChosenRefs(GameObject newChosen)
        {
            ClergyBrain scriptToCheck = newChosen.GetComponent<ClergyBrain>();
            scriptToCheck.OnBeingChosen.Invoke();
            currentChosen = newChosen;
            chosenScript = newChosen.GetComponent<ClergyBrain>();
            chosenSM = newChosen.GetComponent<ClergySM>();
        }

        /// <summary>
        /// To be invoked by ClergyDied. Handles requeuing this member and signaling a new one to spawn.
        /// </summary>
        /// <param name="deadClergy">GameObj reference to the Clergy that is dying</param>
        public void OnClergyDeath(GameObject deadClergy) 
        {
            ClergyBrain chosenScript = deadClergy.GetComponent<ClergyBrain>();
            bool chosenCheck = chosenScript.isChosen;
            float healthDamage = 0f;

            attachedSpawnManager.ClergyQueueAccessor(deadClergy);

            if(chosenCheck)
            { 
                healthDamage = chosenHealthTotalImpact;
                ChooseNewChosen();
            }
            else
            { 
                healthDamage = clergyHealthTotalImpact;
            }
            UpdateBossHealth(healthDamage);
        }

        /// <summary>
        /// Will interact with UI script to feed it the amount it should affect the health bar by when an enemy takes damage.
        /// </summary>
        /// <param name="percentDealt">Decimal form of the percentage of the health bar that should be deducted.</param>
        private void UpdateBossHealth(float percentDealt) 
        { 
                
        }

        //Might make the buff persist for a few seconds once leaving range?
        /// <summary>
        /// Finds the enemies within the buffing range and buffs them while in range.
        /// </summary>
        public void BuffClergyMembers()
        {
            if(currentChosen != null) 
            { 
                   
            }
            else
                print("CurrentChosen is null, make sure FindChosen is used first.");
        }
    }
}
