using Febucci.UI;
using Game.Managers.Timing;
using Game.Systems.Interaction.Detail;
using Game.Utils;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Game.Systems.Achievement
{
    [Serializable]
    public class TextStep
    {
        public TypewriterByCharacter text;
        public LocalizedString template;
        public bool needAfterDeactivation;
        public bool objectPresentation;
        public bool ignoreTemplate;
        public float textDuration;
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

        [Header("Data")]
        [SerializeField] private SpriteAtlas SpriteAtlas;

        [Header("Parameters")]
        [SerializeField] private float NotificationDuration = 3f;

        private Achievement lastAchievement;

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

            lastAchievement = achievement;

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

                NotificationTitles[i].text.ShowText(string.Format(NotificationTitles[i].template.GetLocalizedString(), achievement.GetStatToTitleCase()));

                yield return new WaitUntil(() => finished);

                NotificationTitles[i].text.onTextShowed.RemoveListener(OnFinished);

                yield return new WaitForSeconds(NotificationTitles[i].textDuration);
            }

            // DESCRIPTION

            for (int i = 0; i < NotificationDescriptions.Length; i++)
            {
                bool descFinished = false;

                void OnDescFinished() => descFinished = true;

                NotificationDescriptions[i].text.onTextShowed.AddListener(OnDescFinished);

                if (!NotificationDescriptions[i].ignoreTemplate && !NotificationDescriptions[i].objectPresentation)
                {
                    NotificationDescriptions[i].text.ShowText(string.Format(NotificationDescriptions[i].template.GetLocalizedString(), GameData.Instance.GetPlayerName()));
                }
                else if (NotificationDescriptions[i].ignoreTemplate && !NotificationDescriptions[i].objectPresentation)
                {
                    NotificationDescriptions[i].text.ShowText(string.Format(achievement.localizedDescription.GetLocalizedString(), GameData.Instance.GetPlayerName()));
                }

                if (NotificationDescriptions[i].objectPresentation)
                {
                    NotificationDescriptions[i].text.ShowText(string.Format(NotificationDescriptions[i].template.GetLocalizedString(), achievement.localizedObjectName.GetLocalizedString()));
                    
                    if (achievement.spriteLocalizationNeeded)
                    {
                        NotificationImage.sprite = SpriteAtlasHandling.GetLocalizedSprite(SpriteAtlas, achievement.spriteAtlasID);
                    }
                    else
                    {
                        NotificationImage.sprite = SpriteAtlasHandling.GetSpriteFromAtlas(SpriteAtlas, achievement.spriteAtlasID);
                    }

                    NotificationImage.gameObject.SetActive(true);
                }

                yield return new WaitUntil(() => descFinished);

                NotificationDescriptions[i].text.onTextShowed.RemoveListener(OnDescFinished);

                float duration = NotificationDescriptions[i].textDuration;

                yield return new WaitForSeconds(duration);

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

            DetailSystem.Instance.OnObjectCreated -= NotifyAchievementHided;
            DetailSystem.Instance.OnObjectCreated += NotifyAchievementHided;

            DetailSystem.Instance.RequestDetailObjCreation(lastAchievement.detailObjectID, lastAchievement.spawnPosition, lastAchievement.spawnRotation, lastAchievement.spawnScale);
        }

        private void NotifyAchievementHided()
        {
            DetailSystem.Instance.OnObjectCreated -= NotifyAchievementHided;
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