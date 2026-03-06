using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Character;

namespace Game.UI
{
    public class StatRadialBarUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Slider Slider;
        //[SerializeField] private TMP_Text LevelText;

        [Header("Parameters")]
        [SerializeField] private float VisualMaxValue = 25f;
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
                float levelProgress = stat.Value % VisualMaxValue;
                Slider.value = levelProgress;
            }

            //if (LevelText != null)
            //    LevelText.text = $"LVL {stat.Level}";
        }
    }
}