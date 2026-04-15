using System.Collections;
using System.Text;
using UnityEngine;

namespace Game.Systems.Interaction.Detail
{
    public enum DetailType
    {
        ACHIEVEMENT,
        MILESTONE
    }
    
    [System.Serializable]
    public class DetailObjData
    {
        public string objectID;
        public string spriteAtlasID;
        public string spriteAtlasMilestoneID;
        public DetailType type;
        [TextArea] public string description;

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.AppendLine("Detail Object Data:");
            sb.AppendLine($"  Identifier : {objectID}");
            sb.AppendLine($"  Sprite Atlas Identifier: {spriteAtlasID}");
            sb.AppendLine($"  Detail Type: {type}");
            sb.AppendLine($"  Description: {description}");

            return sb.ToString();
        }
    }

    public class DetailObject : MonoBehaviour
    {
        public DetailObjData m_Data;

        public void SetDetailData(DetailObjData outData) { m_Data = outData; }

        public void PresentDetail()
        {
            if (m_Data == null || DetailSystem.Instance == null) return;

            DetailSystem.Instance.ShowDetail(m_Data);
        }
    }
}