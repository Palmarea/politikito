using UnityEngine;

namespace Game.Character.StateMachine.States
{
    public class FleeState : TamaState
    {
        private Transform target;

        private float noiseSeed;
        private float noiseSpeed = 0.8f;
        private float noiseScale = 1.5f;
        private float verticalAmount = 0.4f;

        private float stuckCheckInterval = 0.5f;
        private float stuckTimer = 0f;
        private Vector2 lastPosition;
        private float stuckThreshold = 0.01f;
        private float forcedDirectionTimer = 0f;
        private float forcedDirectionDuration = 1.0f;
        private float forcedDirection = 0f;

        public FleeState(TamaCharacterController character, Transform target)
            : base(character)
        {
            this.target = target;
            noiseSeed = Random.Range(0f, 1000f);
        }

        public override void FixedUpdate()
        {
            if (target == null)
                return;

            Vector2 currentPosition = movement.Position;

            // Tick timers
            stuckTimer += Time.fixedDeltaTime;
            if (forcedDirectionTimer > 0f)
                forcedDirectionTimer -= Time.fixedDeltaTime;

            // Check if stuck every stuckCheckInterval seconds
            if (stuckTimer >= stuckCheckInterval)
            {
                float distanceMoved = Mathf.Abs(currentPosition.x - lastPosition.x);
                if (distanceMoved < stuckThreshold)
                {
                    // Stuck!! Force opposite direction
                    forcedDirection = (target.position.x > movement.Position.x) ? 1f : -1f;
                    forcedDirectionTimer = forcedDirectionDuration;
                }
                lastPosition = currentPosition;
                stuckTimer = 0f;
            }

            float horizontal;
            if (forcedDirectionTimer > 0f)
                horizontal = forcedDirection;
            else
                horizontal = (target.position.x > movement.Position.x) ? -1f : 1f;

            float time = Time.time * noiseSpeed;
            float ny = Mathf.PerlinNoise(noiseSeed, time * noiseScale);
            ny = (ny * 2f - 1f) * verticalAmount;

            Vector2 dir = new Vector2(horizontal, ny).normalized;

            movement.Move(dir * 1.5f);
        }
    }
}