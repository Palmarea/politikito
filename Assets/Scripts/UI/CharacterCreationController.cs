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
        [Header("Character Display")]
        [SerializeField] private Image characterPreview;
        [SerializeField] private Sprite[] outfitSprites;
        private int currentOutfitIndex = 0;

        [Header("Name Input")]
        [SerializeField] private TMP_InputField nameInput;

        [Header("Navigation Buttons")]
        [SerializeField] private Button leftArrow;
        [SerializeField] private Button rightArrow;
        [SerializeField] private Button confirmButton;

        [Header("Transition")]
        [SerializeField] private string nextSceneName = "MainScene";

        [Header("Locked Outfits")]
        [Tooltip("Which outfit indexes are available (0 = first outfit)")]
        [SerializeField] private int[] unlockedOutfits = { 0 };

        [Header("Feedback")]
        [SerializeField] private TMP_Text feedbackText;

        [Header("Optional")]
        [SerializeField] private TMP_Text outfitLabel;

        // Static data so other scenes can access it
        public static string PlayerName { get; private set; }
        public static int SelectedOutfit { get; private set; }

        private Coroutine feedbackCoroutine;

        private void Start()
        {
            if (leftArrow != null)
                leftArrow.onClick.AddListener(PreviousOutfit);
            if (rightArrow != null)
                rightArrow.onClick.AddListener(NextOutfit);
            if (confirmButton != null)
                confirmButton.onClick.AddListener(ConfirmCharacter);

            if (feedbackText != null)
                feedbackText.gameObject.SetActive(false);

            UpdateCharacterDisplay();
        }

        private void NextOutfit()
        {
            if (outfitSprites == null || outfitSprites.Length == 0) return;
            currentOutfitIndex = (currentOutfitIndex + 1) % outfitSprites.Length;
            UpdateCharacterDisplay();
        }

        private void PreviousOutfit()
        {
            if (outfitSprites == null || outfitSprites.Length == 0) return;
            currentOutfitIndex--;
            if (currentOutfitIndex < 0) currentOutfitIndex = outfitSprites.Length - 1;
            UpdateCharacterDisplay();
        }

        private void UpdateCharacterDisplay()
        {
            if (characterPreview != null && outfitSprites != null && outfitSprites.Length > 0)
            {
                characterPreview.sprite = outfitSprites[currentOutfitIndex];
                characterPreview.preserveAspect = true;
            }

            if (outfitLabel != null)
            {
                outfitLabel.text = "Outfit " + (currentOutfitIndex + 1) + "/" + outfitSprites.Length;
            }

            // Hide feedback when changing outfit
            if (feedbackText != null)
                feedbackText.gameObject.SetActive(false);
        }

        private bool IsOutfitUnlocked(int index)
        {
            foreach (int unlocked in unlockedOutfits)
            {
                if (unlocked == index) return true;
            }
            return false;
        }

        private void ConfirmCharacter()
        {
            // Check if outfit is locked
            if (!IsOutfitUnlocked(currentOutfitIndex))
            {
                ShowFeedback("Personaje bloqueado. Próximamente.");
                characterPreview.GetComponent<RectTransform>().DOShakeAnchorPos(0.5f, 20);
                return;
            }

            // Check if name is empty
            if (nameInput != null && string.IsNullOrEmpty(nameInput.text))
            {
                nameInput.GetComponent<RectTransform>().DOShakeAnchorPos(0.5f, 20);
                ShowFeedback("Escoge un nombre para tu TIKO!");
                return;
            }

            PlayerName = nameInput != null ? nameInput.text : "Politiko";
            SelectedOutfit = currentOutfitIndex;
            GameData.Instance.PlayerName = PlayerName;
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