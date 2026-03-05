using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Character;

namespace Game.UI
{
    public class StatBarUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Slider Slider;
        [SerializeField] private TMP_Text LevelText;

        [Header("Parameters")]
        [SerializeField] private float VisualMaxValue = 125f;
        //[SerializeField] private TMP_Text valueText;

        private void Awake()
        {
            if (Slider != null)
                Slider.maxValue = VisualMaxValue;
        }

        public void UpdateBar(TamaStat stat)
        {
            if (Slider != null)
            {
                Slider.value = stat.Value;
            }

            if (LevelText != null)
                LevelText.text = $"LVL {stat.Level}";

            //if (valueText != null)
            //    valueText.text = Mathf.RoundToInt(stat.Value).ToString();
        }
    }
}