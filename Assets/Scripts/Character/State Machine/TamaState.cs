namespace Game.Character.StateMachine
{
    public abstract class TamaState
    {
        protected TamaCharacterController character;
        protected TamaCharacterMovement movement;

        public TamaState(TamaCharacterController character)
        {
            this.character = character;
            this.movement = character.MovementHandler;
        }

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void Exit() { }
    }
}