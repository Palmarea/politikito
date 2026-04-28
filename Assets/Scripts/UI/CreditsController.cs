using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using FMODUnity;

namespace Game.UI
{
    public class CreditsController : MonoBehaviour
    {
        [Header("Credit Sections (show in order)")]
        [SerializeField] private CanvasGroup[] creditSections;

        [Header("Timing")]
        [SerializeField] private float fadeInDuration = 1f;
        [SerializeField] private float displayDuration = 3f;
        [SerializeField] private float fadeOutDuration = 1f;
        [SerializeField] private float pauseBetweenSections = 0.5f;
        [SerializeField] private FMODUnity.StudioEventEmitter emitter;

        [Header("Transition")]
        [SerializeField] private string titleSceneName = "TitleScreen";

        private void Start()
        {
            // Hide all sections at start
            foreach (var section in creditSections)
            {
                if (section != null)
                    section.alpha = 0f;
            }
            
            StartCoroutine(PlayCredits());
        }

        private IEnumerator PlayCredits()
        {
            emitter.SetParameter("creditsVolume", 0.9f);
            yield return new WaitForSeconds(1f);

            foreach (var section in creditSections)
            {
                if (section == null) continue;

                // Fade in
                yield return StartCoroutine(FadeCanvasGroup(section, 0f, 1f, fadeInDuration));

                // Display
                yield return new WaitForSeconds(displayDuration);

                // Fade out
                yield return StartCoroutine(FadeCanvasGroup(section, 1f, 0f, fadeOutDuration));

                // Pause
                yield return new WaitForSeconds(pauseBetweenSections);
            }
            
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(FadeOutVolume(2f));
            SceneManager.LoadScene(titleSceneName);
        }

        private IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                group.alpha = Mathf.Lerp(from, to, t);
                yield return null;
            }
            group.alpha = to;
        }

        private IEnumerator FadeOutVolume(float duration)
        {
            float startCredits = 1f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float t = duration > 0f ? elapsed / duration : 1f;
                emitter.SetParameter("creditsVolume", Mathf.Lerp(startCredits, 0f, t));

                elapsed += Time.deltaTime;
                yield return null;
            }

            emitter.SetParameter("creditsVolume", 0f);
        }
    }
}