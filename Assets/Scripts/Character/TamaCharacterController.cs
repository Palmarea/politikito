using UnityEngine;

namespace Game.Character
{
    public class TamaCharacterController : MonoBehaviour
    {
        private TamaCharacterMovement MovementHandler;
        private TamaCharacterAnimation AnimationHandler;

        private void Awake()
        {
            MovementHandler = GetComponent<TamaCharacterMovement>();
            AnimationHandler = GetComponent<TamaCharacterAnimation>();
        }

        private void Update()
        {
            if (MovementHandler != null)
            {
                MovementHandler.Move();
            }

            if (AnimationHandler != null)
            {
                AnimationHandler.Animate();
            }
        }
    }
}
