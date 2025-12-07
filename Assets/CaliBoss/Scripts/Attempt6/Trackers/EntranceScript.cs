using UnityEngine;

namespace Cali6
{ 
    public class EntranceScript : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            print("Object collided with trigger.");
            if(other.gameObject.GetComponent<PlayerLogic>() != null) {
                print("Object is player.");
                A6_AnimManager.Instance.PlayerEnterArena();
                Destroy(gameObject);
            }
        }
    }
}
