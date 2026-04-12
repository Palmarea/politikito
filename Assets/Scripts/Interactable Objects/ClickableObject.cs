using Game.Managers.Timing;
using Game.Systems.Milestone;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Systems.Interaction
{
    public class ClickableObject : MonoBehaviour, IInterruptible
    {
        [Header("Events")]
        [Tooltip("Events called when clicked on object")]
        public UnityEvent OnClicked;
        public UnityEvent OnHover;
        public UnityEvent OnOffHover;

        private bool interactable = true;
        
        public bool IsInteractable
        {
            get { return interactable; }
            set { interactable = value; }
        }

        protected SpriteRenderer sr;
        private MaterialPropertyBlock mpb;

        private static readonly int HoverID = Shader.PropertyToID("_Hover");

        protected virtual void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            mpb = new MaterialPropertyBlock();
        }

        public void Click()
        {
            if (!interactable) return;
            OnClicked?.Invoke();
        }

        public void Hover()
        {
            if (!interactable) return;
            OnHover?.Invoke();
        }

        public void OffHover()
        {
            if (!interactable) return;
            OnOffHover?.Invoke();
        }

        public virtual void SetHover(bool state)
        {
            if (sr == null) return;

            state = state && interactable;

            if (state)
            {
                Hover();
            }
            else
            {
                OffHover();
            }

            sr.GetPropertyBlock(mpb);
            mpb.SetFloat(HoverID, state ? 1f : 0f);
            sr.SetPropertyBlock(mpb);
        }

        public void HandleInterruptionStart(InterruptionType type)
        {
            interactable = type == InterruptionType.OUT ? true : false;
        }

        public void HandleInterruptionEnd()
        {
            interactable = true;
        }

        private void OnEnable()
        {
            InterruptionManager.OnInterruptStart += HandleInterruptionStart;
            InterruptionManager.OnInterruptEnd += HandleInterruptionEnd;
        }

        private void OnDisable()
        {
            InterruptionManager.OnInterruptStart -= HandleInterruptionStart;
            InterruptionManager.OnInterruptEnd -= HandleInterruptionEnd;
        }
    }
}
