using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Achievement
{
    public class AchievementSystem : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private AchievementCreator Creator;

        [Header("Parameters")]
        [SerializeField] TextAsset AchievementJSON;
        [SerializeField] private int MaxAchievementList = 3;

        private Dictionary<int, List<Achievement>> AchievementDictionary = new Dictionary<int, List<Achievement>>();
        private int currentAchievementLevel = 1;
        private int currentAchievementOrder = 0;

        // Events
        public event Action<Achievement> OnAchievementRequested;

        private void Awake()
        {
            for (int i = 1; i < MaxAchievementList + 1; i++)
            {
                AchievementDictionary[i] = Creator.CreateAchievementListByLevel(AchievementJSON, i);

                for (int j = 0; j < AchievementDictionary[i].Count; j++)
                {
                    Debug.Log(AchievementDictionary[i][j].ToString());
                }
            }
        }

        public Achievement GetCurrentAchievement()
        {
            OnAchievementRequested?.Invoke(AchievementDictionary[currentAchievementLevel][currentAchievementOrder]);
            return AchievementDictionary[currentAchievementLevel][currentAchievementOrder];
        }

        public Achievement GetNextAchievement()
        {
            if (currentAchievementOrder >= AchievementDictionary[currentAchievementLevel].Count - 1)
            {
                if (currentAchievementLevel >= AchievementDictionary.Count)
                {
                    return null;
                }
                else
                {
                    currentAchievementLevel++;
                    currentAchievementOrder = 0;
                }
            }
            else
            {
                currentAchievementOrder++;
            }

            return GetCurrentAchievement();
        }
    }

}
