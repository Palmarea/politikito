using UnityEngine;

namespace Game.Events
{
    [System.Serializable]
    public class EventChoice
    {
        public string choiceText;
        [Header("Efecto en Stats (positivo sube, negativo baja)")]
        public float charismaEffect;
        public float knowledgeEffect;
        public float determinationEffect;
        [Header("Mensaje de resultado")]
        [TextArea(1, 3)]
        public string resultMessage;
    }

    [CreateAssetMenu(fileName = "NewEvent", menuName = "PouPolitico/Event Data")]
    public class EventDataSO : ScriptableObject
    {
        public string eventTitle;
        [TextArea(3, 6)]
        public string eventDescription;
        public EventChoice[] choices;

        [Header("Condiciones")]
        [Tooltip("Dia minimo para que aparezca este evento")]
        public int minDay = 1;
        [Tooltip("Solo aparece una vez")]
        public bool oneTimeOnly = false;
    }
}