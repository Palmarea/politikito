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
        private bool ignoreNextUpdate = false;

        private void Awake()
        {
            Slider.maxValue = VisualMaxValue;
        }

        public void UpdateBar(TamaStat stat)
        {
            if (ignoreNextUpdate)
            {
                ignoreNextUpdate = false;
                return;
            }

            if (lockedFull)
                return;

            float targetValue = stat.Value % VisualMaxValue;

            if (targetValue == 0 && stat.Value > 0)
            {
                targetValue = VisualMaxValue;
                lockedFull = true;
            }

            if (fillRoutine != null)
                StopCoroutine(fillRoutine);

            fillRoutine = StartCoroutine(AnimateBar(targetValue));
        }

        public void ResetBar()
        {
            lockedFull = false;
            ignoreNextUpdate = true;

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