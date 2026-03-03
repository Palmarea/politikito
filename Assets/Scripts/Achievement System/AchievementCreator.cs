using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Achievement
{
    [System.Serializable]
    public class Achievements
    {
        public Achievement[] achievements;
    }
    
    public class AchievementCreator : MonoBehaviour
    {
        public List<Achievement> CreateAchievementListByLevel(TextAsset AchievementJSON, int levelId)
        {
            Achievements deserializedAchievements = JsonUtility.FromJson<Achievements>(AchievementJSON.text);

            List<Achievement> levelList = new List<Achievement>();

            foreach (Achievement milestone in deserializedAchievements.achievements)
            {
                if (milestone.level == levelId)
                {
                    levelList.Add(milestone);
                }
            }

            levelList.Sort((l1, l2) => l1.order.CompareTo(l2.order));

            return levelList;
        }
    }
}