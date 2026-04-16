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
        [SerializeField] private Image FillImage;

        [Header("Parameters")]
        [SerializeField] private float VisualMaxValue = 25f;
        [SerializeField] private float FillSpeed = 40f;

        [Header("Cooldown Colors")]
        [SerializeField] private Color NormalColor = new Color(0.98f, 0.56f, 0.45f, 1f);   // #FA8E72
        [SerializeField] private Color CooldownColor = new Color(0.45f, 0.77f, 0.98f, 1f); // #72C4FA

        private Coroutine fillRoutine;
        private bool lockedFull = false;
        private int lastLevel = 0;
        private bool isCooldownMode = false;
        private TamaStat cachedStat;

        private void Awake()
        {
            Slider.maxValue = VisualMaxValue;

            if (FillImage != null)
                FillImage.color = NormalColor;
        }

        public void UpdateBar(TamaStat stat)
        {
            if (lockedFull) return;
            if (isCooldownMode) 
            {
                cachedStat = stat;
                return;
            }

            cachedStat = stat;

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

        // Llamado cuando empieza el cooldown del minijuego
        public void StartCooldown(float cooldownDuration)
        {
            isCooldownMode = true;

            if (FillImage != null)
                FillImage.color = CooldownColor;

            if (fillRoutine != null)
                StopCoroutine(fillRoutine);

            Slider.value = 0;
            fillRoutine = StartCoroutine(AnimateCooldown(cooldownDuration));
        }

        // Llamado cuando termina el cooldown
        public void EndCooldown()
        {
            isCooldownMode = false;

            if (FillImage != null)
                FillImage.color = NormalColor;

            if (fillRoutine != null)
                StopCoroutine(fillRoutine);

            // Vuelve a mostrar la stat real
            if (cachedStat != null)
            {
                float levelStart = cachedStat.Level * VisualMaxValue;
                float targetValue = cachedStat.Value - levelStart;
                fillRoutine = StartCoroutine(AnimateBar(targetValue));
            }
            else
            {
                Slider.value = 0;
            }
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

        private IEnumerator AnimateCooldown(float duration)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                Slider.value = Mathf.Lerp(0f, VisualMaxValue, elapsed / duration);
                yield return null;
            }

            Slider.value = VisualMaxValue;
        }
    }
}