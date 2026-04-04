using Game.Systems.Input;
using UnityEngine;

namespace Game.Systems.Interaction.DragNDrop
{
    public class DragDropObject : MonoBehaviour
    {        
        protected bool isBeingDragged = false;
        protected Vector2 offset;
        private bool suscribed = false;

        private void Start()
        {
            if (!suscribed)
            {
                InputManager.Instance.OnSelectCanceled += StopDragging;
                suscribed = true;
            }
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
            transform.localPosition = Vector3.zero;
        }

        public virtual bool AllowToDrop()
        {
            return true;
        }

        public void ResetInitialPosition()
        {
            //initialPosition = transform.parent.localPosition;
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