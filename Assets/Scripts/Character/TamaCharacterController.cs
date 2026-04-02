using System.Xml;
using UnityEngine;
using Game.Character.StateMachine;
using Game.Character.StateMachine.States;

namespace Game.Character
{
    public class TamaCharacterController : MonoBehaviour
    {
        public TamaStateMachine StateMachine { get; private set; }
        public TamaCharacterMovement MovementHandler { get; private set; }
        private TamaCharacterAnimation AnimationHandler;

        private void Awake()
        {
            MovementHandler = GetComponent<TamaCharacterMovement>();
            AnimationHandler = GetComponent<TamaCharacterAnimation>();
            StateMachine = new TamaStateMachine();
        }

        private void Start()
        {
            ChangeState(new RoamState(this));
        }

        private void Update()
        {
            AnimationHandler.Animate();
            StateMachine.Update();
            //Debug.Log(StateMachine.CurrentState.GetType());
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        public void ChangeState(TamaState newState)
        {
            StateMachine.ChangeState(newState);
        }
    }
}
