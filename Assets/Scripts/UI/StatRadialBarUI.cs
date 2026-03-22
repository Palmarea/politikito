using UnityEngine;
using UnityEngine.UI;
using Game.Character;
using System.Collections;

namespace Game.UI
{
    public class StatRadialBarUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Slider Slider;

        [Header("Parameters")]
        [SerializeField] private float VisualMaxValue = 25f;
        [SerializeField] private float FillSpeed = 40f;

        private Coroutine fillRoutine;
        private bool lockedFull = false;
        private int lastLevel = 0;

        private void Awake()
        {
            Slider.maxValue = VisualMaxValue;
        }

        public void UpdateBar(TamaStat stat)
        {
            if (lockedFull) return;
            
            float levelStart = stat.Level * VisualMaxValue;
            float targetValue = stat.Value - levelStart;

            if (Slider != null)
            {
                if (fillRoutine != null)
                    StopCoroutine(fillRoutine);

                if (lastLevel != stat.Level)
                {
                    targetValue = VisualMaxValue;
                    lockedFull = true;
                    lastLevel = stat.Level;
                }

                fillRoutine = StartCoroutine(AnimateBar(targetValue));
            }
        }

        public void ResetBar()
        {
            lockedFull = false;

            if (fillRoutine != null)
                StopCoroutine(fillRoutine);

            Slider.value = 0;
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