using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;
using DG.Tweening;

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

        private void Start()
        {
            if (stampMark != null)
                stampMark.SetActive(false);
            
            if (gameLogo != null)
                gameLogo.SetActive(false);
            
            splashFocus.SetActive(true);
            TweenSplashFocus();
        }

        private void TweenSplashFocus()
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.AppendInterval(1);
            mySequence.Append(splashFocus.transform.DOPunchScale(-1 * Vector3.one * 0.25f, 0.2f).SetEase(Ease.InOutBack));
            mySequence.AppendInterval(1);
            mySequence.SetLoops(-1, LoopType.Restart).Play();
        }

        private void Update()
        {
            if (!hasFocused)
            {
                if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                    FocusScreen();
                
                return;
            }
            
            if (hasStamped) 
                return;

            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                hasStamped = true;
                PlaceStamp();
                StartCoroutine(StampAnimation());
                StartCoroutine(TransitionAfterDelay());
            }
        }

        private void FocusScreen()
        {
            hasFocused = true;
            splashFocus.SetActive(false);
            gameLogo.SetActive(true);
            gameLogo.GetComponent<BrushRevealController>().PlayReveal(() => stampMarkBase.SetActive(true));
        }

        private void PlaceStamp()
        {
            if (stampMark == null || stampRect == null) return;

            stampMark.SetActive(true);
            stampMarkBase.SetActive(false);
        }

        private IEnumerator StampAnimation()
        {
            if (stampRect == null) yield break;

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

        private IEnumerator TransitionAfterDelay()
        {
            gameLogo.GetComponent<BrushRevealController>().PlayHide();
            yield return new WaitForSeconds(delayBeforeTransition);
            stampMark.SetActive(false);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}