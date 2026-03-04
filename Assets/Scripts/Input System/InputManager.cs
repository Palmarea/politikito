using System;
using UnityEngine;

namespace Game.Systems.Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;
        private PlayerInput InputHandler;

        // Events
        public event Action OnSelectPerformed;
        public event Action OnSelectStarted;
        public event Action OnSelectCanceled;

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
                if (InputHandler == null) InputHandler = GetComponent<PlayerInput>();
            }
        }
        #endregion

        public Vector2 GetMousePosition() => InputHandler.GetMousePosition();

        public Vector2 GetRawMousePosition() => InputHandler.GetRawMousePosition();

        private void OnEnable()
        {
            InputHandler.OnSelectPerformed += () => OnSelectPerformed?.Invoke();
            InputHandler.OnSelectStarted += () => OnSelectStarted?.Invoke();
            InputHandler.OnSelectCanceled += () => OnSelectCanceled?.Invoke();
        }

        private void OnDisable()
        {
            InputHandler.OnSelectPerformed -= () => OnSelectPerformed?.Invoke();
            InputHandler.OnSelectStarted -= () => OnSelectStarted?.Invoke();
            InputHandler.OnSelectCanceled -= () => OnSelectCanceled?.Invoke();
        }
    }
}
