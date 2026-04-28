using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Character;

namespace Game.Events
{
    public class EventPopupUI : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject popupPanel;

        [Header("Contenido")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;

        [Header("Botones de Opciones (maximo 3)")]
        [SerializeField] private Button[] choiceButtons;
        [SerializeField] private TMP_Text[] choiceTexts;

        [Header("Resultado")]
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private Button continueButton;

        [Header("Referencia al Personaje")]
        [SerializeField] private TamaCharacterStats characterStats;

        private EventDataSO currentEvent;

        private void Start()
        {
            if (popupPanel != null)
                popupPanel.SetActive(false);
            if (resultPanel != null)
                resultPanel.SetActive(false);

            if (continueButton != null)
                continueButton.onClick.AddListener(CloseResult);
        }

        public void ShowEvent(EventDataSO eventData)
        {
            currentEvent = eventData;

            if (titleText != null)
                titleText.text = eventData.eventTitle;
            if (descriptionText != null)
                descriptionText.text = eventData.eventDescription;

            for (int i = 0; i < choiceButtons.Length; i++)
            {
                if (i < eventData.choices.Length)
                {
                    choiceButtons[i].gameObject.SetActive(true);
                    int choiceIndex = i;

                    if (i < choiceTexts.Length && choiceTexts[i] != null)
                        choiceTexts[i].text = eventData.choices[i].choiceText;

                    choiceButtons[i].onClick.RemoveAllListeners();
                    choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(choiceIndex));
                }
                else
                {
                    choiceButtons[i].gameObject.SetActive(false);
                }
            }

            if (resultPanel != null)
                resultPanel.SetActive(false);
            if (popupPanel != null)
                popupPanel.SetActive(true);
        }

        private void OnChoiceSelected(int index)
        {
            if (currentEvent == null || index >= currentEvent.choices.Length) return;

            EventChoice choice = currentEvent.choices[index];

            if (characterStats != null)
            {
                characterStats.ApplyEventEffects(
                    choice.charismaEffect,
                    choice.knowledgeEffect,
                    choice.determinationEffect
                );
            }

            if (resultPanel != null && resultText != null)
            {
                resultText.text = choice.resultMessage;
                resultPanel.SetActive(true);
            }

            foreach (var btn in choiceButtons)
                btn.gameObject.SetActive(false);
        }

        private void CloseResult()
        {
            if (popupPanel != null)
                popupPanel.SetActive(false);
            if (resultPanel != null)
                resultPanel.SetActive(false);

            if (EventManager.Instance != null)
                EventManager.Instance.OnEventResolved();
        }
    }
}