using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Cali7
{ 
    public class F7_ShardScript : MonoBehaviour
    {
        [SerializeField] public float flightSpeed = 15;
        [SerializeField] public float turnSpeed = 6;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Rigidbody sRB;

        private Quaternion startRot;
        private Transform sTF;
        private Transform pTF;
        private GameObject playerObj;

        public bool inFlight = false;

        private void Start()
        {
            gameObject.SetActive(false);
            sTF = gameObject.transform;
            playerObj = F7_RefManager.PLGS.gameObject;
            pTF = playerObj.transform;
            startRot = transform.localRotation;
            sTF.position = spawnPoint.position;
        }

        private void Update()
        {
            if(gameObject.activeSelf && !inFlight) { 
                sTF.LookAt(pTF.position);
            }
        }

        public void SpawnAndHold() { 
            sTF.position = spawnPoint.position;
            sTF.localRotation = startRot;
            gameObject.SetActive(true);
        }

        public void StartMoving() { 
            inFlight = true;
            StartCoroutine(ShootTowardsPlayer());
        }

        IEnumerator ShootTowardsPlayer() { 
            while(inFlight) { 
                sRB.AddForce((transform.forward * flightSpeed * Time.deltaTime),ForceMode.Force);
                sTF.transform.DORotate(pTF.position, turnSpeed);
                yield return null;
            }
        }

        public void RemoveShard() {
            gameObject.SetActive(false);
            inFlight = false;
        }
    }
}