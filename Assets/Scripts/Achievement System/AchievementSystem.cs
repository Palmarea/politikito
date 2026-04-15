using Game.Character;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Achievement
{
    public class AchievementSystem : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private AchievementCreator Creator;

        [Header("Data")]
        [SerializeField] private AchievementDatabaseSO Database;

        private Dictionary<StatType, List<Achievement>> AchievementDictionary = new();
        private Dictionary<StatType, int> currentIndexPerStat = new();

        public event Action<Achievement> OnNextAchievement;

        private void Awake()
        {
            AchievementDictionary = Creator.CreateAchievementDictionary(Database);

            foreach (var stat in AchievementDictionary.Keys)
            {
                currentIndexPerStat[stat] = -1;
            }
        }

        public Achievement GetCurrentAchievement(StatType stat)
        {
            if (!AchievementDictionary.ContainsKey(stat))
                return null;

            int index = currentIndexPerStat[stat];

            if (index < 0 || index >= AchievementDictionary[stat].Count)
                return null;

            return AchievementDictionary[stat][index];
        }

        public void AdvanceAchievement(TamaStat stat)
        {
            StatType parsedStat = Enum.Parse<StatType>(stat.Name);

            if (!AchievementDictionary.ContainsKey(parsedStat))
                return;

            var list = AchievementDictionary[parsedStat];
            int index = currentIndexPerStat[parsedStat];

            if (index < list.Count - 1)
            {
                index++;
                currentIndexPerStat[parsedStat] = index;

                OnNextAchievement?.Invoke(list[index]);
            }
        }
    }
}