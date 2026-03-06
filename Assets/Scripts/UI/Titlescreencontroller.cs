using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

namespace Game.UI
{
    public class TitleScreenController : MonoBehaviour
    {
        [Header("Stamp")]
        [SerializeField] private GameObject stampMark;
        [SerializeField] private RectTransform stampRect;

        [Header("Transition")]
        [SerializeField] private string nextSceneName = "CharacterCreation";
        [SerializeField] private float delayBeforeTransition = 1.2f;

        [Header("Animation")]
        [SerializeField] private float stampScaleStart = 2f;
        [SerializeField] private float stampScaleEnd = 1f;
        [SerializeField] private float stampAnimDuration = 0.15f;

        private bool hasStamped = false;

        private void Start()
        {
            if (stampMark != null)
                stampMark.SetActive(false);
        }

        private void Update()
        {
            if (hasStamped) return;

            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                hasStamped = true;
                PlaceStamp();
                StartCoroutine(StampAnimation());
                StartCoroutine(TransitionAfterDelay());
            }
        }

        private void PlaceStamp()
        {
            if (stampMark == null || stampRect == null) return;

            stampMark.SetActive(true);
            Vector2 mousePos = Mouse.current.position.ReadValue();
            stampRect.position = mousePos;
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
            yield return new WaitForSeconds(delayBeforeTransition);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}