using Game.Systems.Milestone;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Character.Visual
{    
    public class TamaCharacterVisual : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TamaCharacterAnimation AnimationHandler;
        [SerializeField] private MilestonePresenter MilestonePresenter;

        [Header("Sprite References")]
        [SerializeField] private List<AnimatorOverrideController> CharacterLevelAOC= new List<AnimatorOverrideController>();
        private AnimatorOverrideController currentCharacterAnimator;

        private bool suscribed = false;

        private void Start()
        {
            if (!suscribed)
            {
                MilestonePresenter.OnMilestoneShown += RequestVisualEvolution;
            }
        }

        public void RequestVisualEvolution(int level)
        {
            var aoc = GetCharacterAOC(level - 1);

            if (aoc != currentCharacterAnimator)
            {
                currentCharacterAnimator = aoc;
                AnimationHandler.RequestAnimatorOverride(currentCharacterAnimator);
            }
        }

        private AnimatorOverrideController GetCharacterAOC(int level)
        {
            level = Mathf.Clamp(level, 0, CharacterLevelAOC.Count - 1);

            return CharacterLevelAOC[level];
        }

        private void OnEnable()
        {
            if (MilestonePresenter != null)
            {
                MilestonePresenter.OnMilestoneShown += RequestVisualEvolution;
            }
        }

        private void OnDisable()
        {
            MilestonePresenter.OnMilestoneShown -= RequestVisualEvolution;
        }
    }
}
