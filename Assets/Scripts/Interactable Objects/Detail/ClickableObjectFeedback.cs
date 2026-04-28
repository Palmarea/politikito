using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Game.Systems.Interaction
{
    [RequireComponent(typeof(ClickableObject))]
    public class ClickableObjectFeedback : MonoBehaviour
    {
        private ClickableObject m_clickObj;

        private void Awake()
        {
            m_clickObj = GetComponent<ClickableObject>();
            m_clickObj.OnClicked.AddListener(HandleOnButtonClick);
        }

        private void HandleOnButtonClick()
        {
            transform.DOKill(true);
            transform.DOPunchScale(Vector3.one * -0.2f, 0.25f);
        }

        private void OnDestroy()
        {
            m_clickObj.OnClicked.RemoveListener(HandleOnButtonClick);
        }
    }
}