using UnityEngine;

namespace Game.Character.StateMachine.States
{
    public class RoamState : TamaState
    {
        private Vector2 target;
        private float timer;
        private bool moving;

        private float noiseSeedX;
        private float noiseSeedY;

        private float noiseSpeed = 0.8f;
        private float noiseScale = 1.5f;
        private float noiseAmount = 0.3f;

        public RoamState(TamaCharacterController character) : base(character)
        {
            noiseSeedX = Random.Range(0f, 1000f);
            noiseSeedY = Random.Range(0f, 1000f);
        }

        public override void Enter()
        {
            ChooseNext();
        }

        public override void FixedUpdate()
        {
            timer -= Time.fixedDeltaTime;

            if (!moving)
            {
                if (timer <= 0f)
                    ChooseNext();
                return;
            }

            if (Vector2.Distance(movement.Position, target) < 0.1f || timer <= 0f)
            {
                ChooseNext();
                return;
            }

            Vector2 baseDir = (target - movement.Position).normalized;

            float time = Time.time * noiseSpeed;

            float nx = Mathf.PerlinNoise(noiseSeedX, time * noiseScale);
            float ny = Mathf.PerlinNoise(noiseSeedY, time * noiseScale);

            nx = (nx * 2f - 1f) * noiseAmount;
            ny = (ny * 2f - 1f) * noiseAmount;

            Vector2 dir = (baseDir + new Vector2(nx, ny)).normalized;

            movement.Move(dir);
        }

        private void ChooseNext()
        {
            moving = Random.value > 0.4f;

            if (moving)
            {
                target = movement.Position + Random.insideUnitCircle * 2f;
                timer = Random.Range(1f, 3f);
            }
            else
            {
                timer = Random.Range(0.5f, 2f);
            }
        }
    }
}