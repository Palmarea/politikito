using Game.Managers.Timing;
using Game.Systems.Input;
using Game.Systems.Interaction;
using Game.Systems.Interaction.Detail;
using Game.Systems.Interaction.DragNDrop;
using System;
using UnityEngine;

namespace Game.Managers.Mouse
{
    public class MouseManager : MonoBehaviour, IInterruptible
    {
        public static MouseManager Instance;

        [Header("Configuration")]
        [SerializeField] private LayerMask ClickableLayerMask;

        public event Action OnSimpleClickPerformed;

        private ClickableObject currentHover;
        private bool suscribed = false;
        private bool clickBlocked = false;
        private bool hoverBlocked = false;
        private bool holdingObject = false;

        #region Singleton
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(Instance);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }
        #endregion

        private void Start()
        {
            if (!suscribed)
            {
                InputManager.Instance.OnSelectPerformed += CheckForHitClickableObject;
                suscribed = true;
            }

            CursorManager.Instance.SetCursorState(CursorStateType.DEFAULT);
        }

        private void Update()
        {
            CheckHover();
        }

        private void CheckHover()
        {
            if (hoverBlocked || holdingObject)
                return;

            RaycastHit2D hit = Physics2D.Raycast(
                InputManager.Instance.GetMousePosition(),
                Vector2.zero,
                ClickableLayerMask
            );

            ClickableObject newHover = null;

            if (hit.collider != null)
            {
                newHover = hit.collider.GetComponent<ClickableObject>();
            }

            if (currentHover != null)
                currentHover.SetHover(false);

            currentHover = newHover;

            if (currentHover != null)
            {
                currentHover.SetHover(true);

                bool isDraggable = currentHover.GetComponent<DragDropObject>() != null;

                if (isDraggable)
                {
                    if (currentHover.IsInteractable)
                    {
                        CursorManager.Instance.SetCursorState(CursorStateType.GRABABLE);
                    }
                    else
                    {
                        CursorManager.Instance.SetCursorState(CursorStateType.DEFAULT);
                    }

                }
                else
                {
                    CursorManager.Instance.SetCursorState(CursorStateType.INTEREST);
                }
            }
            else
            {
                CursorManager.Instance.SetCursorState(CursorStateType.DEFAULT);
            }
        }

        private void CheckForHitClickableObject()
        {
            if (clickBlocked) return;

            RaycastHit2D hit = Physics2D.Raycast(
                InputManager.Instance.GetMousePosition(),
                Vector2.zero,
                ClickableLayerMask
            );

            if (hit.collider != null)
            {
                ClickableObject clickable = hit.collider.GetComponent<ClickableObject>();

                if (clickable != null)
                {
                    clickable.Click();

                    DragDropObject dragDrop = hit.collider.GetComponent<DragDropObject>();

                    if (dragDrop != null)
                    {
                        holdingObject = true;
                        CursorManager.Instance.SetCursorState(CursorStateType.HOLD);
                    }
                    else
                    {
                        holdingObject = false;
                        CursorManager.Instance.SetCursorState(CursorStateType.DEFAULT);
                    }
                }
            }
            else
            {
                holdingObject = false;
                CursorManager.Instance.SetCursorState(CursorStateType.DEFAULT);

                OnSimpleClickPerformed?.Invoke();
            }
        }

        public void ReleaseHold()
        {
            holdingObject = false;

            //if (currentHover != null)
            //{
            //    bool isDraggable = currentHover.GetComponent<DragDropObject>() != null;

            //    CursorManager.Instance.SetCursorState(
            //        isDraggable
            //            ? CursorStateType.GRABABLE
            //            : CursorStateType.INTEREST
            //    );

            //    currentHover = null;
            //}
            //else
            //{
                CursorManager.Instance.SetCursorState(CursorStateType.DEFAULT);
            //}
        }

        public void BlockClick(bool state) => clickBlocked = state;

        public void BlockHover(bool state) => hoverBlocked = state;

        public void SetHorizontalRestriction(bool state)
        {
            Cursor.lockState = state ? CursorLockMode.Confined : CursorLockMode.None;
            CursorManager.Instance.SetCursorConstrainedAxis(state);
        }

        public void HandleInterruptionStart(InterruptionType type)
        {
            BlockHover(true);
            CursorManager.Instance.SetCursorState(CursorStateType.DEFAULT);
        }

        public void HandleInterruptionEnd()
        {
            BlockHover(false);
            CursorManager.Instance.SetCursorState(CursorStateType.DEFAULT);
        }

        private void OnEnable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.OnSelectPerformed += CheckForHitClickableObject;
                suscribed = true;
            }

            InterruptionManager.OnInterruptStart += HandleInterruptionStart;
            InterruptionManager.OnInterruptEnd += HandleInterruptionEnd;
        }

        private void OnDisable()
        {
            InputManager.Instance.OnSelectPerformed -= CheckForHitClickableObject;

            InterruptionManager.OnInterruptStart += HandleInterruptionStart;
            InterruptionManager.OnInterruptEnd += HandleInterruptionEnd;
        }
    }
}