using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class TitleScreenController : MonoBehaviour
    {
        [Header("Splash")]
        [SerializeField] private GameObject splashFocus;

        [Header("Title")]
        [SerializeField] private GameObject gameLogo;

        [Header("Stamp")]
        [SerializeField] private GameObject stampMark;
        [SerializeField] private GameObject stampMarkBase;
        [SerializeField] private RectTransform stampRect;

        [Header("Transition")]
        [SerializeField] private string nextSceneName = "CharacterCreation";
        [SerializeField] private float delayBeforeTransition = 1.2f;

        [Header("Animation")]
        [SerializeField] private float stampScaleStart = 2f;
        [SerializeField] private float stampScaleEnd = 1f;
        [SerializeField] private float stampAnimDuration = 0.15f;

        private bool hasFocused = false;
        private bool hasStamped = false;

        private Sequence tweenSequence;
        private AsyncOperation sceneLoadOperation;

        private float initialCounter = 1f;
        private float counter = 0f;


        private void Start()
        {
            if (stampMark != null)
                stampMark.SetActive(false);

            if (gameLogo != null)
                gameLogo.SetActive(false);

            splashFocus.SetActive(true);
            TweenSplashFocus();

            counter = initialCounter;
        }

        private void Update()
        {
            if (counter <= 0)
            {
                if (!hasFocused)
                {
                    if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        OnFocusButtonPressed();
                    }

                    return;
                }
            }
            else
            {
                counter -= Time.deltaTime;
            }
        }

        private void TweenSplashFocus()
        {
            tweenSequence = DOTween.Sequence();
            tweenSequence.AppendInterval(1);
            tweenSequence.Append(
                splashFocus.transform
                    .DOPunchScale(-1 * Vector3.one * 0.25f, 0.2f)
                    .SetEase(Ease.InOutBack)
            );

            tweenSequence.AppendInterval(1);
            tweenSequence.SetLoops(-1, LoopType.Restart).Play();
        }

        public void OnFocusButtonPressed()
        {
            if (hasFocused)
                return;

            hasFocused = true;

            SFXCaller.Play("event:/uiButton");

            tweenSequence?.Kill();

            BrushRevealController splashReveal =
                splashFocus.GetComponent<BrushRevealController>();

            splashReveal.PlayHide(() =>
            {
                splashFocus.SetActive(false);

                gameLogo.SetActive(true);

                StartLoadingNextScene();

                BrushRevealController logoReveal =
                    gameLogo.GetComponent<BrushRevealController>();

                logoReveal.PlayReveal(() =>
                {
                    stampMarkBase.SetActive(true);
                });
            });
        }

        public void OnStampButtonPressed()
        {
            if (!hasFocused || hasStamped)
                return;

            hasStamped = true;

            PlaceStamp();

            StartCoroutine(StampAnimation());
            StartCoroutine(TransitionAfterDelay());

            SFXCaller.Play("event:/uiButton");
        }

        private void PlaceStamp()
        {
            if (stampMark == null || stampRect == null)
                return;

            stampMark.SetActive(true);
            stampMarkBase.SetActive(false);
        }

        private IEnumerator StampAnimation()
        {
            if (stampRect == null)
                yield break;

            float elapsed = 0f;

            while (elapsed < stampAnimDuration)
            {
                elapsed += Time.deltaTime;

                float t = elapsed / stampAnimDuration;
                float scale = Mathf.Lerp(stampScaleStart, stampScaleEnd, t);

                stampRect.localScale = new Vector3(scale, scale, 1f);

                yield return null;
            }

            stampRect.localScale = new Vector3(stampScaleEnd, stampScaleEnd, 1f);
        }

        private void StartLoadingNextScene()
        {
            sceneLoadOperation = SceneManager.LoadSceneAsync(nextSceneName);
            sceneLoadOperation.allowSceneActivation = false;
        }

        private IEnumerator TransitionAfterDelay()
        {
            gameLogo.GetComponent<BrushRevealController>().PlayHide();

            yield return new WaitForSeconds(0.5f);

            stampMark.SetActive(false);

            yield return new WaitForSeconds(delayBeforeTransition);

            sceneLoadOperation.allowSceneActivation = true;
        }

        private void OnDestroy()
        {
            tweenSequence?.Kill();
        }
    }
}