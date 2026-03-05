using UnityEngine;
using System;

namespace Game.Character
{
    [System.Serializable]
    public class TamaStat
    {
        public string Name;
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
            Name = "Charisma",
            Value = 75,
            DecayPerSecond = 0.3f
        };
        [SerializeField]
        private TamaStat HonestyStat = new TamaStat
        {
            Name = "Knowledge",
            Value = 75,
            DecayPerSecond = 0.25f
        };
        [SerializeField]
        private TamaStat WillPowerStat = new TamaStat
        {
            Name = "Determination",
            Value = 75,
            DecayPerSecond = 0.35f
        };

        [Header("Player Actions")]
        [SerializeField] private float waterAmount = 15f;
        [SerializeField] private float cleanAmount = 12f;
        [SerializeField] private float inspireAmount = 10f;

        [Header("Cooldowns (seconds)")]
        [SerializeField] private float waterCooldown = 5f;
        [SerializeField] private float cleanCooldown = 4f;
        [SerializeField] private float inspireCooldown = 6f;

        [Header("Decay")]
        [Tooltip("Uncheck to disable stat decay. Enable for cinematic ending.")]
        [SerializeField] private bool decayEnabled = false;

        private float waterTimer = 0f;
        private float cleanTimer = 0f;
        private float inspireTimer = 0f;
        private float decayMultiplier = 1f;
        private bool isDead = false;

        // Events
        public event Action<TamaStat, TamaStat, TamaStat> OnStatsChanged;
        public event Action<string> OnStatDepleted;

        // Properties
        public TamaStat Charisma => CharismaStat;
        public TamaStat Honesty => HonestyStat;
        public TamaStat WillPower => WillPowerStat;
        public float AverageStats => (CharismaStat.Value + HonestyStat.Value + WillPowerStat.Value) / 3f;
        public bool CanWater => waterTimer <= 0f;
        public bool CanClean => cleanTimer <= 0f;
        public bool CanInspire => inspireTimer <= 0f;
        public float WaterCooldownRemaining => waterTimer;
        public float CleanCooldownRemaining => cleanTimer;
        public float InspireCooldownRemaining => inspireTimer;
        public bool DecayEnabled => decayEnabled;

        private void Update()
        {
            if (isDead) return;

            // Only decay if enabled
            if (decayEnabled)
            {
                float dt = Time.deltaTime * decayMultiplier;
                CharismaStat.ReduceToValue(CharismaStat.DecayPerSecond * dt);
                HonestyStat.ReduceToValue(HonestyStat.DecayPerSecond * dt);
                WillPowerStat.ReduceToValue(WillPowerStat.DecayPerSecond * dt);
            }

            if (waterTimer > 0f) waterTimer -= Time.deltaTime;
            if (cleanTimer > 0f) cleanTimer -= Time.deltaTime;
            if (inspireTimer > 0f) inspireTimer -= Time.deltaTime;

            OnStatsChanged?.Invoke(CharismaStat, HonestyStat, WillPowerStat);

            if (decayEnabled)
                CheckDepleted();
        }

        public void SetDecayMultiplier(float multiplier)
        {
            decayMultiplier = multiplier;
        }

        public void EnableDecay(bool enable)
        {
            decayEnabled = enable;
        }

        public void Water()
        {
            if (!CanWater || isDead) return;
            CharismaStat.AddToValue(waterAmount);
            waterTimer = waterCooldown;
            OnStatsChanged?.Invoke(CharismaStat, HonestyStat, WillPowerStat);
        }

        public void CleanCorruption()
        {
            if (!CanClean || isDead) return;
            HonestyStat.AddToValue(cleanAmount);
            cleanTimer = cleanCooldown;
            OnStatsChanged?.Invoke(CharismaStat, HonestyStat, WillPowerStat);
        }

        public void Inspire()
        {
            if (!CanInspire || isDead) return;
            WillPowerStat.AddToValue(inspireAmount);
            inspireTimer = inspireCooldown;
            OnStatsChanged?.Invoke(CharismaStat, HonestyStat, WillPowerStat);
        }

        public void ApplyEventEffects(float charismaChange, float knowledgeChange, float determinationChange)
        {
            if (charismaChange >= 0)
                CharismaStat.AddToValue(charismaChange);
            else
                CharismaStat.ReduceToValue(Mathf.Abs(charismaChange));

            if (knowledgeChange >= 0)
                HonestyStat.AddToValue(knowledgeChange);
            else
                HonestyStat.ReduceToValue(Mathf.Abs(knowledgeChange));

            if (determinationChange >= 0)
                WillPowerStat.AddToValue(determinationChange);
            else
                WillPowerStat.ReduceToValue(Mathf.Abs(determinationChange));

            OnStatsChanged?.Invoke(CharismaStat, HonestyStat, WillPowerStat);
        }

        private void CheckDepleted()
        {
            if (CharismaStat.IsDepleted)
            {
                isDead = true;
                OnStatDepleted?.Invoke(CharismaStat.Name);
            }
            else if (HonestyStat.IsDepleted)
            {
                isDead = true;
                OnStatDepleted?.Invoke(HonestyStat.Name);
            }
            else if (WillPowerStat.IsDepleted)
            {
                isDead = true;
                OnStatDepleted?.Invoke(WillPowerStat.Name);
            }
        }

        public void ResetStats()
        {
            CharismaStat.Value = 75f;
            HonestyStat.Value = 75f;
            WillPowerStat.Value = 75f;
            isDead = false;
        }
    }
}