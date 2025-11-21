using UnityEngine;
using Caliodore;
using Caliodore.States_Phase1;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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

        [Header("Events and Actions")]
        public UnityEvent ChosenDiedEvent;
        public UnityEvent ClergyDiedEvent;

        [Header("Component Refs")]
        [SerializeField] SpawnManager_Cali attachedSpawnManager;

        [Header("Boss Progression")]
        public float maxTotalBossHealth;
        public float currentTotalBossHealth;
        private float chosenHealthTotalImpact;
        private float clergyHealthTotalImpact;

        private void Awake()
        {
            CurrentPhase = 1;
            GenerateEvents();
        }

        /// <summary>
        /// To update the public variable currentChosen.
        /// </summary>
        /// <returns>The Chosen that is within enemiesInArena.</returns>
        public GameObject FindChosen()
        { 
            GameObject chosenObjRef = attachedSpawnManager.currentlyActiveEnemies.Find(x => x.GetComponent<P1_Clergy>().isChosen == true);
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
        /// To be called when enemies are added or removed from the arena, and when a new Chosen is decided. <br/>
        /// Adds and removes listeners from ChosenDied and ClergyDied as needed. <br/>
        /// Intended to be called AFTER all other events/updates when something dies.
        /// </summary>
        private void UpdateEvents(GameObject targetObject, bool addOrRemove)
        {
            bool objInArena = attachedSpawnManager.currentlyActiveEnemies.Contains(targetObject);
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

            foreach(GameObject currentKey in attachedSpawnManager.currentlyActiveEnemies)
            { 
                UpdateEvents(currentKey, true);
            }
        }

        /// <summary>
        /// To be invoked by ChosenDied. Handles choosing a new Chosen, removing the old listener, and requeuing the clergy.
        /// </summary>
        /// <param name="deadChosen">GameObj reference to the Chosen that is dying</param>
        private void OnChosenDeath(GameObject deadChosen) 
        {
            bool chosenCheck = deadChosen.GetComponent<P1_Clergy>().isChosen;
            if(!chosenCheck)
            { 
                OnChosenDeath(FindChosen());
            }
            else
            {
                UpdateBossHealth(chosenHealthTotalImpact);
                attachedSpawnManager.currentlyActiveEnemies.Remove(deadChosen);
                ChooseNewChosen();
            }
        }

        /// <summary>
        /// To be invoked by ClergyDied. Handles requeuing this member and signaling a new one to spawn.
        /// </summary>
        private void OnClergyDeath(GameObject deadClergy) 
        { 
            UpdateBossHealth(clergyHealthTotalImpact);
            attachedSpawnManager.currentlyActiveEnemies.Remove(deadClergy);
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
                P1_Clergy scriptToCheck = newChosen.GetComponent<P1_Clergy>();
                if(scriptToCheck.CurrentHealth == scriptToCheck.AttachedSO.MaxHealth)
                { 
                    scriptToCheck.isChosen = true;
                    scriptToCheck.AttachedSO = scriptToCheck.chosenSO;
                    currentChosen = newChosen;
                    ClergyToChosenEvents(newChosen);
                    choosingNewChosen = false;
                }
                else
                { 
                    potentialChosen.Remove(newChosen);
                    ChooseNewChosen();
                }
            }
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
