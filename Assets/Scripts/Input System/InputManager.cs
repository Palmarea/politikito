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
                DontDestroyOnLoad(this.gameObject);
            }
        }
        #endregion

        public Vector2 GetMousePosition() => InputHandler.GetMousePosition();

        public Vector2 GetRawMousePosition() => InputHandler.GetRawMousePosition();

        private void HandleSelectPerformed()
        {
            OnSelectPerformed?.Invoke();
        }

        private void HandleSelectStarted()
        {
            OnSelectPerformed?.Invoke();
        }

        private void HandleSelectCanceled()
        {
            OnSelectPerformed?.Invoke();
        }

        private void OnEnable()
        {
            InputHandler.OnSelectPerformed += HandleSelectPerformed;
            InputHandler.OnSelectStarted += HandleSelectStarted;
            InputHandler.OnSelectCanceled += HandleSelectCanceled;
        }

        private void OnDisable()
        {
            InputHandler.OnSelectPerformed -= HandleSelectPerformed;
            InputHandler.OnSelectStarted -= HandleSelectStarted;
            InputHandler.OnSelectCanceled -= HandleSelectCanceled;
        }
    }
}
