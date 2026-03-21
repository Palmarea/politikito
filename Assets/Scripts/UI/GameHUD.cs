using UnityEngine;
using TMPro;
using Game.Character;
using Game.Systems.Milestone;

namespace Game.UI
{
    public class GameHUD : MonoBehaviour
    {
        [Header("Stat Bars")]
        [SerializeField] private StatRadialBarUI CharismaBar;
        [SerializeField] private StatRadialBarUI WisdomBar;
        [SerializeField] private StatRadialBarUI WillpowerBar;

        [Header("Info")]
        [SerializeField] private TMP_Text PlayerLabel;

        [Header("References")]
        [SerializeField] private TamaCharacterStats characterStats;
        [SerializeField] private MilestonePresenter MilestonePresenter;

        private string baseName = "Tiko";

        private void Start()
        {
            RefreshBars();
            SetPlayerLabel(0);
        }

        public void SetPlayerLabel(int level)
        {
            string name = GameData.Instance != null ? GameData.Instance.PlayerName : baseName;
            PlayerLabel.text = $"{name} - NIVEL {level}";
        }

        private void RefreshBars()
        {
            if (characterStats == null) return;

            CharismaBar?.UpdateBar(characterStats.Charisma);
            WisdomBar?.UpdateBar(characterStats.Wisdom);
            WillpowerBar?.UpdateBar(characterStats.WillPower);
        }

        private void NextLevel(int level)
        {
            ResetBars(level);
            SetPlayerLabel(level);
        }

        private void ResetBars(int level)
        {
            CharismaBar?.ResetBar();
            WisdomBar?.ResetBar();
            WillpowerBar?.ResetBar();
        }

        private void OnEnable()
        {
            if (characterStats != null)
            {
                characterStats.OnStatsChanged += RefreshBars;
                MilestonePresenter.OnMilestoneShown += NextLevel;
            }
        }

        private void OnDisable()
        {
            if (characterStats != null)
            {
                characterStats.OnStatsChanged -= RefreshBars;
                MilestonePresenter.OnMilestoneShown -= NextLevel;
            }
        }
    }
}