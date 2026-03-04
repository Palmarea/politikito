using UnityEngine;

namespace Game.Character
{
    [RequireComponent(typeof(Rigidbody2D), typeof(TamaCharacterController))]
    public class TamaCharacterMovement : MonoBehaviour
    {
        [Header("Zone Limits")]
        [SerializeField] private Vector2 MinBounds;
        [SerializeField] private Vector2 MaxBounds;

        [Header("Movement Settings")]
        [SerializeField] private float BaseSpeed = 2f;
        [SerializeField] private Vector2 MoveDurationRange = new Vector2(1f, 3f);
        [SerializeField] private Vector2 IdleDurationRange = new Vector2(0.5f, 2f);

        [Header("Perlin Settings")]
        [SerializeField] private float NoiseSpeed = 0.8f;
        [SerializeField] private float NoiseScale = 1.5f;
        [SerializeField] private float VerticalNoiseAmount = 0.4f;
        [SerializeField] private float RoamNoiseAmount = 0.3f;

        private Rigidbody2D rb;

        private Vector2 currentTarget;
        private float stateTimer;
        private bool isMoving;

        private float speedMultiplier = 1f;
        private bool forceFlee = false;
        private Transform fleeTarget;

        private float noiseSeedX;
        private float noiseSeedY;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            noiseSeedX = Random.Range(0f, 1000f);
            noiseSeedY = Random.Range(0f, 1000f);

            ChooseNextState();
        }

        public void Move()
        {
            stateTimer -= Time.fixedDeltaTime;

            if (forceFlee && fleeTarget != null)
            {
                HandleFlee();
                return;
            }

            if (isMoving)
            {
                HandleRandomMovement();
            }
            else
            {
                if (stateTimer <= 0f)
                    ChooseNextState();
            }
        }

        #region Random Movement

        private void HandleRandomMovement()
        {
            if (Vector2.Distance(rb.position, currentTarget) < 0.1f || stateTimer <= 0f)
            {
                ChooseNextState();
                return;
            }

            Vector2 baseDirection = (currentTarget - rb.position).normalized;

            float time = Time.time * NoiseSpeed;

            float noiseX = Mathf.PerlinNoise(noiseSeedX, time * NoiseScale);
            float noiseY = Mathf.PerlinNoise(noiseSeedY, time * NoiseScale);

            noiseX = (noiseX * 2f - 1f) * RoamNoiseAmount;
            noiseY = (noiseY * 2f - 1f) * RoamNoiseAmount;

            Vector2 direction = (baseDirection + new Vector2(noiseX, noiseY)).normalized;

            MoveInDirection(direction);
        }

        private void ChooseNextState()
        {
            isMoving = Random.value > 0.4f;

            if (isMoving)
            {
                currentTarget = GetRandomPointInsideBounds();
                stateTimer = Random.Range(MoveDurationRange.x, MoveDurationRange.y);
            }
            else
            {
                stateTimer = Random.Range(IdleDurationRange.x, IdleDurationRange.y);
            }
        }

        private Vector2 GetRandomPointInsideBounds()
        {
            float x = Random.Range(MinBounds.x, MaxBounds.x);
            float y = Random.Range(MinBounds.y, MaxBounds.y);
            return new Vector2(x, y);
        }

        #endregion

        #region Flee

        private void HandleFlee()
        {
            if (fleeTarget == null)
                return;

            float horizontalDirection = (fleeTarget.position.x > rb.position.x) ? -1f : 1f;

            float time = Time.time * NoiseSpeed;

            float verticalNoise = Mathf.PerlinNoise(noiseSeedY, time * NoiseScale);
            verticalNoise = (verticalNoise * 2f - 1f) * VerticalNoiseAmount;

            Vector2 fleeDirection = new Vector2(horizontalDirection, verticalNoise).normalized;

            MoveInDirection(fleeDirection * 1.5f);
        }

        #endregion

        #region Core Movement

        private void MoveInDirection(Vector2 direction)
        {
            Vector2 velocity = direction * BaseSpeed * speedMultiplier;
            Vector2 nextPosition = rb.position + velocity * Time.fixedDeltaTime;

            nextPosition.x = Mathf.Clamp(nextPosition.x, MinBounds.x, MaxBounds.x);
            nextPosition.y = Mathf.Clamp(nextPosition.y, MinBounds.y, MaxBounds.y);

            rb.MovePosition(nextPosition);
        }

        #endregion

        #region External Control

        public void SetSpeedMultiplier(float multiplier)
        {
            speedMultiplier = multiplier;
        }

        public void ForceFlee(Transform target)
        {
            fleeTarget = target;
            forceFlee = true;
        }

        public void StopFlee()
        {
            forceFlee = false;
            fleeTarget = null;

            // Forzar pequeño idle para romper dirección anterior
            stateTimer = Random.Range(IdleDurationRange.x, IdleDurationRange.y);
            isMoving = false;
        }

        public void StopMovement()
        {
            enabled = false;
        }

        public void ResumeMovement()
        {
            enabled = true;
            ChooseNextState();
        }

        #endregion

        #region Gizmos

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            Vector3 center = (MinBounds + MaxBounds) / 2f;
            Vector3 size = MaxBounds - MinBounds;

            Gizmos.DrawWireCube(center, size);
        }

        #endregion
    }
}