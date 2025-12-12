using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Events;

namespace Cali7
{ 
    public class F7_ShardScript : MonoBehaviour
    {
        public bool refsLoaded = false;
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
        public LayerMask playerLayer;
        public LayerMask environLayer;
        public LayerMask eDmgLayer;
        public LayerMask otLayer;

        public bool inFlight = false;

        private void Start()
        {
            if(!F7_RefManager.Instance.gotRefs)
                F7_RefManager.OnRefsLoaded?.AddListener(() => SetReferences());
            else
                SetReferences();
        }

        public void SetReferences() { 
            attachedDamager.OnSuccessfulHit?.AddListener(() => DeactivateShard());
            playerObj = FindAnyObjectByType<PlayerLogic>().gameObject;
            sTF = gameObject.transform;
            pTF = playerObj.transform;
            startRot = transform.localRotation;
            sTF.position = spawnPoint.position;
            //playerLayer = LayerMask.GetMask(LayerMask.LayerToName(playerObj.layer));
            //environLayer = LayerMask.GetMask("Environment");
            //eDmgLayer = LayerMask.GetMask("EnemyDamage");
            //otLayer = LayerMask.GetMask("Other");
            refsLoaded = true;
            DeactivateShard();
        }

        private void Update() {
            if(refsLoaded) { 
                if(gameObject.activeSelf && !inFlight) { 
                    sTF.LookAt(pTF.position);
                }
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
                transform.DOLookAt(playerObj.transform.position, 1/turnSpeed);
                sRB.AddForce((transform.forward * flightSpeed),ForceMode.Acceleration);
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
            string otherLayerName = LayerMask.LayerToName(other.gameObject.layer);
            LayerMask otherLayer = other.gameObject.layer;

            switch(otherLayerName) { 
                case(nameof(otLayer)): 
                case(nameof(eDmgLayer)):
                    break;
                    
                case(nameof(environLayer)): 
                    F7_Help.DebugPrint(printDebugLogs, $"{gameObject.name} collided with environment.");
                    DeactivateShard();
                    break;
                    
                case(nameof(playerLayer)): 
                    F7_Help.DebugPrint(printDebugLogs, $"{gameObject.name} collided with player.");
                    DeactivateShard();
                    break;
                    
                default:
                    F7_Help.DebugPrint(printDebugLogs, $"{gameObject.name} collided with {other.gameObject.name}.");
                    break;
            }
        }
    }
}