using Game.Systems.Input;
using UnityEngine;

namespace Game.UI
{
    public class CursorUI : MonoBehaviour
    {        
        private RectTransform m_cursorTransform;
        private RectTransform m_canvasRectTransform;
        private Canvas m_parentCanvas;
        private Camera m_canvasCamera;
        private CanvasGroup m_canvasGroup;

        private bool limitedMovement = false;

        private void Awake()
        {
            m_cursorTransform = GetComponent<RectTransform>();
            m_canvasGroup = GetComponent<CanvasGroup>();
            m_parentCanvas = GetComponentInParent<Canvas>();

            if (m_parentCanvas != null)
            {
                m_canvasRectTransform = m_parentCanvas.GetComponent<RectTransform>();
                m_canvasCamera = m_parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : m_parentCanvas.worldCamera;
            }
        }

        private void Update()
        {
            UpdateCursorPosition();
        }

        private void UpdateCursorPosition()
        {
            if (m_cursorTransform == null ||
                m_canvasRectTransform == null)
                return;

            Vector2 mousePosition =
                InputManager.Instance.GetRawMousePosition();

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                m_canvasRectTransform,
                mousePosition,
                m_canvasCamera,
                out var localPoint))
            {
                if (limitedMovement)
                {
                    localPoint.y = 175f;
                }
                
                m_cursorTransform.anchoredPosition = localPoint;
            }
        }

        public void UpdateCursorVisibility(bool visibility)
        {
            m_canvasGroup.alpha = visibility ? 1 : 0;
        }

        public void UpdateCursorMoveAxis(bool constrained)
        {
            limitedMovement = constrained ? true : false;
        }
    }
}