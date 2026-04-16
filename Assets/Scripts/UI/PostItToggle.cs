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

        private Vector2 shownPos;
        private Vector2 hiddenPos;
        private bool isShown = false;

        private void Start()
        {
            if (PostItRect == null)
                PostItRect = GetComponent<RectTransform>();
            
            shownPos = PostItRect.anchoredPosition;
            hiddenPos = new Vector2(shownPos.x + HiddenOffsetX, shownPos.y);
            PostItRect.anchoredPosition = hiddenPos;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PostItRect.DOAnchorPos(shownPos, AnimDuration).SetEase(Ease.OutBack);
            isShown = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PostItRect.DOAnchorPos(hiddenPos, AnimDuration).SetEase(Ease.InBack);
            isShown = false;
        }
    }
}