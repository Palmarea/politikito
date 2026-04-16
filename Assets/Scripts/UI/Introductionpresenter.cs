using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Game.Managers.Timing;

namespace Game.UI
{
    public class IntroductionPresenter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject IntroductionPanel;
        [SerializeField] private TMP_Text IntroductionText;
        [SerializeField] private Button CloseButton;

        [Header("Parameters")]
        [SerializeField][TextArea(4, 10)] private string IntroductionContent =
            "Conoce a {nombre}\n\n" +
            "{nombre} es una joven promesa y futuro joven líder.\n\n" +
            "Pero antes, necesita estar preparado para hacer escuchar su voz.\n\n" +
            "Toma los siguientes objetos para cuidar de {nombre}";

        [SerializeField] private float FadeInDuration = 0.5f;

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = IntroductionPanel.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
                canvasGroup = IntroductionPanel.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
            IntroductionPanel.SetActive(true);
            CloseButton.onClick.AddListener(CloseIntroduction);
        }

        private void Start()
        {
            string playerName = GameData.Instance != null ? GameData.Instance.PlayerName : "tu personaje";
            string content = IntroductionContent.Replace("{nombre}", playerName);
            IntroductionText.text = content;

            InterruptionManager.Instance.EnableInterruption(InterruptionType.CINEMATIC);
            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            float elapsed = 0f;

            while (elapsed < FadeInDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Clamp01(elapsed / FadeInDuration);
                yield return null;
            }

            canvasGroup.alpha = 1f;
        }

        private void CloseIntroduction()
        {
            IntroductionPanel.SetActive(false);
            InterruptionManager.Instance.DisableInteruption();
        }
    }
}