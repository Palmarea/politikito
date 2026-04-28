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
        public void ReduceToValue(float amount) => Value = Mathf.Clamp(Value - amount, MinValue, MaxValue);
        public float Normalized => Value / MaxValue;
        public bool IsDepleted => Value <= MinValue;
    }

    [RequireComponent(typeof(TamaCharacterController))]
    public class TamaCharacterStats : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField]
        private TamaStat CharismaStat = new TamaStat
        {
            Name = "Carisma",
            Level = 0,
            Value = 0,
            DecayPerSecond = 0.3f
        };
        [SerializeField]
        private TamaStat WisdomStat = new TamaStat
        {
            Name = "Sabiduría",
            Level = 0,
            Value = 0,
            DecayPerSecond = 0.25f
        };
        [SerializeField]
        private TamaStat WillPowerStat = new TamaStat
        {
            Name = "Voluntad",
            Level = 0,
            Value = 0,
            DecayPerSecond = 0.35f
        };

        [Header("Player Actions")]
        [SerializeField] private DifficultyValue WateringPointsPerLevel;
        [SerializeField] private DifficultyValue FeedingPointsPerLevel;
        [SerializeField] private DifficultyValue ExercisingPointsPerLevel;

        //[Header("Cooldowns (seconds)")]
        //[SerializeField] private float waterCooldown = 5f;
        //[SerializeField] private float cleanCooldown = 4f;
        //[SerializeField] private float inspireCooldown = 6f;

        [Header("Decay")]
        [Tooltip("Uncheck to disable stat decay. Enable for cinematic ending.")]
        [SerializeField] private bool decayEnabled = false;

        //private float waterTimer = 0f;
        //private float cleanTimer = 0f;
        //private float inspireTimer = 0f;
        private float decayMultiplier = 1f;
        private bool isDead = false;

        // Events
        public event Action OnStatsChanged;
        public event Action<TamaStat> OnStatLevelUp;
        public event Action<int> OnAllStatsReachedSameLevel;
        public event Action<string> OnStatDepleted;

        // Properties
        public TamaStat Charisma => CharismaStat;
        public TamaStat Wisdom => WisdomStat;
        public TamaStat WillPower => WillPowerStat;
        public float AverageStats => (CharismaStat.Value + WisdomStat.Value + WillPowerStat.Value) / 3f;
        //public bool CanWater => waterTimer <= 0f;
        //public bool CanClean => cleanTimer <= 0f;
        //public bool CanInspire => inspireTimer <= 0f;
        //public float WaterCooldownRemaining => waterTimer;
        //public float CleanCooldownRemaining => cleanTimer;
        //public float InspireCooldownRemaining => inspireTimer;
        public bool DecayEnabled => decayEnabled;

        private float[] levelThresholds = { 25.0f, 50.0f, 75.0f, 100.0f };

        //private void Update()
        //{
        //    if (isDead) return;

        //    // Only decay if enabled
        //    if (decayEnabled)
        //    {
        //        float dt = Time.deltaTime * decayMultiplier;
        //        CharismaStat.ReduceToValue(CharismaStat.DecayPerSecond * dt);
        //        WisdomStat.ReduceToValue(WisdomStat.DecayPerSecond * dt);
        //        WillPowerStat.ReduceToValue(WillPowerStat.DecayPerSecond * dt);
        //    }

        //    if (waterTimer > 0f) waterTimer -= Time.deltaTime;
        //    if (cleanTimer > 0f) cleanTimer -= Time.deltaTime;
        //    if (inspireTimer > 0f) inspireTimer -= Time.deltaTime;

        //    OnStatsChanged?.Invoke(CharismaStat, WisdomStat, WillPowerStat);

        //    if (decayEnabled)
        //        CheckDepleted();
        //}

        //public void SetDecayMultiplier(float multiplier)
        //{
        //    decayMultiplier = multiplier;
        //}

        //public void EnableDecay(bool enable)
        //{
        //    decayEnabled = enable;
        //}

        public void HandleWateringAction()
        {
            //if (!CanWater || isDead) return;
            //waterTimer = waterCooldown;
            ApplyAction(CharismaStat, WateringPointsPerLevel);
        }

        public void HandleFeedingAction()
        {
            //if (!CanClean || isDead) return;
            //cleanTimer = cleanCooldown;
            ApplyAction(WisdomStat, FeedingPointsPerLevel);
        }

        public void HandleExercisingAction()
        {
            //if (!CanInspire || isDead) return;
            //inspireTimer = inspireCooldown;
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
            if (CharismaStat.Level == WisdomStat.Level &&
                WisdomStat.Level == WillPowerStat.Level)
            {
                OnAllStatsReachedSameLevel?.Invoke(CharismaStat.Level);
                
            }
        }

        public void ApplyEventEffects(float charismaChange, float knowledgeChange, float determinationChange)
        {
            if (charismaChange >= 0)
                CharismaStat.AddToValue(charismaChange);
            else
                CharismaStat.ReduceToValue(Mathf.Abs(charismaChange));

            if (knowledgeChange >= 0)
                WisdomStat.AddToValue(knowledgeChange);
            else
                WisdomStat.ReduceToValue(Mathf.Abs(knowledgeChange));

            if (determinationChange >= 0)
                WillPowerStat.AddToValue(determinationChange);
            else
                WillPowerStat.ReduceToValue(Mathf.Abs(determinationChange));

            //OnStatLevelUp?.Invoke(CharismaStat, WisdomStat, WillPowerStat);
        }

        //private void CheckDepleted()
        //{
        //    if (CharismaStat.IsDepleted)
        //    {
        //        isDead = true;
        //        OnStatDepleted?.Invoke(CharismaStat.Name);
        //    }
        //    else if (WisdomStat.IsDepleted)
        //    {
        //        isDead = true;
        //        OnStatDepleted?.Invoke(WisdomStat.Name);
        //    }
        //    else if (WillPowerStat.IsDepleted)
        //    {
        //        isDead = true;
        //        OnStatDepleted?.Invoke(WillPowerStat.Name);
        //    }
        //}

        public void ResetStats()
        {
            CharismaStat.Value = 0f;
            WisdomStat.Value = 0f;
            WillPowerStat.Value = 0f;
            isDead = false;
        }
    }
}