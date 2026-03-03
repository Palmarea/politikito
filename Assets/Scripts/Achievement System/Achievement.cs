using System.Text;

namespace Game.Systems.Achievement
{
    [System.Serializable]
    public class Achievement
    {
        public int level;
        public int order;
        public string title;
        public string description;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Achievement:");
            sb.AppendLine($"  Level: {level}");
            sb.AppendLine($"  Order: {order}");
            sb.AppendLine($"  Title: {title}");
            sb.AppendLine($"  Description: {description}");

            return sb.ToString();
        }
    }
}