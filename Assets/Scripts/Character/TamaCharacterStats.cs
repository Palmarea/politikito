using UnityEngine;

namespace Game.Character
{
    [System.Serializable]
    public class TamaStat
    {
        public string Name;
        public float Value;
        public float MinValue = 0;
        public float MaxValue = 100;

        public void AddToValue(float amount) => Value = Mathf.Clamp(Value + amount, MinValue, MaxValue);

        public void ReduceToValue(float amount) => Value = Mathf.Clamp(Value - amount, MinValue, MaxValue);
    }
    
    [RequireComponent(typeof(TamaCharacterController))]
    public class TamaCharacterStats : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private TamaStat IntegrityStat;
        [SerializeField] private TamaStat HonestyStat;
        [SerializeField] private TamaStat WillPowerStat;

        private void Awake()
        {
            
        }
    }
}