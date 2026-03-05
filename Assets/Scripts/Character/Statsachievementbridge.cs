using UnityEngine;
using Game.Systems.Achievement;

namespace Game.Character
{
    public class StatsAchievementBridge : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TamaCharacterStats characterStats;
        [SerializeField] private AchievementSystem achievementSystem;

        [Header("Level Thresholds")]
        [Tooltip("When average stats reach these values, advance achievement")]
        [SerializeField] private float[] levelThresholds = { 30f, 50f, 70f, 85f, 95f };

        private int currentLevel = 0;

        private void Start()
        {
            if (characterStats != null)
                characterStats.OnStatsChanged += CheckForLevelUp;
        }

        private void CheckForLevelUp(TamaStat charisma, TamaStat knowledge, TamaStat determination)
        {
            if (achievementSystem == null) return;

            float avg = characterStats.AverageStats;

            for (int i = levelThresholds.Length - 1; i >= 0; i--)
            {
                if (avg >= levelThresholds[i] && i >= currentLevel)
                {
                    int levelsToAdvance = i - currentLevel + 1;
                    for (int j = 0; j < levelsToAdvance; j++)
                    {
                        achievementSystem.AdvanceAchievement();
                    }
                    currentLevel = i + 1;
                    break;
                }
            }
        }

        private void OnDestroy()
        {
            if (characterStats != null)
                characterStats.OnStatsChanged -= CheckForLevelUp;
        }
    }
}