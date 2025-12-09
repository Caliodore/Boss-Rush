using UnityEngine;

namespace Cali7
{ 
    public class F7_ForwardCast : MonoBehaviour
    {
        [SerializeField] Color rayColor = Color.azure;
        private void Update()
        {
            /*Vector3 dirVec = F7_RefManager.PLGS.gameObject.transform.position - gameObject.transform.position;
            dirVec.y = 0;
            dirVec.Normalize();
            dirVec *= 3;*/
            Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward, rayColor, 0.5f);
            Debug.DrawLine(gameObject.transform.position, F7_RefManager.PLGS.gameObject.transform.position, Color.hotPink, 0.5f);
        }
    }
}
