using Game.Managers.Mouse;
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

            if (level < 0)
                level = 0;

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

        protected float currentProgress = 0f;
        protected bool isActive = false;

        public event Action OnMinigameClosed;
        public event Action OnMinigameCompleted;

        protected virtual void Awake()
        {
            CloseButton.onClick.AddListener(CloseMinigame);
            MinigameUI.SetActive(false);
        }

        protected virtual void Update()
        {
            if (!isActive) return;
            UpdateMinigame();
        }

        public virtual void StartMinigame()
        {
            currentProgress = 0f;
            UpdateProgressUI();

            isActive = true;
            MinigameUI.SetActive(true);

            //MouseManager.Instance.UpdateOcuppiedState(true);
        }

        protected abstract void UpdateMinigame();

        public virtual void CloseMinigame()
        {
            if (!isActive) return;

            isActive = false;
            MinigameUI.SetActive(false);

            //MouseManager.Instance.UpdateOcuppiedState(false);

            OnMinigameClosed?.Invoke();
        }

        protected void AddProgress(float amount)
        {
            if (!isActive) return;

            currentProgress += amount;
            currentProgress = Mathf.Clamp(currentProgress, 0f, 100f);

            UpdateProgressUI();

            if (currentProgress >= 100f)
            {
                CompleteMinigame();
            }
        }

        private void UpdateProgressUI()
        {
            if (ProgressBar != null)
                ProgressBar.value = currentProgress / 100f;
        }

        private void CompleteMinigame()
        {
            isActive = false;

            MinigameUI.SetActive(false);
            MouseManager.Instance.UpdateOcuppiedState(false);

            OnMinigameCompleted?.Invoke();

            // Lo que pase después es responsabilidad del minijuego concreto
            OnCompleted();
        }

        // Cada minijuego define qué pasa al completarse
        protected abstract void OnCompleted();
    }
}
