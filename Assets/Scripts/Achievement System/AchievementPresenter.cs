using Febucci.UI;
using Game.Managers.Timing;
using Game.Systems.Interaction.Detail;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems.Achievement
{
    [Serializable]
    public class TextStep
    {
        public TypewriterByCharacter text;
        [TextArea] public string template;
        public bool needAfterDeactivation;
        public bool objectPresentation;
    }

    public class AchievementPresenter : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private AchievementSystem MainSystem;

        [Header("References")]
        [SerializeField] private GameObject NotificationCanvasUI;
        [SerializeField] private TextStep[] NotificationTitles;
        [SerializeField] private Image NotificationImage;
        [SerializeField] private TextStep[] NotificationDescriptions;

        [Header("Parameters")]
        [SerializeField] private float NotificationDuration = 3f;
        [SerializeField] private float TextDuration = 2f;

        public event Action OnAchievementNotificationHided;

        private Coroutine presentationRoutine;

        private void Awake()
        {
            if (MainSystem == null) MainSystem = GetComponent<AchievementSystem>();

            NotificationCanvasUI.SetActive(false);
            NotificationImage.gameObject.SetActive(false);
        }

        private void ShowNotification(Achievement achievement)
        {
            if (!NotificationCanvasUI.activeInHierarchy)
                NotificationCanvasUI.SetActive(true);

            //SFX
            SFXCaller.Play("event:/LevelUpSmall");

            // Si ya hay una coroutine corriendo, la cancelamos
            if (presentationRoutine != null)
                StopCoroutine(presentationRoutine);

            DetailSystem.Instance.RequestDetailObjCreation(achievement.detailObjectID);

            presentationRoutine = StartCoroutine(ShowSequence(achievement));

            InterruptionManager.Instance.EnableInterruption(InterruptionType.NOTIFICATION);
        }

        private IEnumerator ShowSequence(Achievement achievement)
        {
            for (int i = 0; i < NotificationTitles.Length; i++)
            {
                bool finished = false;

                void OnFinished() => finished = true;

                NotificationTitles[i].text.onTextShowed.AddListener(OnFinished);

                NotificationTitles[i].text.ShowText(string.Format(NotificationTitles[i].template, achievement.stat));

                yield return new WaitUntil(() => finished);

                NotificationTitles[i].text.onTextShowed.RemoveListener(OnFinished);

                yield return new WaitForSeconds(TextDuration);
            }

            // DESCRIPTION

            for (int i = 0; i < NotificationDescriptions.Length; i++)
            {
                bool descFinished = false;

                void OnDescFinished() => descFinished = true;

                NotificationDescriptions[i].text.onTextShowed.AddListener(OnDescFinished);

                NotificationDescriptions[i].text.ShowText(string.Format(NotificationDescriptions[i].template, GameData.Instance.GetPlayerLabel().ToUpper()));

                if (NotificationDescriptions[i].objectPresentation)
                {
                    NotificationImage.gameObject.SetActive(true);
                }

                yield return new WaitUntil(() => descFinished);

                NotificationDescriptions[i].text.onTextShowed.RemoveListener(OnDescFinished);

                yield return new WaitForSeconds(TextDuration);

                if (NotificationDescriptions[i].needAfterDeactivation)
                {
                    NotificationDescriptions[i].text.ShowText("");
                }
            }

            // WAIT FINAL
            yield return new WaitForSeconds(NotificationDuration);

            HideNotification();
        }

        private void HideNotification()
        {
            if (NotificationCanvasUI.activeInHierarchy)
                NotificationCanvasUI.SetActive(false);

            foreach (var textStep in NotificationTitles)
                textStep.text.ShowText("");

            foreach (var textStep in NotificationDescriptions)
                textStep.text.ShowText("");

            NotificationImage.gameObject.SetActive(false);

            InterruptionManager.Instance.DisableInteruption();
            OnAchievementNotificationHided?.Invoke();
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