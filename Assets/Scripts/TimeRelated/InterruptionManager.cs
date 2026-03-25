using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Managers.Timing
{
    public class InterruptionManager : MonoBehaviour
    {
        public static InterruptionManager Instance;

        #region Singleton
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(Instance);
            }
            else
            {
                Instance = this;
            }
        }
        #endregion

        [Header("Configuration")]
        [SerializeField] List<GameObject> InterumpibleObjects = new List<GameObject>();

        private bool inInterruption = false;
        public bool IsInInterruption => inInterruption;

        public void EnableInterruption()
        {
            if (InterumpibleObjects.Count <= 0) return;
            
            ApplyState(false);
        }

        public void DisableInteruption()
        {
            if (InterumpibleObjects.Count <= 0) return;

            ApplyState(true);
        }

        private void ApplyState(bool state)
        {
            foreach (GameObject obj in InterumpibleObjects)
            {
                obj.SetActive(false);
            }

            inInterruption = !state;
        }
    }
}