using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cali3
{ 
    public class T3_UI_Manager : MonoBehaviour
    {
        public static T3_UI_Manager UI_Manager;

        [Header("Component Refs")]
        [SerializeField] public Canvas BossCanvas;
        [SerializeField] public Bar bossHealthBar;

        [Header("Individual UI Elements")]
        [SerializeField] public TMP_Text bossTitle;
        [SerializeField] public Image bossInfoDisplay;

        private void Awake()
        {
            if(UI_Manager == null)
                UI_Manager = this;
        }

        private void Start()
        {
            //T3_Brain.MainBrain.
        }
    }
}
