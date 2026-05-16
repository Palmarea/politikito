using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Game.UI
{
    public class PostItToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Config")]
        [SerializeField] private RectTransform PostItRect;
        [SerializeField] private float HiddenOffsetX = 680f; // cuanto se oculta hacia la derecha
        [SerializeField] private float AnimDuration = 0.3f;
        [SerializeField] private float DisableDuration = 1.0f;

        private Vector2 shownPos;
        private Vector2 hiddenPos;
        private Vector2 disablePos;

        private bool isAnimating = false;
        private bool isHidden = true;

        public bool test = false;

        private void Start()
        {
            if (PostItRect == null)
                PostItRect = GetComponent<RectTransform>();
            
            shownPos = PostItRect.anchoredPosition;
            hiddenPos = new Vector2(shownPos.x + HiddenOffsetX, shownPos.y);
            disablePos = new Vector2(hiddenPos.x - 300, hiddenPos.y);
            PostItRect.anchoredPosition = shownPos;
        }

        private void Update()
        {
            if (test)
            {
                test = false;
                OnPostItCompleted();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isAnimating || !isHidden) return;

            isAnimating = true;

            PostItRect.DOAnchorPos(shownPos, AnimDuration)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    isAnimating = false;
                    isHidden = false;
                });
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isAnimating || isHidden) return;

            isAnimating = true;

            PostItRect.DOAnchorPos(hiddenPos, AnimDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    isAnimating = false;
                    isHidden = true;
                });
        }

        public void OnPostItCompleted()
        {
            isAnimating = true;

            PostItRect.DOAnchorPos(disablePos, DisableDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
        }
    }
}