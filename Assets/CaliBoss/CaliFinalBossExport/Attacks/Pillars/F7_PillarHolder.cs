using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Cali7 { 
    public class F7_PillarHolder : MonoBehaviour
    {
        [SerializeField] List<GameObject> pillarsHeld;

        public void StartRaisingPillars(int pillInd) {
            foreach(GameObject pillarGO in pillarsHeld) { 
                float randX = UnityEngine.Random.Range(-12f,12f);
                float randZ = UnityEngine.Random.Range(-12f,12f);
                Vector3 randPillPos = new Vector3(randX, 0, randZ);
                randPillPos += F7_RefManager.PLGS.gameObject.transform.position;
                randPillPos.y = -9;
                pillarGO.transform.position = randPillPos;
                pillarGO.SetActive(true);
            }
            pillarsHeld[pillInd].SetActive(true);
            pillarsHeld[pillInd].GetComponent<F7_PillarScript>().StartRising();
        }

        public void StartLowerPillar() { 

        }
    }
}