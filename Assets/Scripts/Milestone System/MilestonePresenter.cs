using Game.Managers.Timing;
using Game.Systems.Achievement;
using Game.Systems.CameraControl;
using Game.Systems.Milestone.Inspect;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems.Milestone
{
    public class MilestonePresenter : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Animator Animator;
        [SerializeField] private MilestoneSystem MainSystem;
        [SerializeField] private AchievementPresenter PairSystem;
        [SerializeField] private MilestoneInspectSystem InspectSystem;
        [SerializeField] private CameraController CameraController;

        [Header("UI References")]
        [SerializeField] private GameObject MilestoneCanvasUI;
        [SerializeField] private Image MilestoneImageUI;
        [SerializeField] private TextMeshProUGUI MilestoneDescriptionUI;
        [SerializeField] private Button MilestoneNext;
        [SerializeField] private GameObject TutorialPostIt;

        [Header("Milestone Images")]
        [SerializeField] private List<Sprite> MilestoneImages;

        [Header("Fallback Timer")]
        [SerializeField] private float fallbackDelay = 5f;

        public event Action<int> OnMilestoneShown;
        public event Action OnLastMilestoneShown;
        private Milestone currentMilestone;
        private bool hasBeenRequested = false;

        private bool specificSceneDone = false;
        private bool endMilestoneTriggered = false;
        int counter = 0;

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

        private void RequestForced(Milestone milestone)
        {
            hasBeenRequested = true;
            currentMilestone = milestone;

            ShowMilestone();
        }

        private void ShowMilestone()
        {
            if (!hasBeenRequested) return;

            MilestoneDescriptionUI.SetText(string.Format(currentMilestone.description, GameData.Instance.GetPlayerName()));
            MilestoneCanvasUI.SetActive(true);

            InspectSystem.RequestMDOCreation(currentMilestone);

            MilestoneImageUI.sprite = MilestoneImages[currentMilestone.level - 1];
            MilestoneImageUI.gameObject.SetActive(true);

            MilestoneNext.gameObject.SetActive(false);
            
            SFXCaller.Play("event:/LevelUpGrande");
            Animator.SetTrigger("PresentNews");

            InterruptionManager.Instance.EnableInterruption(InterruptionType.NOTIFICATION);
        }

        public void OnPresentAnimationFinished()
        {
            MilestoneNext.gameObject.SetActive(true);
            MilestoneNext.onClick.RemoveListener(OnNextPressed);
            MilestoneNext.onClick.AddListener(OnNextPressed);
            OnMilestoneShown?.Invoke(currentMilestone.level);

            if (counter == 0)
            {
                TutorialPostIt.SetActive(false);
                counter++;
            }   
        }

        private void OnNextPressed()
        {
            MilestoneNext.onClick.RemoveListener(OnNextPressed);
            MilestoneNext.gameObject.SetActive(false);
            Animator.SetTrigger("HideNews");

            if (currentMilestone.level == 5 && !endMilestoneTriggered)
            {
                endMilestoneTriggered = true;
                OnLastMilestoneShown?.Invoke();
            }

            CameraController.RequestForcedSectionRefresh();
        }

        public void OnHideAnimationFinished()
        {
            HideMilestone();
        }

        private void HideMilestone()
        {
            InterruptionManager.Instance.DisableInteruption();
            MilestoneCanvasUI.SetActive(false);
            MilestoneImageUI.gameObject.SetActive(false);
            hasBeenRequested = false;

            if (currentMilestone.level == 3 && !specificSceneDone)
            {
                specificSceneDone = true;
                MainSystem.ForceAdvance(3);
            }
        }

        private void OnEnable()
        {
            MainSystem.OnMilestoneReached += RequestButWait;
            MainSystem.OnForcedMilestoneReached += RequestForced;
            PairSystem.OnAchievementNotificationHided += ShowMilestone;
        }

        private void OnDisable()
        {
            MainSystem.OnMilestoneReached -= RequestButWait;
            MainSystem.OnForcedMilestoneReached += RequestForced;
            PairSystem.OnAchievementNotificationHided -= ShowMilestone;
        }
    }
}