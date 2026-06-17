using DG.Tweening;
using MaskTransitions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        [Header("Brush Effect")]
        [SerializeField] private List<TMPBrushController> BrushTextList;
        [SerializeField] private List<UIImageBrushController> BrushImagesList;

        private Coroutine feedbackCoroutine;

        private void Start()
        {
            if (confirmButton != null)
                confirmButton.onClick.AddListener(ConfirmCharacter);

            if (feedbackText != null)
                feedbackText.gameObject.SetActive(false);

            confirmButton.enabled = false;
            nameInput.enabled = false;

            foreach (var b in BrushTextList)
            {
                b.HideInstant();
            }

            foreach (var b in BrushImagesList)
            {
                b.HideInstant();
            }

            StartCoroutine(OnShow());
        }

        private IEnumerator OnShow()
        {
            foreach (var b in BrushTextList)
            {
                b.Show();
            }

            foreach (var b in BrushImagesList)
            {
                b.Show();
            }

            yield return new WaitForSeconds(1f);

            confirmButton.enabled = true;
            nameInput.enabled = true;
        }

        private void ConfirmCharacter()
        {
            // Check if name is empty
            if (nameInput != null && string.IsNullOrEmpty(nameInput.text))
            {
                nameInput.GetComponent<RectTransform>().DOShakeAnchorPos(0.5f, 20);
                ShowFeedback();
                return;
            }

            if (GameData.Instance != null) GameData.Instance.PlayerName = nameInput != null ? nameInput.text : "TIKO";
            TransitionManager.Instance.LoadLevel(nextSceneName);
        }

        private void ShowFeedback()
        {
            if (feedbackText == null) return;

            feedbackText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Character Creation Screen", "charCreationScreen.nameLog");
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