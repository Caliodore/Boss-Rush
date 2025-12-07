using UnityEngine;

namespace Cali7
{ 
    public class EntranceScript : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            print("Object collided with trigger.");
            if(other.gameObject.GetComponent<PlayerLogic>() != null) {
                print("Object is player.");
                F7_EventManager.Instance.OnArenaEntered?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
