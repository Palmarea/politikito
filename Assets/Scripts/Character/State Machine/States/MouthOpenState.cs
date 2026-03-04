using Game.Character.StateMachine;
using UnityEngine;

namespace Game.Character.StateMachine.States
{
    public class MouthOpenState : TamaState
    {
        private float duration;
        private float timer;
        private System.Action onFinished;

        public MouthOpenState(
            TamaCharacterController character,
            float duration,
            System.Action onFinished)
            : base(character)
        {
            this.duration = duration;
            this.onFinished = onFinished;
        }

        public override void Enter()
        {
            timer = duration;

            //character.OpenMouth();
        }

        public override void Update()
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                onFinished?.Invoke();
            }
        }

        public override void Exit()
        {
            //character.CloseMouth();
        }
    }
}