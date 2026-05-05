using Febucci.UI;
using Game.Managers.Mouse;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Systems.Ending
{
    public class EndingConvesationController : MonoBehaviour
    {
        [System.Serializable]
        private class UIObject
        {
            public RectTransform objTransform;
            public CanvasGroup objCanvasGroup;
        }

        [Header("References")]
        [SerializeField] private UIObject character;
        [SerializeField] private List<UIObject> statBars;
        [SerializeField] private UIObject textBox;
        [SerializeField] private UIObject background;
        [SerializeField] private RandomAnimatorTrigger TriggerAnim;

        [Header("Particles")]
        [SerializeField] private List<ParticleSystem> StatParticles;
        [SerializeField] private ParticleSystem MouseParticle;

        [Header("Container")]
        [SerializeField] private RectTransform Container;
        [SerializeField] private float containerMoveY = 200f;
        [SerializeField] private float containerMoveDuration = 1f;

        [Header("Floating")]
        [SerializeField] private float floatAmplitude = 10f;
        [SerializeField] private float floatSpeed = 2f;

        [Header("Typing")]
        [SerializeField] private TypewriterByCharacter typewriter;
        [SerializeField] private float afterTypeDuration = 1f;

        [Header("Scene")]
        [SerializeField] private string nextSceneName;

        private AsyncOperation asyncLoad;

        private List<Vector2> originalStatPositions = new List<Vector2>();
        private Vector2 originalContainerPos;

        private bool forceMouseToCenter = false;
        private bool visibleMouse = true;
        private bool keepShakingStats = false;
        private bool floatStats = false;

        private void Start()
        {
            originalContainerPos = Container.anchoredPosition;

            // Character
            character.objCanvasGroup.alpha = 0f;
            character.objTransform.gameObject.SetActive(true);

            // TextBox
            textBox.objCanvasGroup.alpha = 0f;
            textBox.objTransform.gameObject.SetActive(true);

            // Stats
            foreach (var item in statBars)
            {
                item.objTransform.GetComponent<Slider>().value = 25f;

                originalStatPositions.Add(item.objTransform.anchoredPosition);

                item.objCanvasGroup.alpha = 0f;
                item.objTransform.gameObject.SetActive(false);
            }

            TriggerAnim.SetNeedFull(true);

            StartCoroutine(RunEndingSequence());
        }

        private void Update()
        {
            if (forceMouseToCenter)
                ForceMouseToCenter();

            if (keepShakingStats)
                ShakeStatsRealtime();
            else if (floatStats)
                FloatStats();
        }

        // =========================
        // SECUENCIA
        // =========================
        private IEnumerator RunEndingSequence()
        {
            yield return FadeUI(background, 0, 1, 1f);

            yield return FadeUI(character, 0, 1, 1f);
            TriggerAnim.SetNeedFull(false);
            yield return FadeUI(textBox, 0, 1, 0.5f);

            yield return TypeLine("HOLA.");
            yield return TypeLine("GRACIAS POR IMPULSAR MI CRECIMIENTO.");

            //  Mover container antes de mostrar stats
            yield return MoveContainer(originalContainerPos, originalContainerPos + Vector2.up * containerMoveY);

            floatStats = true;

            yield return ShowStat(0, "LA REGADERA.");
            yield return ShowStat(1, "LAS GALLETAS.");
            yield return ShowStat(2, "EL EJERCICIO.");

            yield return TypeLine("FUERON HERRAMIENTAS ÚTILES PARA CONSTRUIR MI CAMINO AL PODER.");

            yield return TypeLine("PERO YA NO MÁS.");

            //  detener float y resetear posiciones
            floatStats = false;
            ResetStatsPosition();

            yield return DrainStats();

            yield return TypeLine("YA NO LAS NECESITO.");
            BreakStats();

            //  volver container a su posición original
            yield return MoveContainer(Container.anchoredPosition, originalContainerPos);

            LockMouseToCenter();
            yield return TypeLine("Y EN CUANTO A TI...");

            BreakMouse();
            yield return TypeLine("TAMPOCO TE NECESITO.");

            yield return TypeLine("AHORA YO TENGO EL PODER... EL CONTROL...");
            yield return TypeLine("GRACIAS POR CUMPLIR TU PAPEL.");
            yield return TypeLine("HASTA NUNCA.");


            yield return FadeUI(textBox, 1, 0, 0.5f);
            TriggerAnim.SetNeedFull(true);
            yield return FadeUI(character, 1, 0, 1f);
            yield return FadeUI(background, 1, 0, 1f);

            ResetCursor();

            yield return ActivateLoadedScene();
        }

        // =========================
        // HELPERS
        // =========================

        private IEnumerator FadeUI(UIObject obj, float from, float to, float duration)
        {
            float t = 0;
            obj.objCanvasGroup.alpha = from;

            while (t < duration)
            {
                t += Time.deltaTime;
                obj.objCanvasGroup.alpha = Mathf.Lerp(from, to, t / duration);
                yield return null;
            }

            obj.objCanvasGroup.alpha = to;
        }

        private IEnumerator TypeLine(string text)
        {
            typewriter.ShowText(text);
            yield return new WaitUntil(() => !typewriter.isShowingText);
            yield return new WaitForSeconds(afterTypeDuration);
        }

        private IEnumerator ShowStat(int index, string line)
        {
            var stat = statBars[index];
            stat.objTransform.gameObject.SetActive(true);

            yield return FadeUI(stat, 0, 1, 0.5f);
            yield return TypeLine(line);
        }

        private IEnumerator DrainStats()
        {
            keepShakingStats = true;
            SFXCaller.Play("event:/uiButtonBreak3");

            float duration = 2f;
            float t = 0;

            List<Slider> sliders = new List<Slider>();
            foreach (var s in statBars)
                sliders.Add(s.objTransform.GetComponent<Slider>());

            while (t < duration)
            {
                t += Time.deltaTime;

                foreach (var slider in sliders)
                    slider.value = Mathf.Lerp(25f, 0f, t / duration);

                yield return null;
            }
        }

        private void ShakeStatsRealtime()
        {
            for (int i = 0; i < statBars.Count; i++)
            {
                Vector2 basePos = originalStatPositions[i];
                Vector2 offset = Random.insideUnitCircle * 5f;

                statBars[i].objTransform.anchoredPosition = basePos + offset;
            }
        }

        private void FloatStats()
        {
            float time = Time.time;

            for (int i = 0; i < statBars.Count; i++)
            {
                Vector2 basePos = originalStatPositions[i];

                float offsetY = Mathf.Sin(time * floatSpeed + i) * floatAmplitude;

                statBars[i].objTransform.anchoredPosition = basePos + Vector2.up * offsetY;
            }
        }

        private void ResetStatsPosition()
        {
            for (int i = 0; i < statBars.Count; i++)
            {
                statBars[i].objTransform.anchoredPosition = originalStatPositions[i];
            }
        }

        private void BreakStats()
        {
            keepShakingStats = false;

            string[] breakref = { "event:/uiButtonBreak1", "event:/uiButtonBreak2", "event:/uiButtonBreak3" };

            for (int i = 0; i < statBars.Count; i++)
            {
                statBars[i].objTransform.anchoredPosition = originalStatPositions[i];
                statBars[i].objTransform.gameObject.SetActive(false);
                StatParticles[i].Play();
                SFXCaller.Play(breakref[i]);
            }
        }

        private IEnumerator MoveContainer(Vector2 from, Vector2 to)
        {
            float t = 0;

            while (t < containerMoveDuration)
            {
                t += Time.deltaTime;
                Container.anchoredPosition = Vector2.Lerp(from, to, t / containerMoveDuration);
                yield return null;
            }

            Container.anchoredPosition = to;
        }

        // =========================
        // MOUSE
        // =========================

        private void LockMouseToCenter()
        {
            Cursor.lockState = CursorLockMode.Confined;
            forceMouseToCenter = true;
            ForceMouseToCenter();
        }

        private void ForceMouseToCenter()
        {
            if (Mouse.current == null)
                return;

            Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Vector2 jitter = Random.insideUnitCircle * 2f;

            if (!visibleMouse)
            {
                Cursor.visible = false;
                CursorManager.Instance.SetCursorVisibility(false);
            }

            Mouse.current.WarpCursorPosition(center + jitter);
        }

        private void BreakMouse()
        {
            Cursor.visible = false;
            CursorManager.Instance.SetCursorVisibility(false);
            visibleMouse = false;
            MouseParticle.Play();
            SFXCaller.Play("event:/uiButtonBreak2");
            StartAsyncLoad();
        }

        private void ResetCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
            CursorManager.Instance.SetCursorVisibility(true);

            forceMouseToCenter = false;
            visibleMouse = true;
        }

        // =========================
        // TRANSICION ESCENA
        // =========================

        private void StartAsyncLoad()
        {
            if (asyncLoad != null) return;

            asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
            asyncLoad.allowSceneActivation = false;

            StartCoroutine(WaitForSceneReady());
        }

        private IEnumerator WaitForSceneReady()
        {
            while (asyncLoad.progress < 0.9f)
            {
                yield return null;
            }
        }

        private IEnumerator ActivateLoadedScene()
        {
            yield return new WaitUntil(() => asyncLoad != null && asyncLoad.progress >= 0.9f);

            asyncLoad.allowSceneActivation = true;
        }

        private void OnDestroy()
        {
            ResetCursor();
        }

        private void OnDisable()
        {
            ResetCursor();
        }
    }
}