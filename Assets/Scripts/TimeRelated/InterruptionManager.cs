using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Managers.Timing
{
    public enum InterruptionType
    {
        TRANSITION,
        CINEMATIC,
        NOTIFICATION,
        MINIGAME
    }
    
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

        public static event Action<InterruptionType> OnInterruptStart;

        public static event Action OnInterruptEnd;

        public void EnableInterruption(InterruptionType interruptionType)
        {
            OnInterruptStart?.Invoke(interruptionType);
        }

        public void DisableInteruption()
        {
            OnInterruptEnd?.Invoke();
        }
    }
}