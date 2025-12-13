using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Cali7 { 
    public class F7_MatSwapper : MonoBehaviour
    {
        public static F7_MatSwapper Instance;
        public Color rageColor;
        [SerializeField] Renderer[] bossRenderers;

        public bool isRageMats;

        public UnityEvent<bool> OnSwapMaterials;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        private void Start()
        {
            OnSwapMaterials ??= new();
            OnSwapMaterials?.AddListener(swapState => SwapMats(swapState));
        }

        public void SwapMats(bool swapToRage) { 
            isRageMats = swapToRage;
            if(swapToRage) { 
                foreach(Renderer currentRenderer in bossRenderers)
                    StartCoroutine(HandleFlashMaterialSwap(currentRenderer));
            }
        }

        IEnumerator HandleFlashMaterialSwap(Renderer renderer)
        {
            Material[] originalMats = new Material[renderer.materials.Length];

            for (int i = 0; i < originalMats.Length; i++)
            {
                originalMats[i] = renderer.materials[i];
            }

            Material[] newMats = new Material[renderer.materials.Length];

            for (int i = 0; i < newMats.Length; i++)
            {
                Material rageMaterial = originalMats[i];

                rageMaterial.color += rageColor;

                newMats[i] = rageMaterial;
            }

            renderer.materials = newMats;

            while(isRageMats) { 
                yield return null;
            }
            if(!isRageMats)
                renderer.materials = originalMats;
        }
    }
}