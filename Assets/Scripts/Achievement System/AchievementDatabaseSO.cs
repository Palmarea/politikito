using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Achievement
{
    [CreateAssetMenu(fileName = "AchievementDB", menuName = "Game/Achievement Database")]
    public class AchievementDatabaseSO : ScriptableObject
    {
        public List<Achievement> AchievementDB = new List<Achievement>();
    }

    public enum StatType
    {
        Carisma,
        Sabiduria,
        Voluntad
    }
}