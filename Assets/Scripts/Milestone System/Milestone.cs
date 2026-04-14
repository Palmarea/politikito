using System.Text;
using UnityEngine;

namespace Game.Systems.Milestone
{
    [System.Serializable]
    public class Milestone
    {
        public int level;
        public string spriteAtlasID;
        public string detailObjectID;
        public Vector3 spawnPosition;
        public Vector3 spawnRotation;
        public Vector3 spawnScale;
        [TextArea] public string description;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Milestone:");
            sb.AppendLine($"  Level: {level}");
            sb.AppendLine($"  Sprite Atlas Identifier: {spriteAtlasID}");
            sb.AppendLine($"  Detail Atlas Identifier: {detailObjectID}");
            sb.AppendLine($"  Spawn Position: {spawnPosition}");
            sb.AppendLine($"  Spawn Rotation: {spawnRotation}");
            sb.AppendLine($"  Spawn Scale: {spawnScale}");
            sb.AppendLine($"  Description: {description}");

            return sb.ToString();
        }
    }
}