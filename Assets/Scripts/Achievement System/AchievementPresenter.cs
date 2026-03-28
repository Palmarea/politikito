using Game.Managers.Timing;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.Systems.Achievement
{
    public class AchievementPresenter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AchievementSystem MainSystem;

        [Header("Dependencies")]
        [SerializeField] private GameObject NotificationCanvasUI;
        [SerializeField] private GameObject NotificationUI;
        [SerializeField] private TextMeshProUGUI NotificationTitle;
        [SerializeField] private TextMeshProUGUI NotificationDescription;

        [Header("Parameters")]
        [SerializeField] private float NotificationDuration = 3f;

        public event Action OnAchievementNotificationHided;

        private Coroutine hideRoutine;

        private void Awake()
        {
            if (MainSystem == null) MainSystem = GetComponent<AchievementSystem>();
            NotificationCanvasUI.SetActive(false);
        }

        private void ShowNotification(Achievement achievement)
        {
            if (!NotificationCanvasUI.activeInHierarchy)
                NotificationCanvasUI.SetActive(true);

            if (!NotificationUI.activeInHierarchy)
                NotificationUI.SetActive(true);

            InterruptionManager.Instance.EnableInterruption(InterruptionType.NOTIFICATION);

            NotificationTitle.text = achievement.title;
            NotificationDescription.text = achievement.description;

            //SFX
            SFXCaller.Play("event:/LevelUpSmall");

            // Si ya hay una coroutine corriendo, la cancelamos
            if (hideRoutine != null)
                StopCoroutine(hideRoutine);

            hideRoutine = StartCoroutine(HideAfterDelay());
        }

        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(NotificationDuration);
            HideNotification();
        }

        private void HideNotification()
        {
            if (NotificationCanvasUI.activeInHierarchy) NotificationCanvasUI.SetActive(false);
            if (NotificationUI.activeInHierarchy) NotificationUI.SetActive(false);

            NotificationTitle.text = "";
            NotificationDescription.text = "";

            OnAchievementNotificationHided?.Invoke();
            InterruptionManager.Instance.DisableInteruption();
        }

        private void OnEnable()
        {
            MainSystem.OnNextAchievement += ShowNotification;
        }

        private void OnDisable()
        {
            MainSystem.OnNextAchievement -= ShowNotification;
        }
    }
}