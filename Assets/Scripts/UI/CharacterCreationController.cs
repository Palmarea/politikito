using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using DG.Tweening;

namespace Game.UI
{
    public class CharacterCreationController : MonoBehaviour
    {
        [Header("Name Input")]
        [SerializeField] private TMP_InputField nameInput;

        [Header("Navigation Buttons")]
        [SerializeField] private Button confirmButton;

        [Header("Transition")]
        [SerializeField] private string nextSceneName = "MainScene";

        [Header("Feedback")]
        [SerializeField] private TMP_Text feedbackText;

        private Coroutine feedbackCoroutine;

        private void Start()
        {
            if (confirmButton != null)
                confirmButton.onClick.AddListener(ConfirmCharacter);

            if (feedbackText != null)
                feedbackText.gameObject.SetActive(false);
        }

        private void ConfirmCharacter()
        {
            // Check if name is empty
            if (nameInput != null && string.IsNullOrEmpty(nameInput.text))
            {
                nameInput.GetComponent<RectTransform>().DOShakeAnchorPos(0.5f, 20);
                ShowFeedback("Escoge un nombre para tu TIKO!");
                return;
            }

            GameData.Instance.PlayerName = nameInput != null ? nameInput.text : "TIKO";
            SceneManager.LoadScene(nextSceneName);
        }

        private void ShowFeedback(string message)
        {
            if (feedbackText == null) return;

            feedbackText.text = message;
            feedbackText.gameObject.SetActive(true);

            if (feedbackCoroutine != null)
                StopCoroutine(feedbackCoroutine);
            feedbackCoroutine = StartCoroutine(HideFeedbackAfterDelay(3f));
        }

        private IEnumerator HideFeedbackAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (feedbackText != null)
                feedbackText.gameObject.SetActive(false);
        }
    }
}