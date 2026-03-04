using Game.Systems.Input;
using UnityEngine;

namespace Game.Systems.Interaction.DragNDrop
{
    public class DragDropObject : MonoBehaviour
    {
        private bool isBeingDragged = false;
        private Vector2 offset;
        private Vector3 initialPosition;

        private void Awake()
        {
            initialPosition = transform.position;
        }

        private void Update()
        {
            if (!isBeingDragged) return;

            transform.position = InputManager.Instance.GetMousePosition() + offset;
        }

        public void StartDragging()
        {
            isBeingDragged = true;
            offset = (Vector2)transform.position - InputManager.Instance.GetMousePosition();
        }

        public void StopDragging()
        {
            if (!isBeingDragged) return;

            isBeingDragged = false;

            BackToOrigin();
        }

        public void BackToOrigin()
        {
            transform.position = initialPosition;
        }

        private void OnEnable()
        {
            InputManager.Instance.OnSelectCanceled += StopDragging;
        }

        private void OnDisable()
        {
            if (InputManager.Instance != null)
                InputManager.Instance.OnSelectCanceled -= StopDragging;
        }
    }
}