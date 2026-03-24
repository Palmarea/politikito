using UnityEngine;

namespace Game.Character
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TamaCharacterMovement : MonoBehaviour
    {
        [Header("Zone Limits")]
        [SerializeField] private Vector2 MinBounds;
        [SerializeField] private Vector2 MaxBounds;

        [Header("Movement Settings")]
        [SerializeField] private float BaseSpeed = 2f;

        private Rigidbody2D rb;
        private float speedMultiplier = 1f;

        public Vector2 Position => rb.position;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Move(Vector2 direction)
        {
            Vector2 velocity = direction * BaseSpeed * speedMultiplier;
            Vector2 nextPosition = rb.position + velocity * Time.fixedDeltaTime;

            nextPosition.x = Mathf.Clamp(nextPosition.x, MinBounds.x, MaxBounds.x);
            nextPosition.y = rb.position.y;

            rb.MovePosition(nextPosition);
        }

        public void SetSpeedMultiplier(float multiplier)
        {
            speedMultiplier = multiplier;
        }

        public void Stop()
        {
            rb.linearVelocity = Vector2.zero;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            Vector3 center = (MinBounds + MaxBounds) / 2f;
            Vector3 size = MaxBounds - MinBounds;

            Gizmos.DrawWireCube(center, size);
        }
    }
}