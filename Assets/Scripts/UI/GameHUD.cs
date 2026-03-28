using Game.Character;
using Game.Managers.Timing;
using Game.Systems.Interaction;
using Game.Systems.Milestone;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    [System.Serializable]
    public class StatUIObject
    {
        public StatRadialBarUI StatBar;
        public ClickableObject ClickObject;

        public void UpdateUIState(bool state)
        {
            StatBar.gameObject.SetActive(state);
            ClickObject.IsInteractable = state;
            ClickObject.gameObject.SetActive(state);
        }
    }   

    public class GameHUD : MonoBehaviour, IInterruptible
    {
        [Header("Stat Bars")]
        [SerializeField] private StatUIObject CharismaUIO;
        [SerializeField] private StatUIObject WisdomUIO;
        [SerializeField] private StatUIObject WillpowerUIO;

        [Header("Info")]
        [SerializeField] private TMP_Text PlayerLabel;

        [Header("References")]
        [SerializeField] private TamaCharacterStats characterStats;
        [SerializeField] private MilestonePresenter MilestonePresenter;

        private const string baseName = "Tiko";

        private void Awake()
        {
            SetupUIStats();
        }

        private void Start()
        {
            RefreshBars();
            SetPlayerLabel(0);
        }

        private void SetupUIStats()
        {
            CharismaUIO.ClickObject.gameObject.transform.parent = CharismaUIO.StatBar.gameObject.transform;
            WisdomUIO.ClickObject.gameObject.transform.parent = WisdomUIO.StatBar.gameObject.transform;
            WillpowerUIO.ClickObject.gameObject.transform.parent = WillpowerUIO.StatBar.gameObject.transform;
        }

        public void SetPlayerLabel(int level)
        {
            string name = GameData.Instance != null ? GameData.Instance.PlayerName : baseName;
            PlayerLabel.text = $"{name} - NIVEL {level}";
        }

        private void RefreshBars()
        {
            if (characterStats == null) return;

            CharismaUIO?.StatBar.UpdateBar(characterStats.Charisma);
            WisdomUIO?.StatBar.UpdateBar(characterStats.Wisdom);
            WillpowerUIO?.StatBar.UpdateBar(characterStats.WillPower);
        }

        private void NextLevel(int level)
        {
            ResetBars(level);
            SetPlayerLabel(level);
        }

        private void ResetBars(int level)
        {
            CharismaUIO?.StatBar.ResetBar();
            WisdomUIO?.StatBar.ResetBar();
            WillpowerUIO?.StatBar.ResetBar();
        }
        
        public void HandleInterruptionStart(InterruptionType type)
        {
            switch (type)
            {
                case InterruptionType.TRANSITION:
                    CharismaUIO.ClickObject.IsInteractable = false;
                    WisdomUIO.ClickObject.IsInteractable = false;
                    WillpowerUIO.ClickObject.IsInteractable = false;
                    break;
                case (InterruptionType.CINEMATIC or InterruptionType.NOTIFICATION):
                    CharismaUIO.UpdateUIState(false);
                    WisdomUIO.UpdateUIState(false);
                    WillpowerUIO.UpdateUIState(false);
                    break;

            }
        }

        public void HandleInterruptionEnd()
        {
            CharismaUIO.UpdateUIState(true);
            WisdomUIO.UpdateUIState(true);
            WillpowerUIO.UpdateUIState(true);
        }

        private void OnEnable()
        {
            if (characterStats != null)
            {
                characterStats.OnStatsChanged += RefreshBars;
                MilestonePresenter.OnMilestoneShown += NextLevel;
            }

            InterruptionManager.OnInterruptStart += HandleInterruptionStart;
            InterruptionManager.OnInterruptEnd += HandleInterruptionEnd;
        }

        private void OnDisable()
        {
            if (characterStats != null)
            {
                characterStats.OnStatsChanged -= RefreshBars;
                MilestonePresenter.OnMilestoneShown -= NextLevel;
            }

            InterruptionManager.OnInterruptStart -= HandleInterruptionStart;
            InterruptionManager.OnInterruptEnd -= HandleInterruptionEnd;
        }
    }
}