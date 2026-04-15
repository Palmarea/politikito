using System.Collections;
using UnityEngine;

namespace Game.Systems.Ending
{
    public class RandomAnimatorTrigger : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Animator animator;

        [Header("Settings")]
        [SerializeField] private string triggerName = "Play";
        [SerializeField] private float minTime = 2f;
        [SerializeField] private float maxTime = 5f;

        [Header("Control")]
        [SerializeField] private bool playOnStart = true;

        private Coroutine loopCoroutine;

        private void Start()
        {
            if (playOnStart)
                StartLoop();
        }

        public void StartLoop()
        {
            if (loopCoroutine == null)
                loopCoroutine = StartCoroutine(TriggerLoop());
        }

        public void StopLoop()
        {
            if (loopCoroutine != null)
            {
                StopCoroutine(loopCoroutine);
                loopCoroutine = null;
            }
        }

        private IEnumerator TriggerLoop()
        {
            while (true)
            {
                float waitTime = Random.Range(minTime, maxTime);
                yield return new WaitForSeconds(waitTime);

                animator.SetTrigger(triggerName);
            }
        }
    }
}