using UnityEngine;
using System.Collections;

namespace Cali7 { 
    public class F7_BloodWall : MonoBehaviour
    {
        public float timeWallStays = 12f;
        public void TurnOffObj() { StartCoroutine(DisableCooldown()); }

        IEnumerator DisableCooldown() { 
            yield return new WaitForSeconds(timeWallStays);
            gameObject.SetActive(false);
        }
    }
}
