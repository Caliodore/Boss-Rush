using UnityEngine;

namespace Cali7
{ 
    public class EntranceScript : MonoBehaviour
    {
        public bool printDebugLogs = false;
        private void OnTriggerEnter(Collider other)
        {
            F7_Help.DebugPrint(printDebugLogs, "Object collided with trigger.");
            if(other.gameObject.GetComponent<PlayerLogic>() != null) {
                print("Object is player.");
                F7_EventManager.Instance.OnArenaEntered?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
