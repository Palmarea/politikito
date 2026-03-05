using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Character;

namespace Game.UI
{
    public class GameHUD : MonoBehaviour
    {
        [Header("Stat Bars")]
        [SerializeField] private StatBarUI charismaBar;
        [SerializeField] private StatBarUI knowledgeBar;
        [SerializeField] private StatBarUI determinationBar;

        [Header("Info")]
        [SerializeField] private TMP_Text dayText;

        [Header("Action Buttons")]
        [SerializeField] private Button waterButton;
        [SerializeField] private Button cleanButton;
        [SerializeField] private Button inspireButton;
        [SerializeField] private TMP_Text waterCooldownText;
        [SerializeField] private TMP_Text cleanCooldownText;
        [SerializeField] private TMP_Text inspireCooldownText;

        [Header("Referencia")]
        [SerializeField] private TamaCharacterStats characterStats;

        private void Start()
        {
            if (waterButton != null)
                waterButton.onClick.AddListener(() => characterStats?.Water());
            if (cleanButton != null)
                cleanButton.onClick.AddListener(() => characterStats?.CleanCorruption());
            if (inspireButton != null)
                inspireButton.onClick.AddListener(() => characterStats?.Inspire());

            if (characterStats != null)
                characterStats.OnStatsChanged += UpdateBars;
        }

        private void Update()
        {
            if (characterStats == null) return;
            UpdateCooldown(waterButton, waterCooldownText,
                characterStats.CanWater, characterStats.WaterCooldownRemaining);
            UpdateCooldown(cleanButton, cleanCooldownText,
                characterStats.CanClean, characterStats.CleanCooldownRemaining);
            UpdateCooldown(inspireButton, inspireCooldownText,
                characterStats.CanInspire, characterStats.InspireCooldownRemaining);
        }

        private void UpdateBars(TamaStat charisma, TamaStat knowledge, TamaStat determination)
        {
            if (charismaBar != null) charismaBar.UpdateBar(charisma);
            if (knowledgeBar != null) knowledgeBar.UpdateBar(knowledge);
            if (determinationBar != null) determinationBar.UpdateBar(determination);
        }

        private void UpdateCooldown(Button btn, TMP_Text text, bool canUse, float remaining)
        {
            if (btn != null) btn.interactable = canUse;
            if (text != null)
                text.text = canUse ? "" : Mathf.CeilToInt(remaining) + "s";
        }

        public void SetDay(int day)
        {
            if (dayText != null)
                dayText.text = "Dia " + day;
        }

        private void OnDestroy()
        {
            if (characterStats != null)
                characterStats.OnStatsChanged -= UpdateBars;
        }
    }
}