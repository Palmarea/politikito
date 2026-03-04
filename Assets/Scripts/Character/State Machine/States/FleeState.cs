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

            float horizontal = (target.position.x > movement.Position.x) ? -1f : 1f;

            float time = Time.time * noiseSpeed;
            float ny = Mathf.PerlinNoise(noiseSeed, time * noiseScale);
            ny = (ny * 2f - 1f) * verticalAmount;

            Vector2 dir = new Vector2(horizontal, ny).normalized;

            movement.Move(dir * 1.5f);
        }
    }
}