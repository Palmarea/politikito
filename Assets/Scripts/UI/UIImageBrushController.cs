using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIImageBrushController : MonoBehaviour
{
    public Action Hidden;

    [Header("Animation")]
    [SerializeField] private float hideDuration = 0.8f;

    [SerializeField]
    private AnimationCurve easeCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    private static readonly int RevealAmountID =
        Shader.PropertyToID("_RevealAmount");

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
        if (materialInstance != null)
            Destroy(materialInstance);
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

    public void HideBrush(Action onHidden = null)
    {
        StopCurrentAnimation();

        imageComponent.enabled = true;
        SetRevealAmount(1f);

        currentAnimation = StartCoroutine(
            AnimateHide(() =>
            {
                imageComponent.enabled = false;

                Hidden?.Invoke();
                onHidden?.Invoke();
            }));
    }

    private void SetRevealAmount(float value)
    {
        materialInstance.SetFloat(
            RevealAmountID,
            value);
    }

    private IEnumerator AnimateHide(Action callback)
    {
        float elapsed = 0f;

        while (elapsed < hideDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / hideDuration);
            float eased = easeCurve.Evaluate(t);

            SetRevealAmount(
                Mathf.Lerp(1f, 0f, eased));

            yield return null;
        }

        SetRevealAmount(0f);

        callback?.Invoke();

        currentAnimation = null;
    }

    private void StopCurrentAnimation()
    {
        if (currentAnimation == null)
            return;

        StopCoroutine(currentAnimation);
        currentAnimation = null;
    }
}