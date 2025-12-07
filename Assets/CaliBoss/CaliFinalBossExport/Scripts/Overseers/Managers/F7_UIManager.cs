using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Cali7
{ 
    public class F7_UIManager : MonoBehaviour
    {
        public static F7_UIManager Instance;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        private void Start()
        {
            SetEvents();
            F7_RefManager.UIBT.text = "Hostellus, Eternal Martyr";
        }

        private void SetEvents() { F7_EventManager.Instance.OnArenaEntered?.AddListener(() => EnableBossUI()); }

        public void EnableBossUI() { 
            F7_RefManager.UIBC.gameObject.SetActive(true);
            F7_RefManager.UIPC.gameObject.SetActive(true);
        }

        public void UpdateBossHealth(int healthChange) { 
            F7_RefManager.UIBH.UpdateBar(healthChange, F7_RefManager.BCNT.currentHealth);
        }
    }
}
