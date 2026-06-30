using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class LanguageButtonController : MonoBehaviour
    {
        [Serializable]
        private class LanguageButton
        {
            public GameData.Language SelectedLanguage;
            public SpriteState State;
        }
        
        [Header("Parameters")]
        [SerializeField] private List<LanguageButton> ButtonLanguages;

        private Button Button;
        private LanguageButton currentButton;
        private int currentIndex;

        void Start()
        {
            Button = GetComponent<Button>();
            currentButton = ButtonLanguages[0];

            SelectButton(currentButton);

            Button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            if (currentIndex >= ButtonLanguages.Count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }

            currentButton = ButtonLanguages[currentIndex];
            SelectButton(currentButton);

            SFXCaller.Play("event:/uiButton");
        }

        private void SelectButton(LanguageButton but)
        {
            Button.GetComponent<Image>().sprite = currentButton.State.highlightedSprite;
            Button.spriteState = currentButton.State;
            GameData.Instance.ChangeLanguage(currentButton.SelectedLanguage);
        }

        private void OnDisable()
        {
            Button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnDestroy()
        {
            Button.onClick.RemoveListener(OnButtonClicked);
        }
    }
}