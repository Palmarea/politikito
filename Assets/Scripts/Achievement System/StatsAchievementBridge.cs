using UnityEngine;
using Game.Systems.Achievement;
using Game.Character;

namespace Game.Systems.Achievement
{
    public class StatsAchievementBridge : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TamaCharacterStats CharacterStats;
        [SerializeField] private AchievementSystem AchievementSystem;

        private void Start()
        {
            if (CharacterStats != null)
                CharacterStats.OnStatLevelUp += RequestLevelUpAchievement;
        }

        private void RequestLevelUpAchievement(TamaStat stat)
        {
            if (AchievementSystem == null) return;

            AchievementSystem.AdvanceAchievement();
        }

        private void OnDestroy()
        {
            if (CharacterStats != null)
                CharacterStats.OnStatLevelUp -= RequestLevelUpAchievement;
        }
    }
}