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
        [SerializeField] private TextAsset AchievementJSON;
        [SerializeField] private int MaxAchievementList = 3;

        private Dictionary<int, List<Achievement>> AchievementDictionary = new();
        private int currentAchievementLevel = 1;
        private int currentAchievementOrder = -1;

        // Nuevo evento
        public event Action<Achievement> OnNextAchievement;

        private void Awake()
        {
            for (int i = 1; i <= MaxAchievementList; i++)
            {
                AchievementDictionary[i] =
                    Creator.CreateAchievementListByLevel(AchievementJSON, i);
            }
        }

        public Achievement GetCurrentAchievement()
        {
            if (!AchievementDictionary.ContainsKey(currentAchievementLevel))
                return null;

            return AchievementDictionary[currentAchievementLevel][currentAchievementOrder];
        }

        public void AdvanceAchievement()
        {
            Achievement next = MoveNextInternal();

            if (next != null)
                OnNextAchievement?.Invoke(next);
        }

        public Achievement AdvanceAndGetAchievement()
        {
            Achievement next = MoveNextInternal();

            if (next != null)
                OnNextAchievement?.Invoke(next);

            return next;
        }

        private Achievement MoveNextInternal()
        {
            if (!AchievementDictionary.ContainsKey(currentAchievementLevel))
                return null;

            var currentList = AchievementDictionary[currentAchievementLevel];

            if (currentAchievementOrder < currentList.Count - 1)
            {
                currentAchievementOrder++;
            }
            else
            {
                if (currentAchievementLevel < MaxAchievementList)
                {
                    currentAchievementLevel++;
                    currentAchievementOrder = 0;
                }
                else
                {
                    return null;
                }
            }

            return GetCurrentAchievement();
        }
    }
}