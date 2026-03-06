using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Attach this to a GameObject that has a SpriteRenderer using the
/// "Custom/BrushStrokeReveal" shader.
///
/// Usage:
///   – Enable <see cref="playOnStart"/> to trigger the reveal automatically.
///   – Or call <see cref="PlayReveal"/> / <see cref="PlayHide"/> from code.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class BrushRevealController : MonoBehaviour
{
    public Action Revealed;
    public Action Hidden;
    
    [Header("Animation Settings")]
    [Tooltip("Duration in seconds for the reveal animation.")]
    [SerializeField] private float revealDuration = 1.5f;

    [Tooltip("Duration in seconds for the hide animation.")]
    [SerializeField] private float hideDuration = 1.0f;

    [Tooltip("Ease curve applied to the reveal progress. Use a linear curve for a constant speed, or a slow-in/out curve for a more organic feel.")]
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Space]
    [Tooltip("Play the reveal animation automatically when the object starts.")]
    [SerializeField] private bool playOnStart = true;

    // ── Private state ──────────────────────────────────────────────────────
    private static readonly int RevealAmountID = Shader.PropertyToID("_RevealAmount");

    private SpriteRenderer _spriteRenderer;
    private Material       _materialInstance; // instanced copy so we don't affect other sprites
    private Coroutine      _currentCoroutine;

    // ── Unity lifecycle ────────────────────────────────────────────────────
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // Clone the material so each sprite can have its own _RevealAmount value
        _materialInstance = new Material(_spriteRenderer.sharedMaterial);
        _spriteRenderer.material = _materialInstance;
    }

    private void Start()
    {
        if (playOnStart)
            PlayReveal();
    }

    private void OnDestroy()
    {
        // Clean up the cloned material
        if (_materialInstance != null)
            Destroy(_materialInstance);
    }

    // ── Public API ─────────────────────────────────────────────────────────

    /// <summary>Animates the sprite from invisible to fully visible.</summary>
    public void PlayReveal(Action onReveal = null)
    {
        StopCurrentAnimation();
        // Snap to 0 here at runtime so the sprite is visible during Edit Mode
        // (shader default = 1) and only disappears the moment the animation fires.
        SetRevealAmount(0f);
        _currentCoroutine = StartCoroutine(AnimateReveal(0f, 1f, revealDuration, onReveal));
    }

    /// <summary>Animates the sprite from fully visible to invisible.</summary>
    public void PlayHide(Action onHide = null)
    {
        StopCurrentAnimation();
        _currentCoroutine = StartCoroutine(AnimateReveal(GetRevealAmount(), 0f, hideDuration, onHide));
    }

    /// <summary>Instantly shows the sprite without animation.</summary>
    public void ShowInstant() => SetRevealAmount(1f);

    /// <summary>Instantly hides the sprite without animation.</summary>
    public void HideInstant() => SetRevealAmount(0f);

    /// <summary>Returns the current reveal progress (0 = hidden, 1 = fully shown).</summary>
    public float GetRevealAmount() => _materialInstance.GetFloat(RevealAmountID);

    // ── Internal helpers ───────────────────────────────────────────────────

    private void SetRevealAmount(float value)
    {
        _materialInstance.SetFloat(RevealAmountID, value);
    }

    private void StopCurrentAnimation()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
    }

    private IEnumerator AnimateReveal(float startValue, float endValue, float duration, Action callback = null)
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
        // Ensure we land exactly on the target value
        SetRevealAmount(endValue);
        callback?.Invoke();
        _currentCoroutine = null;
    }
}
