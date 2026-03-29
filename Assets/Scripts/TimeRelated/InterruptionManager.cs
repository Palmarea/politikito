using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Managers.Timing
{
    public enum InterruptionType
    {
        TRANSITION,
        CINEMATIC,
        NOTIFICATION
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

        private bool inInterruption = false;
        public bool IsInInterruption => inInterruption;

        public void EnableInterruption(InterruptionType interruptionType)
        {
            //if (!inInterruption)
            //{
            //}
            //else
            //{
            //    Debug.LogWarning("Interruption ignored because already interrupted");
            //}
                inInterruption = true;
                OnInterruptStart?.Invoke(interruptionType);
        }

        public void DisableInteruption()
        {
            inInterruption = false;
            OnInterruptEnd?.Invoke();
        }
    }
}