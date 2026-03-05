using Game.Character;
using System.Collections;
using UnityEngine;

namespace Game.Systems.Milestone
{
    public class StatsMilestoneBridge : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TamaCharacterStats CharacterStats;
        [SerializeField] private MilestoneSystem MilestoneSystem;

        private void Start()
        {
            if (CharacterStats != null)
                CharacterStats.OnAllStatsReachedSameLevel += RequestLevelUpMilestone;
        }

        private void RequestLevelUpMilestone(int level)
        {
            if (MilestoneSystem == null) return;

            MilestoneSystem.AdvanceMilestone(level);
        }

        private void OnDestroy()
        {
            if (CharacterStats != null)
                CharacterStats.OnAllStatsReachedSameLevel -= RequestLevelUpMilestone;
        }
    }
}