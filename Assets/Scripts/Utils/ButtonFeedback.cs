using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class ButtonFeedback : MonoBehaviour
{
    private Button button;
    private RectTransform rectTransform;

    private void Awake()
    {
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
        button.onClick.AddListener(HandleOnButtonClick);
    }

    private void HandleOnButtonClick()
    {
        rectTransform.DOKill(true);
        rectTransform.DOPunchScale(Vector3.one * -0.2f, 0.25f);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(HandleOnButtonClick);
    }
}
