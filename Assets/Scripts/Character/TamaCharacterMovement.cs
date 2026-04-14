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

        private Vector2 m_InitialMinBounds;
        private Vector2 m_InitialMaxBounds;

        public Vector2 Position => rb.position;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            m_InitialMinBounds = MinBounds;
            m_InitialMaxBounds = MaxBounds;
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

        public void SetReducedBounds(float dir, float horizontalOffset)
        {
            //float securedMin = -8.1f + (-1 * 19.2f);
            //float securedMax = 8.1f + (1 * 19.2f);

            //float resolveMin = securedMin + horizontalOffset;
            //float resolveMax = securedMax + horizontalOffset;

            //// 1. Calcular tamaño del rango
            //float width = resolveMax - resolveMin;
            //float halfWidth = width / 2f;

            //// 2. Calcular centro
            //float center = (resolveMin + resolveMax) / 2f;

            //// 3. Clamp del centro para que el rango no se salga
            //float clampedCenter = Mathf.Clamp(
            //    center,
            //    securedMin + halfWidth,
            //    securedMax - halfWidth
            //);

            //// 4. Reconstruir límites
            //float finalMin = clampedCenter - halfWidth;
            //float finalMax = clampedCenter + halfWidth;

            //MinBounds = new Vector2(finalMin, MinBounds.y);
            //MaxBounds = new Vector2(finalMax, MaxBounds.y);

            //Debug.Log($"FINAL MIN: {finalMin} | FINAL MAX: {finalMax}");

            float securedMin = -8.1f + (-1 * 19.2f);
            float securedMax = 8.1f + (1 * 19.2f);

            // Este es el tamaño FIJO de tu zona (lo que antes era -8.1 a 8.1)
            float halfWidth = 8.1f;

            // Clamp del centro para que el bloque completo no se salga
            float clampedCenter = Mathf.Clamp(
                horizontalOffset,
                securedMin + halfWidth,
                securedMax - halfWidth
            );

            float finalMin = clampedCenter - halfWidth;
            float finalMax = clampedCenter + halfWidth;

            MinBounds = new Vector2(finalMin, MinBounds.y);
            MaxBounds = new Vector2(finalMax, MaxBounds.y);

            Debug.Log($"CENTER: {clampedCenter} | MIN: {finalMin} | MAX: {finalMax}");
        }

        public void ResetBounds()
        {
            MinBounds = m_InitialMinBounds;
            MaxBounds = m_InitialMaxBounds;
        }

        public void Stop()
        {
            rb.linearVelocity = Vector2.zero;
        }

        public float GetMinX()
        {
            return MinBounds.x;
        }

        public float GetMaxX()
        {
            return MaxBounds.x;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Vector3 center = (MinBounds + MaxBounds) / 2f;
            Vector3 size = MaxBounds - MinBounds;

            Gizmos.DrawWireCube(center, size);
        }
    }
}