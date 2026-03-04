using Game.Systems.Input;
using Game.Systems.Minigames;
using UnityEngine;

namespace Game.Systems.Interaction.DragNDrop
{
    public class DragDropObject : MonoBehaviour
    {
        private bool isBeingDragged = false;
        private Vector2 offset;
        private Vector3 initialPosition;
        private bool suscribed = false;

        private void Awake()
        {
            initialPosition = transform.position;
        }

        private void Start()
        {
            if (!suscribed)
            {
                InputManager.Instance.OnSelectCanceled += StopDragging;
                suscribed = true;
            }
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
            if (InputManager.Instance != null)
            {
                InputManager.Instance.OnSelectCanceled += StopDragging;
                suscribed = true;
            }
        }

        private void OnDisable()
        {
            if (InputManager.Instance != null)
                InputManager.Instance.OnSelectCanceled -= StopDragging;
        }
    }
}