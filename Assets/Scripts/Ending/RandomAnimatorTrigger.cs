using System.Collections;
using UnityEngine;

namespace Game.Systems.Ending
{
    public class RandomAnimatorTrigger : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Animator animator;

        [Header("Settings")]
        [SerializeField] private float minTime = 2f;
        [SerializeField] private float maxTime = 5f;

        private Coroutine loopCoroutine;

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

        public void SetNeedFull(bool state)
        {
            animator.SetBool("NeedFull", state);

            if (!state)
            {
                StartLoop();
            }
            else
            {
                StopLoop();
            }
        }

        private IEnumerator TriggerLoop()
        {
            while (true)
            {
                float waitTime = Random.Range(minTime, maxTime);
                yield return new WaitForSeconds(waitTime);

                animator.SetTrigger("Spasm");
            }
        }
    }
}