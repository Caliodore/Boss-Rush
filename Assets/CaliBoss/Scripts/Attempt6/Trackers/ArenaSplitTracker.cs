using Caliodore;
using UnityEngine;

namespace Cali6
{ 
    public class ArenaSplitTracker : MonoBehaviour
    {
        public bool playerInArea;

        private void OnTriggerStay(Collider other)
        {
            if(other.transform.gameObject == A6_Brain.Instance.playerScript.gameObject) { 
                playerInArea = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.transform.gameObject == A6_Brain.Instance.playerScript.gameObject) { 
                playerInArea = false;
            }
        }
    }
}
