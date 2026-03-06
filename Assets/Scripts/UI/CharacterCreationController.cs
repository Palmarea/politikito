using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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

        [Header("Optional")]
        [SerializeField] private TMP_Text outfitLabel;

        // Static data so other scenes can access it
        public static string PlayerName { get; private set; }
        public static int SelectedOutfit { get; private set; }

        private void Start()
        {
            if (leftArrow != null)
                leftArrow.onClick.AddListener(PreviousOutfit);
            if (rightArrow != null)
                rightArrow.onClick.AddListener(NextOutfit);
            if (confirmButton != null)
                confirmButton.onClick.AddListener(ConfirmCharacter);

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
            }

            if (outfitLabel != null)
            {
                outfitLabel.text = "Outfit " + (currentOutfitIndex + 1) + "/" + outfitSprites.Length;
            }
        }

        private void ConfirmCharacter()
        {
            if (nameInput != null && string.IsNullOrEmpty(nameInput.text))
            {
                // Flash the input or show error
                if (nameInput.placeholder is TMP_Text placeholder)
                    placeholder.text = "Write a name!";
                return;
            }

            PlayerName = nameInput != null ? nameInput.text : "Politiko";
            SelectedOutfit = currentOutfitIndex;

            SceneManager.LoadScene(nextSceneName);
        }
    }
}