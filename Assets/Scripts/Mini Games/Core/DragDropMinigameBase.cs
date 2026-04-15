using Game.Character;
using Game.Managers.Mouse;
using Game.Systems.Interaction;
using Game.Systems.Interaction.DragNDrop;
using Game.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems.Minigames
{
    [System.Serializable]
    public class DifficultyValue
    {
        [Tooltip("Value per level. Index = character level")]
        [SerializeField] private float[] valuesPerLevel;

        public float GetValue(int level)
        {
            if (valuesPerLevel == null || valuesPerLevel.Length == 0)
                return 0f;
            if (level < 0) level = 0;
            if (level >= valuesPerLevel.Length)
                level = valuesPerLevel.Length - 1;
            return valuesPerLevel[level];
        }
    }

    public abstract class DragDropMinigameBase : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject MinigameUI;
        [SerializeField] private Slider ProgressBar;
        [SerializeField] private Button CloseButton;

        [Header("Dependencies")]
        [SerializeField] protected TamaCharacterStats CharacterStats;
        [SerializeField] protected DragDropReceiver Receiver;
        [SerializeField] protected DragDropObject DDObject;

        [Header("Cooldown Config")]
        [SerializeField] private float MinigameCooldownTime = 3.0f;

        [Header("Cooldown UI")]
        [SerializeField] private StatRadialBarUI CooldownRadialBar;

        [Header("Cooldown Visual")]
        [SerializeField] private SpriteRenderer ObjectSpriteRenderer;
        private static readonly Color GrayColor = new Color(0.4f, 0.4f, 0.4f, 1f);

        protected float currentProgress = 0f;
        protected float minigameCooldownTimer = 0f;
        protected bool isActive = false;
        protected bool isCooling = false;

        public event Action OnMinigameClosed;
        public event Action OnMinigameCompleted;

        protected virtual void Awake()
        {
            minigameCooldownTimer = MinigameCooldownTime;
            CloseButton.onClick.AddListener(CloseMinigame);
            MinigameUI.SetActive(false);
            Receiver.UpdateActive(false);
        }

        protected virtual void Update()
        {
            if (isCooling)
            {
                minigameCooldownTimer -= Time.deltaTime;
                if (minigameCooldownTimer < 0f)
                {
                    isCooling = false;
                    minigameCooldownTimer = MinigameCooldownTime;
                    OnCooldownFinished();
                }
                return;
            }
            if (!isActive) return;
            UpdateMinigame();
        }

        protected virtual bool CheckForMinigameStart()
        {
            if (isCooling) return false;
            if (!isActive)
            {
                isActive = true;
                currentProgress = 0f;
                UpdateProgressUI();
                MinigameUI.SetActive(true);
                MinigameManager.Instance.StartMinigame(this);
            }
            DDObject.StartDragging();
            return true;
        }

        public virtual void StartMinigame()
        {
            if (!CheckForMinigameStart()) return;
        }

        protected abstract void UpdateMinigame();

        public virtual void CloseMinigame()
        {
            if (!isActive || isCooling) return;
            isActive = false;
            MinigameUI.SetActive(false);
            Receiver.UpdateActive(false);
            MinigameManager.Instance.EndMinigame();
            OnMinigameClosed?.Invoke();
        }

        protected void AddProgress(float amount)
        {
            if (!isActive || isCooling) return;
            currentProgress += amount;
            currentProgress = Mathf.Clamp(currentProgress, 0f, 100f);
            UpdateProgressUI();
            if (currentProgress >= 100f)
                CompleteMinigame();
        }

        private void UpdateProgressUI()
        {
            if (ProgressBar != null)
                ProgressBar.value = currentProgress / 100f;
        }

        private void OnCooldownFinished()
        {
            MinigameUI.SetActive(false);
            if (CooldownRadialBar != null)
                CooldownRadialBar.EndCooldown();
            if (ObjectSpriteRenderer != null)
                ObjectSpriteRenderer.color = Color.white;
        }

        private void CompleteMinigame()
        {
            isActive = false;
            MinigameUI.SetActive(false);
            MouseManager.Instance.UpdateOcuppiedState(false);
            MinigameManager.Instance.EndMinigame();
            OnMinigameCompleted?.Invoke();
            Receiver.UpdateActive(false);
            OnCompleted();
            isCooling = true;
            if (ObjectSpriteRenderer != null)
                ObjectSpriteRenderer.color = GrayColor;
            if (CooldownRadialBar != null)
                CooldownRadialBar.StartCooldown(MinigameCooldownTime);
        }

        protected abstract void OnCompleted();
    }
}