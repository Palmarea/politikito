using Game.Managers.Mouse;
using Game.Systems.Input;
using Game.Systems.Minigames;
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
                Vector3 mouseWorldPos = InputManager.Instance.GetMousePosition();
                mouseWorldPos.z = 0f;

                Vector3 localPos = transform.parent.InverseTransformPoint(mouseWorldPos);
                localPos.y = -300f / transform.parent.localScale.y; // fixed Y
                localPos.z = 0f;

                transform.localPosition = localPos;
                rb.position = localPos;
            }
        }

        public override void StartDragging()
        {
            isBeingDragged = true;
            offset = (Vector2)transform.position - InputManager.Instance.GetMousePosition();

            freeFall = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;

            GetComponent<ClickableObject>().IsInteractable = true;
        }

        public override void StopDragging()
        {
            if (!isBeingDragged) return;

            isBeingDragged = false;
            freeFall = true;

            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;

            MouseManager.Instance.ReleaseHold();

            GetComponent<ClickableObject>().IsInteractable = false;

        }

        public override void BackToOrigin()
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            freeFall = false;
            isBeingDragged = false;
            rb.linearVelocity = Vector2.zero;
            transform.localPosition = Vector3.zero;
            rb.position = transform.position;
            Physics2D.SyncTransforms();

            if (MinigameManager.Instance.IsMinigameActive)
            {
                MouseManager.Instance.SetHorizontalRestriction(false);
            }

            MouseManager.Instance.ReleaseHold();

            GetComponent<ClickableObject>().IsInteractable = true;

        }

        public override bool AllowToDrop()
        {
            return freeFall;
        }
    }
}