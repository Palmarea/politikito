namespace Game.Character.StateMachine
{
    public class TamaStateMachine
    {
        public TamaState CurrentState { get; private set; }

        public void ChangeState(TamaState newState)
        {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState?.Enter();
        }

        public void Update()
        {
            CurrentState?.Update();
        }

        public void FixedUpdate()
        {
            CurrentState?.FixedUpdate();
        }
    }
}