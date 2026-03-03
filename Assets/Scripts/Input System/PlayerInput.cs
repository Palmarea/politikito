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
        public event Action OnSelectPerformed;

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

            // Select
            InputSystem.Game.Select.performed += (InputAction.CallbackContext obj) => OnSelectPerformed?.Invoke();

            initialized = true;
        }

        private void UnInitializeInputnEvents()
        {
            InputSystem.Disable();

            //Select
            InputSystem.Game.Select.performed -= (InputAction.CallbackContext obj) => OnSelectPerformed?.Invoke();
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
