using UnityEngine;
using TMPro;
using Game.Character;

namespace Game.UI
{
    public class GameHUD : MonoBehaviour
    {
        [Header("Stat Bars")]
        [SerializeField] private StatBarUI CharismaBar;
        [SerializeField] private StatBarUI WisdomBar;
        [SerializeField] private StatBarUI WillpowerBar;

        [Header("Info")]
        [SerializeField] private TMP_Text dayText;

        [Header("References")]
        [SerializeField] private TamaCharacterStats characterStats;

        private void Start()
        {
            RefreshBars();
        }

        public void SetDay(int day)
        {
            if (dayText != null)
                dayText.text = "Dia " + day;
        }

        private void RefreshBars()
        {
            if (characterStats == null) return;

            CharismaBar?.UpdateBar(characterStats.Charisma);
            WisdomBar?.UpdateBar(characterStats.Wisdom);
            WillpowerBar?.UpdateBar(characterStats.WillPower);
        }

        private void OnEnable()
        {
            if (characterStats != null)
                characterStats.OnStatsChanged += RefreshBars;
        }

        private void OnDisable()
        {
            if (characterStats != null)
                characterStats.OnStatsChanged -= RefreshBars;
        }
    }
}