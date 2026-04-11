using System;
using UnityEngine;

namespace Game.UI
{
    public class TransitionHandler : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Animator Animator;

        public event Action OnTransBlackEnded;
        public event Action OnTransTransparentEnded;

        public void RequestTransitionTB()
        {
            Animator.SetBool("Transit", true);
        }

        public void BlackTransitionEnded()
        {
            OnTransBlackEnded?.Invoke();
        }

        public void RequestTransitionTT()
        {
            Animator.SetBool("Transit", false);
        }

        public void TransparentTransitionEnded()
        {
            OnTransTransparentEnded?.Invoke();
        }
    }
}
