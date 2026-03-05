using UnityEngine;
using System;
using Game.Managers.Timing;

namespace Game.Managers.Timing
{
    public class DaySystem : MonoBehaviour
    {
        [Header("Config")]
        [Tooltip("How many real seconds equals one in-game day")]
        [SerializeField] private float secondsPerDay = 60f;

        [Header("References")]
        [SerializeField] private TMPro.TMP_Text dayText;

        private int currentDay = 1;
        private float lastDayTime = 0f;

        // Events
        public event Action<int> OnNewDay;

        // Properties
        public int CurrentDay => currentDay;

        private void Update()
        {
            if (TimeManager.Instance == null) return;
            if (TimeManager.Instance.TimeStop) return;

            float elapsed = TimeManager.Instance.CurrentTime - lastDayTime;

            if (elapsed >= secondsPerDay)
            {
                lastDayTime = TimeManager.Instance.CurrentTime;
                currentDay++;
                OnNewDay?.Invoke(currentDay);

                if (dayText != null)
                    dayText.text = "Day " + currentDay;
            }
        }

        private void Start()
        {
            if (dayText != null)
                dayText.text = "Day 1";
        }
    }
}