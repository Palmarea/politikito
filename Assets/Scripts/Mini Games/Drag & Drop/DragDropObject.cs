using Game.Systems.Input;
using Game.Systems.Minigames;
using UnityEngine;

namespace Game.Systems.Interaction.DragNDrop
{
    public class DragDropObject : MonoBehaviour
    {
        protected bool isBeingDragged = false;
        protected Vector2 offset;
        protected Vector3 initialPosition;
        private bool suscribed = false;

        private void Start()
        {
            if (!suscribed)
            {
                InputManager.Instance.OnSelectCanceled += StopDragging;
                suscribed = true;
            }

            initialPosition = transform.position;
        }

        protected virtual void Update()
        {
            if (isBeingDragged)
            {
                transform.position = InputManager.Instance.GetMousePosition() + offset;
            }
        }

        public virtual void StartDragging()
        {
            isBeingDragged = true;
            offset = (Vector2)transform.position - InputManager.Instance.GetMousePosition();
        }

        public virtual void StopDragging()
        {
            if (!isBeingDragged) return;

            isBeingDragged = false;

            BackToOrigin();
        }

        public virtual void BackToOrigin()
        {
            transform.position = initialPosition;
        }

        public virtual bool AllowToDrop()
        {
            return true;
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