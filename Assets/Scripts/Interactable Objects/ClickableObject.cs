using UnityEngine;
using UnityEngine.Events;

namespace Game.Systems.Interaction
{
    public class ClickableObject : MonoBehaviour
    {
        [Header("Events")]
        [Tooltip("Events called when clicked on object")]
        [SerializeField] private UnityEvent OnClicked;
        [SerializeField] private UnityEvent OnHover;
        [SerializeField] private UnityEvent OnOffHover;

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
            OnClicked?.Invoke();
        }

        public void Hover()
        {
            OnHover?.Invoke();
        }

        public void OffHover()
        {
            OnOffHover?.Invoke();
        }

        public virtual void SetHover(bool state)
        {
            if (sr == null) return;

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
    }
}
