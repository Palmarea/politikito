using Game.Systems.Input;
using UnityEngine;

namespace Game.Systems.Interaction.DragNDrop
{
    public class DragDropPhysicObject : DragDropObject
    {
        private Rigidbody2D rb;
        private bool freeFall = false;
        private Renderer rend;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rend = GetComponent<SpriteRenderer>();
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        protected override void Update()
        {
            if (freeFall)
            {
                if (!rend.isVisible)
                {
                    BackToOrigin();
                }
            }
            else if (isBeingDragged)
            {
                Vector2 targetPosition = InputManager.Instance.GetMousePosition() + offset;
                transform.position = targetPosition;
                rb.position = transform.position;
            }
        }

        public override void StartDragging()
        {
            base.StartDragging();

            freeFall = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
        }

        public override void StopDragging()
        {
            if (!isBeingDragged) return;

            isBeingDragged = false;
            freeFall = true;

            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
        }

        public override void BackToOrigin()
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            freeFall = false;
            rb.linearVelocity = Vector2.zero;
            // Esto es lo importante
            transform.localPosition = Vector3.zero;

            // sincroniza el rigidbody con el transform
            rb.position = transform.position;
        }

        public override bool AllowToDrop()
        {
            return freeFall;
        }
    }
}