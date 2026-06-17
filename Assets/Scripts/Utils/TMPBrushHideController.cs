using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TMPBrushHideController : MonoBehaviour
{
    public Action Hidden;

    [Header("Animation")]
    [SerializeField] private float hideDuration = 0.8f;

    [SerializeField]
    private AnimationCurve easeCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Brush Settings")]
    [SerializeField, Range(0f, 1f)]
    private float brushiness = 0.9f;

    [SerializeField]
    private float noiseScaleX = 25f;

    [SerializeField]
    private float noiseScaleY = 12f;

    [SerializeField, Range(0.01f, 0.5f)]
    private float edgeWidth = 0.05f;

    [SerializeField, Range(0f, 3f)]
    private float strokeAngle = 1.5f;

    [Header("Debug")]
    [SerializeField]
    private bool debugMode;

    [SerializeField]
    [Range(0f, 1f)]
    private float debugReveal = 1f;

    private static readonly int RevealAmountID =
        Shader.PropertyToID("_RevealAmount");

    private static readonly int BrushinessID =
        Shader.PropertyToID("_Brushiness");

    private static readonly int NoiseScaleXID =
        Shader.PropertyToID("_NoiseScaleX");

    private static readonly int NoiseScaleYID =
        Shader.PropertyToID("_NoiseScaleY");

    private static readonly int EdgeWidthID =
        Shader.PropertyToID("_EdgeWidth");

    private static readonly int StrokeAngleID =
        Shader.PropertyToID("_StrokeAngle");

    private TMP_Text textComponent;
    private Material materialInstance;
    private Coroutine currentAnimation;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();

        materialInstance = new Material(textComponent.fontSharedMaterial);
        textComponent.fontMaterial = materialInstance;

        ApplyBrushSettings();
        SetRevealAmount(1f);
    }

    private void Update()
    {
        if (!debugMode)
            return;

        ApplyBrushSettings();
        SetRevealAmount(debugReveal);
    }

    private void OnValidate()
    {
        if (materialInstance == null)
            return;

        ApplyBrushSettings();

        if (debugMode)
            SetRevealAmount(debugReveal);
    }

    private void OnDestroy()
    {
        if (materialInstance != null)
            Destroy(materialInstance);
    }

    private void ApplyBrushSettings()
    {
        materialInstance.SetFloat(
            BrushinessID,
            brushiness);

        materialInstance.SetFloat(
            NoiseScaleXID,
            noiseScaleX);

        materialInstance.SetFloat(
            NoiseScaleYID,
            noiseScaleY);

        materialInstance.SetFloat(
            EdgeWidthID,
            edgeWidth);

        materialInstance.SetFloat(
            StrokeAngleID,
            strokeAngle);
    }

    private void SetRevealAmount(float value)
    {
        materialInstance.SetFloat(
            RevealAmountID,
            value);
    }

    public float GetRevealAmount()
    {
        return materialInstance.GetFloat(
            RevealAmountID);
    }

    public void ShowInstant()
    {
        StopCurrentAnimation();

        textComponent.enabled = true;

        SetRevealAmount(1f);
    }

    public void HideInstant()
    {
        StopCurrentAnimation();

        SetRevealAmount(0f);

        textComponent.enabled = false;
    }

    public void HideBrush(Action onHidden = null)
    {
        StopCurrentAnimation();

        textComponent.enabled = true;

        SetRevealAmount(1f);

        currentAnimation = StartCoroutine(
            AnimateHide(() =>
            {
                textComponent.enabled = false;

                Hidden?.Invoke();
                onHidden?.Invoke();
            }));
    }

    private IEnumerator AnimateHide(Action callback)
    {
        float elapsed = 0f;

        while (elapsed < hideDuration)
        {
            elapsed += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    elapsed / hideDuration);

            float eased =
                easeCurve.Evaluate(t);

            SetRevealAmount(
                Mathf.Lerp(
                    1f,
                    0f,
                    eased));

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