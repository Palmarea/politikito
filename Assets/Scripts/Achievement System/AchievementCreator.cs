using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Systems.Achievement
{
    public class AchievementCreator : MonoBehaviour
    {
        public Dictionary<StatType, List<Achievement>> CreateAchievementDictionary(AchievementDatabaseSO database)
        {
            Dictionary<StatType, List<Achievement>> dict = new();

            foreach (var achievement in database.AchievementDB)
            {
                if (!dict.ContainsKey(achievement.stat))
                    dict[achievement.stat] = new List<Achievement>();

                dict[achievement.stat].Add(achievement);
            }

            foreach (var list in dict.Values)
            {
                list.Sort((a, b) => a.level.CompareTo(b.level));
            }

            return dict;
        }
    }
}