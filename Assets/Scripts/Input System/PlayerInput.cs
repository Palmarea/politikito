using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Systems.Input
{
    public class PlayerInput : MonoBehaviour
    {
        private DefaultInputSystem InputSystem;
        private bool initialized = false;

        // Events
        public event Action OnSelectStarted;
        public event Action OnSelectPerformed;
        public event Action OnSelectCanceled;

        private void Awake()
        {
            InputSystem = new DefaultInputSystem();
            InitializeInputnEvents();
        }

        public Vector2 GetMousePosition()
        {
            return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }

        public Vector2 GetRawMousePosition()
        {
            return Mouse.current.position.ReadValue();
        }

        private void InitializeInputnEvents()
        {
            if (initialized) return;

            InputSystem.Enable();

            InputSystem.Game.Select.started += ctx => OnSelectStarted?.Invoke();
            InputSystem.Game.Select.performed += ctx => OnSelectPerformed?.Invoke();
            InputSystem.Game.Select.canceled += ctx => OnSelectCanceled?.Invoke();

            initialized = true;
        }

        private void UnInitializeInputnEvents()
        {
            InputSystem.Disable();

            InputSystem.Game.Select.started -= ctx => OnSelectStarted?.Invoke();
            InputSystem.Game.Select.performed -= ctx => OnSelectPerformed?.Invoke();
            InputSystem.Game.Select.canceled -= ctx => OnSelectCanceled?.Invoke();
        }

        #region Input System
        private void OnEnable()
        {
            InitializeInputnEvents();
        }

        private void OnDisable()
        {
            UnInitializeInputnEvents();
        }
        #endregion
    }
}
