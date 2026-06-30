using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Systems.CameraControl;
using UnityEngine;
using Game.Managers.Timing;
using System;
using Game.Systems.Interaction.DragNDrop;

public class InitialSequence : MonoBehaviour
{
    [Serializable]
    private class PopUpHideObj
    {
        public GameObject Go;
        public string SoundSfx;
    }
    
    [Header("Camera")]
    [SerializeField] private CameraController CameraController;
    [SerializeField] private float InitialZoom = 5.39f;
    [SerializeField] private float ZoomDuration = 2f;

    [Header("Tiko")]
    [SerializeField] private GameObject RealCharacter;
    [SerializeField] private Transform CharacterTransform;
    [SerializeField] private Transform TargetPosition;
    [SerializeField] private float MoveDuration = 1.5f;

    [Header("Animation")]
    [SerializeField] private Animator Animator;
    [SerializeField] private string SaluteStateName = "TikoInitialSalute";

    [Header("Reveal Objects")]
    [SerializeField] private List<PopUpHideObj> ObjectsToReveal = new();
    [SerializeField] private float DelayBetweenObjects = 0.15f;
    [SerializeField] private float PopDuration = 0.3f;
    [SerializeField] private float PopOvershoot = 1.15f;

    private void Start()
    {
        RealCharacter.SetActive(false);
        
        foreach (var obj in ObjectsToReveal)
        {
            obj.Go.SetActive(false);
        }
        
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        InterruptionManager.Instance.EnableInterruption(InterruptionType.CINEMATIC);
        
        CameraController.SetZoomImmediate(0f);
        CameraController.SetZoom(InitialZoom, ZoomDuration);

        yield return new WaitForSeconds(ZoomDuration);

        yield return MoveToTarget();

        yield return SaluteRoutine();

        CharacterTransform.gameObject.SetActive(false);
        RealCharacter.SetActive(true);

        InterruptionManager.Instance.DisableInteruption();

        Debug.Log("Secuencia terminada.");
    }

    private IEnumerator MoveToTarget()
    {
        yield return CharacterTransform.DOMove(TargetPosition.position, MoveDuration)
            .SetEase(Ease.InOutSine)
            .WaitForCompletion();
    }

    private IEnumerator SaluteRoutine()
    {
        Animator.SetTrigger("Salute");

        Coroutine revealRoutine = StartCoroutine(RevealObjects());

        yield return WaitForAnimation(SaluteStateName);

        yield return revealRoutine;
    }

    private IEnumerator RevealObjects()
    {
        foreach (PopUpHideObj item in ObjectsToReveal)
        {
            Transform t = item.Go.transform;

            DragDropPhysicObject physic = item.Go.GetComponentInChildren<DragDropPhysicObject>();

            GameObject visualClone = null;
            Transform originalParent = null;

            if (physic != null)
            {
                visualClone = CreateVisualClone(physic);

                originalParent = physic.transform.parent;

                physic.transform.SetParent(null, true);
                physic.gameObject.SetActive(false);
            }

            Vector3 originalScale = t.localScale;

            item.Go.SetActive(true);

            t.localScale = Vector3.zero;

            Sequence seq = DOTween.Sequence();

            seq.Append(
                t.DOScale(originalScale * PopOvershoot, PopDuration * 0.6f)
                    .SetEase(Ease.OutBack));

            seq.Append(
                t.DOScale(originalScale, PopDuration * 0.4f)
                    .SetEase(Ease.OutQuad)).OnComplete(() =>
                    {
                        if (physic != null)
                        {
                            Destroy(visualClone);

                            physic.gameObject.SetActive(true);
                            physic.transform.SetParent(originalParent, false);
                            physic.SnapToAnchor();
                            physic.transform.localScale = Vector3.one;
                        }
                    });

            SFXCaller.Play("event:/" + item.SoundSfx);

            yield return new WaitForSeconds(DelayBetweenObjects);
        }
    }

    private IEnumerator WaitForAnimation(string stateName, int layer = 0)
    {
        yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName));

        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo state = Animator.GetCurrentAnimatorStateInfo(layer);

            return !state.IsName(stateName) || state.normalizedTime >= 1f;
        });
    }

    private GameObject CreateVisualClone(DragDropPhysicObject physic)
    {
        GameObject clone = new GameObject(physic.name + "_Visual");

        clone.transform.SetParent(physic.transform.parent, false);
        clone.transform.localPosition = physic.transform.localPosition;
        clone.transform.localRotation = physic.transform.localRotation;
        clone.transform.localScale = physic.transform.localScale;

        SpriteRenderer source = physic.GetComponent<SpriteRenderer>();
        SpriteRenderer sr = clone.AddComponent<SpriteRenderer>();

        sr.sprite = source.sprite;
        sr.color = source.color;
        sr.flipX = source.flipX;
        sr.flipY = source.flipY;
        sr.sortingLayerID = source.sortingLayerID;
        sr.sortingOrder = source.sortingOrder;
        sr.drawMode = source.drawMode;
        sr.maskInteraction = source.maskInteraction;

        return clone;
    }
}