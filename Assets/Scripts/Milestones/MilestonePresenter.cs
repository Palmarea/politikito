using Game.Systems.Achievement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems.Milestone
{
    public class MilestonePresenter : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Animator Animator;

        [Header("UI References")]
        [SerializeField] private MilestoneSystem MainSystem;
        [SerializeField] private AchievementPresenter PairSystem;
        [SerializeField] private GameObject MilestoneCanvasUI;
        [SerializeField] private Image MilestoneImageUI;
        [SerializeField] private Button MilestoneNext;

        [Header("Milestone Images")]
        [SerializeField] private List<Sprite> MilestoneImages;

        public event Action<int> OnMilestoneShown;
        private Milestone currentMilestone;
        private bool hasBeenRequested = false;

        private void Awake()
        {
            MilestoneCanvasUI.SetActive(false);
            MilestoneNext.gameObject.SetActive(false);
        }

        private void RequestButWait(Milestone milestone)
        {
            hasBeenRequested = true;
            currentMilestone = milestone;
        }

        private void ShowMilestone()
        {
            if (!hasBeenRequested) return;

            MilestoneCanvasUI.SetActive(true);

            MilestoneImageUI.sprite = MilestoneImages[currentMilestone.level - 1];
            MilestoneImageUI.gameObject.SetActive(true);

            MilestoneNext.gameObject.SetActive(false);

            Animator.SetTrigger("PresentNews");
        }

        // llamado por Animation Event al terminar PresentNews
        public void OnPresentAnimationFinished()
        {
            MilestoneNext.gameObject.SetActive(true);
            MilestoneNext.onClick.RemoveListener(OnNextPressed);
            MilestoneNext.onClick.AddListener(OnNextPressed);
            OnMilestoneShown?.Invoke(currentMilestone.level);
        }

        private void OnNextPressed()
        {
            MilestoneNext.onClick.RemoveListener(OnNextPressed);
            MilestoneNext.gameObject.SetActive(false);
            Animator.SetTrigger("HideNews");
        }

        // llamado por Animation Event al terminar HideNews
        public void OnHideAnimationFinished()
        {
            HideMilestone();
        }

        private void HideMilestone()
        {
            MilestoneCanvasUI.SetActive(false);
            MilestoneImageUI.gameObject.SetActive(false);
            hasBeenRequested = false;
        }

        private void OnEnable()
        {
            MainSystem.OnMilestoneReached += RequestButWait;
            PairSystem.OnAchievementNotificationHided += ShowMilestone;
        }

        private void OnDisable()
        {
            MainSystem.OnMilestoneReached -= RequestButWait;
            PairSystem.OnAchievementNotificationHided -= ShowMilestone;
        }
    }
}