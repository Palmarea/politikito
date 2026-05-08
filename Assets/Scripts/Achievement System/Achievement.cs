using System.Text;
using UnityEngine;
using UnityEngine.Localization;

namespace Game.Systems.Achievement
{
    [System.Serializable]
    public class Achievement
    {
        public int level;
        public StatType stat;
        public string spriteAtlasID;
        public string detailObjectID;
        public Vector3 spawnPosition;
        public Vector3 spawnRotation;
        public Vector3 spawnScale;
        public LocalizedString localizedStat;
        public LocalizedString localizedDescription;
        public LocalizedString localizedObjectName;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Achievement:");
            sb.AppendLine($"  Level: {level}");
            sb.AppendLine($"  Statistic : {stat}");
            sb.AppendLine($"  Sprite Atlas Identifier: {spriteAtlasID}");
            sb.AppendLine($"  Detail Atlas Identifier: {detailObjectID}");
            sb.AppendLine($"  Spawn Position: {spawnPosition}");
            sb.AppendLine($"  Spawn Rotation: {spawnRotation}");
            sb.AppendLine($"  Spawn Scale: {spawnScale}");

            return sb.ToString();
        }

        public string GetStatToTitleCase()
        {
            string stat = localizedStat.GetLocalizedString();
            
            if (string.IsNullOrEmpty(stat))
                return string.Empty;

            if (stat.Length == 1)
                return char.ToUpper(stat[0]).ToString();

            return char.ToUpper(stat[0]) + stat.Substring(1).ToLower();
        }
    }
}