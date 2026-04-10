using System.Text;
using UnityEngine;

namespace Game.Systems.Achievement
{
    [System.Serializable]
    public class Achievement
    {
        public int level;
        public StatType stat;
        public string spriteAtlasID;
        public string detailObjectID;
        [TextArea] public string description;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Achievement:");
            sb.AppendLine($"  Level: {level}");
            sb.AppendLine($"  Statistic : {stat}");
            sb.AppendLine($"  Sprite Atlas Identifier: {spriteAtlasID}");
            sb.AppendLine($"  Detail Atlas Identifier: {detailObjectID}");
            sb.AppendLine($"  Description: {description}");

            return sb.ToString();
        }
    }
}