using UnityEngine;

namespace Game.Managers.Timing
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance;

        #region Singleton
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(Instance);
            }
            else
            {
                Instance = this;
            }

            TimeStop = false;
        }
        #endregion

        public float CurrentTime { get; private set; }
        public bool TimeStop { get; private set; }

        private void Update()
        {
            if (TimeStop) return;

            CurrentTime += Time.deltaTime;
        }

        public void ToggleTimeStop() => TimeStop = !TimeStop;
    }
}
