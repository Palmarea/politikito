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
        [SerializeField] private SpriteRenderer[] section1Doodles;
        [SerializeField] private SpriteRenderer[] section2Doodles;

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
            foreach (var section in creditSections)
            {
                if (section != null) section.alpha = 0f;
            }

            foreach (var sprite in section1Doodles)
            {
                if (sprite == null) continue;

                Color color = sprite.color;
                color.a = 0f;
                sprite.color = color;
            }
            
            foreach (var sprite in section2Doodles)
            {
                if (sprite == null) continue;

                Color color = sprite.color;
                color.a = 0f;
                sprite.color = color;
            }

            StartCoroutine(PlayCredits());
        }

        private IEnumerator PlayCredits()
        {
            emitter.SetParameter("creditsVolume", 0.9f);
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < creditSections.Length; i++)
            {
                CanvasGroup section = creditSections[i];

                if (section == null) continue;

                SpriteRenderer[] doodles = GetDoodlesForSection(i);

                // Fade in simultáneo
                StartCoroutine(FadeCanvasGroup(section, 0f, 1f, fadeInDuration));

                if (doodles != null)
                {
                    StartCoroutine(FadeSprites(doodles, 0f, 1f, fadeInDuration));
                }

                yield return new WaitForSeconds(fadeInDuration);

                // Display
                yield return new WaitForSeconds(displayDuration);

                // Fade out simultáneo
                StartCoroutine(FadeCanvasGroup(section, 1f, 0f, fadeOutDuration));

                if (doodles != null)
                {
                    StartCoroutine(FadeSprites(doodles, 1f, 0f, fadeOutDuration));
                }

                yield return new WaitForSeconds(fadeOutDuration);

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

        private IEnumerator FadeSprites(SpriteRenderer[] sprites, float from, float to, float duration)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                float t = Mathf.Clamp01(elapsed / duration);
                float alpha = Mathf.Lerp(from, to, t);

                foreach (var sprite in sprites)
                {
                    if (sprite == null) continue;

                    Color color = sprite.color;
                    color.a = alpha;
                    sprite.color = color;
                }

                yield return null;
            }

            foreach (var sprite in sprites)
            {
                if (sprite == null) continue;

                Color color = sprite.color;
                color.a = to;
                sprite.color = color;
            }
        }

        private SpriteRenderer[] GetDoodlesForSection(int index)
        {
            switch (index)
            {
                case 1: return section1Doodles;
                case 2: return section2Doodles;
                default: return null;
            }
        }
    }
}