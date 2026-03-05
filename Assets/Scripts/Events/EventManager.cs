using UnityEngine;
using System.Collections.Generic;
using Game.Managers.Timing;

namespace Game.Events
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }

        [Header("Config")]
        [SerializeField] private EventDataSO[] allEvents;
        [SerializeField] private float timeBetweenEvents = 15f;
        [SerializeField] private float firstEventDelay = 5f;

        [Header("Referencias")]
        [SerializeField] private EventPopupUI eventPopupUI;

        private float eventTimer;
        private List<EventDataSO> usedOneTimeEvents = new List<EventDataSO>();
        private bool eventActive = false;
        private int currentDay = 1;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            eventTimer = firstEventDelay;
        }

        private void Update()
        {
            if (eventActive) return;
            if (TimeManager.Instance != null && TimeManager.Instance.TimeStop) return;

            eventTimer -= Time.deltaTime;
            if (eventTimer <= 0f)
            {
                TriggerRandomEvent();
                eventTimer = timeBetweenEvents;
            }
        }

        public void SetCurrentDay(int day)
        {
            currentDay = day;
        }

        private void TriggerRandomEvent()
        {
            List<EventDataSO> available = new List<EventDataSO>();

            foreach (var evt in allEvents)
            {
                if (evt.minDay > currentDay) continue;
                if (evt.oneTimeOnly && usedOneTimeEvents.Contains(evt)) continue;
                available.Add(evt);
            }

            if (available.Count == 0) return;

            EventDataSO chosen = available[Random.Range(0, available.Count)];

            if (chosen.oneTimeOnly)
                usedOneTimeEvents.Add(chosen);

            ShowEvent(chosen);
        }

        private void ShowEvent(EventDataSO eventData)
        {
            eventActive = true;

            if (TimeManager.Instance != null && !TimeManager.Instance.TimeStop)
                TimeManager.Instance.ToggleTimeStop();

            if (eventPopupUI != null)
                eventPopupUI.ShowEvent(eventData);
        }

        public void OnEventResolved()
        {
            eventActive = false;

            if (TimeManager.Instance != null && TimeManager.Instance.TimeStop)
                TimeManager.Instance.ToggleTimeStop();
        }
    }
}