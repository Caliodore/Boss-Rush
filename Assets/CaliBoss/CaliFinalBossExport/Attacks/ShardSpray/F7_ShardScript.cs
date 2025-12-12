using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Events;

namespace Cali7
{ 
    public class F7_ShardScript : MonoBehaviour
    {
        public bool printDebugLogs = true;
        [SerializeField] public float flightSpeed = 15;
        [Range(0f, 60f)] [SerializeField] public float turnSpeed = 6f;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Rigidbody sRB;
        [SerializeField] public Damager attachedDamager;
        [SerializeField] public F7_SIP parentScript;

        public UnityEvent OnFinishTrajectory;

        private Quaternion startRot;
        private Transform sTF;
        private Transform pTF;
        private GameObject playerObj;
        private LayerMask playerLayer;
        private LayerMask environLayer;

        public bool inFlight = false;

        private void Start()
        {
            attachedDamager.OnSuccessfulHit?.AddListener(() => DeactivateShard());
            //gameObject.SetActive(false);
            StartCoroutine(WaitForRefLoad());
        }

        IEnumerator WaitForRefLoad() { 
            yield return new WaitForSeconds(0.25f);
            playerObj = FindAnyObjectByType<PlayerLogic>().gameObject;
            sTF = gameObject.transform;
            pTF = playerObj.transform;
            startRot = transform.localRotation;
            sTF.position = spawnPoint.position;
            playerLayer = LayerMask.GetMask(LayerMask.LayerToName(playerObj.layer));
            environLayer = LayerMask.GetMask("Environment");
            DeactivateShard();
        }

        private void Update() {
            if(gameObject.activeSelf && !inFlight) { 
                //sTF.LookAt(pTF.position);
            }
        }

        public void SpawnAndHold() { 
            sTF.position = spawnPoint.position;
            sTF.localRotation = startRot;
            gameObject.SetActive(true);
        }

        public void StartMoving() { 
            gameObject.transform.parent = null;
            inFlight = true;
            StartCoroutine(ShootTowardsPlayer());
        }

        IEnumerator ShootTowardsPlayer() { 
            if(sTF == null)
                F7_Help.DebugPrint(printDebugLogs, $"sTF is null.");
            if(pTF == null)
                F7_Help.DebugPrint(printDebugLogs, $"pTF is null.");
            while(inFlight) { 
                sRB.AddForce((transform.forward * flightSpeed * Time.deltaTime),ForceMode.Force);
                Quaternion lookAtRot = transform.rotation;
                lookAtRot.SetLookRotation(pTF.position);
                Vector3 rotOut = lookAtRot.eulerAngles;
                sTF.transform.DORotate(rotOut, 1/turnSpeed);
                yield return null;
            }
        }

        public F7_SIP SetParentScript(F7_SIP refIn) { return refIn; }

        public void DeactivateShard() {
            F7_Help.DebugPrint(printDebugLogs, $"{gameObject.name} is requesting to be deactivated.");
            inFlight = false;
            parentScript.DisableShard();
        }

        private void OnTriggerEnter(Collider other)
        {
            string otherLayer = LayerMask.LayerToName(other.gameObject.layer);
            if(otherLayer.Equals(playerLayer)) {
                F7_Help.DebugPrint(printDebugLogs, $"The shard parent collided with the player in case the damager didn't realize. Otherwise, the shard should be being disabled.");
            }
            else if(otherLayer.Equals(environLayer)) { 
                F7_Help.DebugPrint(printDebugLogs, $"Shard parent collided with environment, removing shard.");
                DeactivateShard();
            }
            else { 
                F7_Help.DebugPrint(printDebugLogs, $"Shard parent collided with {other.gameObject.name} with layer name of {LayerMask.LayerToName(other.gameObject.layer)}.");
            }
        }
    }
}