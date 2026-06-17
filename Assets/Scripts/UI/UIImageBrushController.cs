using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIImageBrushController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float revealDuration = 0.8f;
    [SerializeField] private float hideDuration = 0.8f;

    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    private static readonly int RevealAmountID = Shader.PropertyToID("_RevealAmount");
    private Image imageComponent;
    private Material materialInstance;
    private Coroutine currentAnimation;

    private void Awake()
    {
        imageComponent = GetComponent<Image>();

        materialInstance = new Material(imageComponent.material);
        imageComponent.material = materialInstance;

        SetRevealAmount(1f);
    }

    private void OnDestroy()
    {
        if (materialInstance != null) Destroy(materialInstance);
    }

    public void Show()
    {
        StopCurrentAnimation();

        imageComponent.enabled = true;

        currentAnimation = StartCoroutine(AnimateReveal(GetRevealAmount(), 1f, revealDuration));
    }

    public void Hide()
    {
        StopCurrentAnimation();

        imageComponent.enabled = true;

        currentAnimation = StartCoroutine(AnimateReveal(GetRevealAmount(), 0f, hideDuration, () => imageComponent.enabled = false));
    }

    public void ShowInstant()
    {
        StopCurrentAnimation();

        imageComponent.enabled = true;

        SetRevealAmount(1f);
    }

    public void HideInstant()
    {
        StopCurrentAnimation();

        SetRevealAmount(0f);

        imageComponent.enabled = false;
    }

    public float GetRevealAmount()
    {
        return materialInstance.GetFloat(RevealAmountID);
    }

    private void SetRevealAmount(float value)
    {
        materialInstance.SetFloat(RevealAmountID, value);
    }

    private IEnumerator AnimateReveal(float startValue, float endValue, float duration, System.Action onComplete = null)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            float eased = easeCurve.Evaluate(t);

            SetRevealAmount(Mathf.Lerp(startValue, endValue, eased));

            yield return null;
        }

        SetRevealAmount(endValue);

        onComplete?.Invoke();

        currentAnimation = null;
    }

    private void StopCurrentAnimation()
    {
        if (currentAnimation == null) return;

        StopCoroutine(currentAnimation);
        currentAnimation = null;
    }
}