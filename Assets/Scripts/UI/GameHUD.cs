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

        private void Start()
        {
            RefreshBars();
            SetPlayerLabel(0);
        }

        public void SetPlayerLabel(int level)
        {
            PlayerLabel.text = $"{GameData.Instance.PlayerName} - NIVEL {level}";
        }

        private void RefreshBars()
        {
            if (characterStats == null) return;

            CharismaBar?.UpdateBar(characterStats.Charisma);
            WisdomBar?.UpdateBar(characterStats.Wisdom);
            WillpowerBar?.UpdateBar(characterStats.WillPower);
        }

        private void ResetBars(int level)
        {
            CharismaBar?.ResetBar();
            WisdomBar?.ResetBar();
            WillpowerBar?.ResetBar();

            SetPlayerLabel(level);
        }

        private void OnEnable()
        {
            if (characterStats != null)
            {
                characterStats.OnStatsChanged += RefreshBars;
                MilestonePresenter.OnMilestoneShown += ResetBars;
            }
        }

        private void OnDisable()
        {
            if (characterStats != null)
            {
                characterStats.OnStatsChanged -= RefreshBars;
                MilestonePresenter.OnMilestoneShown -= ResetBars;
            }
        }
    }
}