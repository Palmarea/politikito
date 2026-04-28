using Game.Character.StateMachine;
using UnityEngine;

namespace Game.Character.StateMachine.States
{
    public class FrozenState : TamaState
    {
        public FrozenState(TamaCharacterController character)
            : base(character)
        {
        }

        public override void Enter()
        {
            // Detener cualquier movimiento actual
            movement.Stop();
        }

        public override void FixedUpdate()
        {
            // No hacer nada → personaje totalmente quieto
        }

        public override void Exit()
        {
            // No necesitamos hacer nada aquí
            // El siguiente estado decidirá cómo moverse
        }
    }
}