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
            movement.Stop();
        }
    }
}