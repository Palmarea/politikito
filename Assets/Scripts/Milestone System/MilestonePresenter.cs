using Game.Managers.Timing;
using Game.Systems.Achievement;
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

        private Coroutine fallbackRoutine;

        public event Action<int> OnMilestoneShown;
        private Milestone currentMilestone;
        private bool hasBeenRequested = false;

        private bool specificSceneDone = false;
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

            if (fallbackRoutine != null)
                StopCoroutine(fallbackRoutine);

            fallbackRoutine = StartCoroutine(FallbackTimer());
        }

        private void ShowMilestone()
        {
            if (!hasBeenRequested) return;

            if (fallbackRoutine != null)
                StopCoroutine(fallbackRoutine);

            InterruptionManager.Instance.EnableInterruption(InterruptionType.NOTIFICATION);

            MilestoneDescriptionUI.SetText(string.Format(currentMilestone.description, GameData.Instance.GetPlayerLabel()));
            MilestoneCanvasUI.SetActive(true);

            InspectSystem.RequestMDOCreation(currentMilestone);

            MilestoneImageUI.sprite = MilestoneImages[currentMilestone.level - 1];
            MilestoneImageUI.gameObject.SetActive(true);

            MilestoneNext.gameObject.SetActive(false);
            
            SFXCaller.Play("event:/LevelUpGrande");
            Animator.SetTrigger("PresentNews");
        }

        // llamado por Animation Event al terminar PresentNews
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

        private IEnumerator FallbackTimer()
        {
            yield return new WaitForSeconds(fallbackDelay);

            if (hasBeenRequested)
            {
                Debug.Log("Fallback milestone triggered");
                ShowMilestone();
            }
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