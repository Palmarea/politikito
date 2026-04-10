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
        [TextArea] public string description;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Achievement:");
            sb.AppendLine($"  Level: {level}");
            sb.AppendLine($"  Statistic : {stat}");
            sb.AppendLine($"  Sprite Atlas Identifier: {spriteAtlasID}");
            sb.AppendLine($"  Description: {description}");

            return sb.ToString();
        }
    }
}