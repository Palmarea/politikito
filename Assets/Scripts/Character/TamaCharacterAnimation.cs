using UnityEngine;

namespace Game.Character
{
    [RequireComponent(typeof(TamaCharacterController))]
    public class TamaCharacterAnimation : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Animate()
        {

        }

        public void RequestAnimatorOverride(AnimatorOverrideController animatorOverride)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            float normalizedTime = currentState.normalizedTime;

            animator.runtimeAnimatorController = animatorOverride;

            animator.Play(currentState.fullPathHash, 0, normalizedTime);
        }
    }
}
