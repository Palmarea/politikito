using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Character;
using System.Collections;

namespace Game.UI
{
    public class StatRadialBarUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Slider Slider;
        //[SerializeField] private TMP_Text LevelText;

        [Header("Parameters")]
        [SerializeField] private float VisualMaxValue = 25f;
        [SerializeField] private float FillSpeed = 40f;
        //[SerializeField] private TMP_Text valueText;
        private Coroutine fillRoutine;

        private void Awake()
        {
            if (Slider != null)
                Slider.maxValue = VisualMaxValue;
        }

        public void UpdateBar(TamaStat stat)
        {
            float levelStart = stat.Level * VisualMaxValue;
            float targetValue = stat.Value - levelStart;

            if (Slider != null)
            {
                if (fillRoutine != null)
                    StopCoroutine(fillRoutine);

                fillRoutine = StartCoroutine(AnimateBar(targetValue));
            }

            //if (LevelText != null)
            //    LevelText.text = $"LVL {stat.Level}";
        }

        private IEnumerator AnimateBar(float target)
        {
            while (Mathf.Abs(Slider.value - target) > 0.01f)
            {
                Slider.value = Mathf.MoveTowards(
                    Slider.value,
                    target,
                    FillSpeed * Time.deltaTime
                );

                yield return null;
            }

            Slider.value = target;
        }
    }
}