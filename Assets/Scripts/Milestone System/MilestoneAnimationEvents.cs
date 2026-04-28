using Game.Systems.Milestone;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Milestones
{
    public class MilestoneAnimationEvents : MonoBehaviour
    {
        [SerializeField] private MilestonePresenter presenter;

        public void PresentAnimationFinished()
        {
            presenter.OnPresentAnimationFinished();
        }

        public void HideAnimationFinished()
        {
            presenter.OnHideAnimationFinished();
        }
    }
}