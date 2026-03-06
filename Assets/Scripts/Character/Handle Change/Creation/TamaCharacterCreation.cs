using Game.Character.Visual;
using Game.Utils;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Character.Creation
{
    public class TamaCharacterCreation : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TamaCharacterVisual CharacterVisual;
        
        [Header("UI")]
        [SerializeField] private GameObject CanvasUI;
        [SerializeField] private Button PreviousButton;
        [SerializeField] private Button NextButton;
        [SerializeField] private TMP_InputField InputField;

        [Header("Message Box")]
        [SerializeField] private SimpleTypewriter Typewriter;

        [Header("Character Buttons")]
        [SerializeField] private List<Button> CharacterButtons;

        private int selectedCharacterIndex = -1;

        private enum CreationPhase
        {
            Heading,
            Naming,
            Finishing
        }

        private CreationPhase currentPhase;

        private void Awake()
        {
            PreviousButton.onClick.AddListener(GoBack);
            NextButton.onClick.AddListener(GoNext);

            for (int i = 0; i < CharacterButtons.Count; i++)
            {
                int index = i;
                CharacterButtons[i].onClick.AddListener(() => SelectCharacter(index));
            }

            StartCreation();
        }

        private void StartCreation()
        {
            CanvasUI.SetActive(true);
            ChangePhase(CreationPhase.Heading);
        }

        private void ChangePhase(CreationPhase phase)
        {
            currentPhase = phase;

            switch (phase)
            {
                case CreationPhase.Heading:
                    PreviousButton.gameObject.SetActive(false);
                    foreach (Button btn in CharacterButtons)
                    {
                        btn.gameObject.SetActive(true);
                    }
                    InputField.gameObject.SetActive(false);
                    Typewriter.ShowText("Selecciona al personaje");
                    break;

                case CreationPhase.Naming:

                    foreach (Button btn in CharacterButtons)
                    {
                        btn.gameObject.SetActive(false);
                    }
                    PreviousButton.gameObject.SetActive(true);
                    InputField.gameObject.SetActive(true);
                    InputField.text = string.Empty;
                    Typewriter.ShowText("Ingresa su nombre");
                    break;

                case CreationPhase.Finishing:
                    InputField.gameObject.SetActive(false);
                    Typewriter.ShowText($"¿Confirmar a {InputField.text}?");
                    break;
            }
        }

        private void SelectCharacter(int index)
        {
            selectedCharacterIndex = index;
        }

        private void GoNext()
        {
            switch (currentPhase)
            {
                case CreationPhase.Heading:
                    if (selectedCharacterIndex < 0) return;
                    ChangePhase(CreationPhase.Naming);
                    break;

                case CreationPhase.Naming:
                    if (string.IsNullOrWhiteSpace(InputField.text)) return;
                    ChangePhase(CreationPhase.Finishing);
                    break;

                case CreationPhase.Finishing:
                    ConfirmCreation();
                    break;
            }
        }

        private void GoBack()
        {
            switch (currentPhase)
            {
                case CreationPhase.Naming:
                    ChangePhase(CreationPhase.Heading);
                    break;

                case CreationPhase.Finishing:
                    ChangePhase(CreationPhase.Naming);
                    break;
            }
        }

        private void ConfirmCreation()
        {
            Debug.Log($"Character Created of type: {selectedCharacterIndex}, and name: {InputField.text}");
            //CharacterVisual.RequestVisualEvolution((TamaType)selectedCharacterIndex, 0);
            CanvasUI.SetActive(false);
        }
    }
}