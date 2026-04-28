using Game.Character;
using Game.Managers.Timing;
using Game.Systems.Achievement;
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
        public SpriteRenderer ObjectSprite;
        public CanvasGroup CanvasGroup;
        public Transform AnchorPoint;

        private Color m_normalColor;
        private Color m_hidedColor;
        private readonly Color m_cooldownColor = new(0.4f, 0.4f, 0.4f, 1f);

        public void SetupObject()
        {
            m_normalColor = ObjectSprite.color;
            m_hidedColor = new Color(m_normalColor.r, m_normalColor.g, m_normalColor.r, 0f);
        } 

        public void UpdateUIState(bool state)
        {            
            CanvasGroup.alpha = state ? 1 : 0;
            UpdateInteraction(state);
            UpdateSpriteState(state);
        }

        public void UpdateInteraction(bool state)
        {
            ClickObject.IsInteractable = state;
        }

        private void UpdateSpriteState(bool state)
        {
            ObjectSprite.color = state ? m_normalColor : m_hidedColor;
        }

        public void UpdateSpriteCooldown(bool state)
        {
            if (ObjectSprite.color == m_hidedColor) return;

            ObjectSprite.color = state ? m_cooldownColor : m_normalColor;
        }
    }   

    public class GameHUD : MonoBehaviour, IInterruptible
    {
        [Header("Stat Bars")]
        [SerializeField] private StatUIObject CharismaUIO = new();
        [SerializeField] private StatUIObject WisdomUIO = new();
        [SerializeField] private StatUIObject WillpowerUIO = new();

        [Header("Info")]
        [SerializeField] private TMP_Text PlayerLabel;

        [Header("References")]
        [SerializeField] private TamaCharacterStats characterStats;
        [SerializeField] private MilestonePresenter MilestonePresenter;

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
            CharismaUIO.ClickObject.gameObject.transform.parent = CharismaUIO.AnchorPoint;
            CharismaUIO.SetupObject();

            WisdomUIO.ClickObject.gameObject.transform.parent = WisdomUIO.AnchorPoint;
            WisdomUIO.SetupObject();
            
            WillpowerUIO.ClickObject.gameObject.transform.parent = WillpowerUIO.AnchorPoint;
            WillpowerUIO.SetupObject();
        }

        public void SetPlayerLabel(int level)
        {
            PlayerLabel.text = $"{GameData.Instance.PlayerName} - NIVEL {level}";
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
            ResetBars();
            SetPlayerLabel(level);
        }

        public void RequesStatBarObjectsCooldown(StatType type, bool state)
        {
            switch (type)
            {
                case StatType.Carisma:
                    CharismaUIO.UpdateSpriteCooldown(state);
                    break;
                case StatType.Sabiduria:
                    WisdomUIO.UpdateSpriteCooldown(state);
                    break;
                case StatType.Voluntad:
                    WillpowerUIO.UpdateSpriteCooldown(state);
                    break;
            }
        }

        private void ResetBars()
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
                    CharismaUIO.UpdateInteraction(false);
                    WisdomUIO.UpdateInteraction(false);
                    WillpowerUIO.UpdateInteraction(false);
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