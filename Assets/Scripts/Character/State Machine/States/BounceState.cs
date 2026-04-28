using UnityEngine;

namespace Game.Character.StateMachine.States
{
    public class BounceState : TamaState
    {
        private float direction = 1f;

        private float noiseSeed;
        private const float noiseSpeed = 0.8f;
        private const float noiseScale = 1.5f;
        private const float verticalAmount = 0.4f;

        public BounceState(TamaCharacterController character)
            : base(character)
        {
            noiseSeed = Random.Range(0f, 1000f);

            direction = Random.value > 0.5f ? 1f : -1f;
        }

        public override void FixedUpdate()
        {
            Vector2 pos = movement.Position;

            if (pos.x <= movementMinX())
            {
                direction = 1f;
            }
            else if (pos.x >= movementMaxX())
            {
                direction = -1f;
            }

            float time = Time.time * noiseSpeed;
            float ny = Mathf.PerlinNoise(noiseSeed, time * noiseScale);
            ny = (ny * 2f - 1f) * verticalAmount;

            Vector2 dir = new Vector2(direction, ny).normalized;

            movement.Move(dir);
        }

        private float movementMinX() => character.MovementHandler.GetMinX();
        private float movementMaxX() => character.MovementHandler.GetMaxX();
    }
}