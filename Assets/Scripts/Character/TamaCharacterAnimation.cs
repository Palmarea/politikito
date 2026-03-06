using UnityEngine;

namespace Game.Character
{
    [RequireComponent(typeof(TamaCharacterController))]
    public class TamaCharacterAnimation : MonoBehaviour
    {
        [SerializeField] private Animator Animator;

        public void Animate()
        {

        }

        public void RequestAnimatorOverride(AnimatorOverrideController animatorOverride)
        {
            AnimatorStateInfo currentState = Animator.GetCurrentAnimatorStateInfo(0);
            float normalizedTime = currentState.normalizedTime;

            Animator.runtimeAnimatorController = animatorOverride;

            Animator.Play(currentState.fullPathHash, 0, normalizedTime);
        }

        public void SetMiniGame(int value)
        {
            Animator.SetInteger("MiniGame", value);
        }

        public void SetReceivingWater(bool value)
        {
            Animator.SetBool("IsReceivingWater", value);
        }

        public void SetMouthOpen(bool value)
        {
            Animator.SetBool("MouthOpen", value);
        }

        public void SetHoldingWeight(bool value)
        {
            Animator.SetBool("HoldingWeight", value);
        }

        public void SetWaitingInput(bool value)
        {
            Animator.SetBool("WaitingForInput", value);
        }
    }
}
