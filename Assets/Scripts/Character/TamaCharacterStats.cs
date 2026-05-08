using UnityEngine;
using System;
using Game.Systems.Minigames;

namespace Game.Character
{
    [System.Serializable]
    public class TamaStat
    {
        public string Name;
        public int Level;
        public float Value;
        public float MinValue = 0;
        public float MaxValue = 100;
        public float DecayPerSecond = 0.3f;

        public void AddToValue(float amount) => Value = Mathf.Clamp(Value + amount, MinValue, MaxValue);
    }

    [RequireComponent(typeof(TamaCharacterController))]
    public class TamaCharacterStats : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField]
        private TamaStat CharismaStat = new TamaStat
        {
            Name = "Charisma",
            Level = 0,
            Value = 0,
            DecayPerSecond = 0.3f
        };
        [SerializeField]
        private TamaStat WisdomStat = new TamaStat
        {
            Name = "Wisdom",
            Level = 0,
            Value = 0,
            DecayPerSecond = 0.25f
        };
        [SerializeField]
        private TamaStat WillPowerStat = new TamaStat
        {
            Name = "Willpower",
            Level = 0,
            Value = 0,
            DecayPerSecond = 0.35f
        };

        [Header("Player Actions")]
        [SerializeField] private DifficultyValue WateringPointsPerLevel;
        [SerializeField] private DifficultyValue FeedingPointsPerLevel;
        [SerializeField] private DifficultyValue ExercisingPointsPerLevel;

        // Events
        public event Action OnStatsChanged;
        public event Action<TamaStat> OnStatLevelUp;
        public event Action<int> OnAllStatsReachedSameLevel;

        // Properties
        public TamaStat Charisma => CharismaStat;
        public TamaStat Wisdom => WisdomStat;
        public TamaStat WillPower => WillPowerStat;

        private readonly float[] levelThresholds = { 25.0f, 50.0f, 75.0f, 100.0f };

        public void HandleWateringAction()
        {
            ApplyAction(CharismaStat, WateringPointsPerLevel);
        }

        public void HandleFeedingAction()
        {
            ApplyAction(WisdomStat, FeedingPointsPerLevel);
        }

        public void HandleExercisingAction()
        {
            ApplyAction(WillPowerStat, ExercisingPointsPerLevel);
        }

        private void ApplyAction(TamaStat stat, DifficultyValue pointsPerLevel)
        {
            if (!CanAttemptLevelUp(stat))
                return;

            stat.AddToValue(pointsPerLevel.GetValue(stat.Level));
            CheckForLevelUp(stat);
            OnStatsChanged?.Invoke();
        }

        private bool CanAttemptLevelUp(TamaStat stat)
        {
            if (stat.Level >= levelThresholds.Length)
                return false;

            int nextLevel = stat.Level + 1;

            foreach (var other in new[] { CharismaStat, WisdomStat, WillPowerStat })
            {
                if (other == stat) continue;

                if (other.Level < nextLevel - 1)
                    return false;
            }

            return true;
        }

        private void CheckForLevelUp(TamaStat stat)
        {
            if (stat.Level >= levelThresholds.Length) return;

            if (stat.Value >= levelThresholds[stat.Level])
            {
                stat.Level++;
                OnStatLevelUp?.Invoke(stat);

                CheckIfAllStatsSameLevel();
            }
        }

        private void CheckIfAllStatsSameLevel()
        {
            if (CharismaStat.Level == WisdomStat.Level && WisdomStat.Level == WillPowerStat.Level)
            {
                OnAllStatsReachedSameLevel?.Invoke(CharismaStat.Level);
            }
        }
    }
}