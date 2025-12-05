using TMPro;
using UnityEngine;

namespace Cali6
{ 
    public class A6_UIManager : MonoBehaviour
    {
        [Header("Canvases")]
        [SerializeField] public Canvas playerHealthCanvas;
        [SerializeField] public Canvas bossHealthCanvas;
        [SerializeField] public Canvas pauseScreenCanvas;

        [Header("Brolive Components")]
        [SerializeField] public Bar bossHealthBar;
        [SerializeField] public PlayerHealthDisplay playerHealthDisplay;

        [Header("Misc")]
        [SerializeField] public TMP_Text bossTitleText;

        private int currentHealth;
        private int maxHealth;

        public static A6_UIManager Instance;
        public A6_UIManager() { }

        private void Start()
        {
            currentHealth = maxHealth = A6_Brain.Instance.maxHealth;
        }

        public void EnableBossDisplay() { 
            bossHealthCanvas.enabled = true;
        }

        public void UpdateHealth(int healthChange) {
            currentHealth -= healthChange;
            bossHealthBar.UpdateBar(healthChange, currentHealth);
        }
    }
}
