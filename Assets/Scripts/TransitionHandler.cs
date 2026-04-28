using System;
using UnityEngine;

namespace Game.UI
{
    public class TransitionHandler : MonoBehaviour
    {
        private Animator Animator;

        public event Action OnTransBlackEnded;
        public event Action OnTransTransparentEnded;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }

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
