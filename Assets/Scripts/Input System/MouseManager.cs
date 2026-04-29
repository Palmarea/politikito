using Game.Systems.Input;
using Game.Systems.Interaction;
using Game.Systems.Interaction.Detail;
using Game.Systems.Interaction.DragNDrop;
using System;
using UnityEngine;

namespace Game.Managers.Mouse
{
    public class MouseManager : MonoBehaviour
    {
        public static MouseManager Instance;

        [Header("Configuration")]
        [SerializeField] private LayerMask ClickableLayerMask;

        public event Action OnSimpleClickPerformed;

        private ClickableObject currentHover;
        private bool ocuppied = false;
        private bool suscribed = false;
        private bool clickBlocked = false;
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
            if (clickBlocked || holdingObject)
                return;

            RaycastHit2D hit = Physics2D.Raycast(
                InputManager.Instance.GetMousePosition(),
                Vector2.zero,
                ClickableLayerMask
            );

            ClickableObject newHover = null;

            if (hit.collider != null && !ocuppied)
            {
                newHover = hit.collider.GetComponent<ClickableObject>();
            }

            if (newHover != currentHover)
            {
                if (currentHover != null)
                    currentHover.SetHover(false);

                currentHover = newHover;

                if (currentHover != null)
                {
                    currentHover.SetHover(true);

                    bool isDraggable = currentHover.GetComponent<DragDropObject>() != null;

                    CursorManager.Instance.SetCursorState(
                        isDraggable
                            ? CursorStateType.GRABABLE
                            : CursorStateType.INTEREST
                    );
                }
                else
                {
                    CursorManager.Instance.SetCursorState(CursorStateType.DEFAULT);
                }
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

            if (hit.collider != null && !ocuppied)
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

                    DetailObject detObj = hit.collider.GetComponent<DetailObject>();
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

            if (currentHover != null)
            {
                bool isDraggable = currentHover.GetComponent<DragDropObject>() != null;

                CursorManager.Instance.SetCursorState(
                    isDraggable
                        ? CursorStateType.GRABABLE
                        : CursorStateType.INTEREST
                );
            }
            else
            {
                CursorManager.Instance.SetCursorState(CursorStateType.DEFAULT);
            }
        }

        public void UpdateOcuppiedState(bool state)
        {
            ocuppied = state;
            clickBlocked = !state;
        }

        public void SetHorizontalRestriction(bool state)
        {
            Cursor.lockState = state ? CursorLockMode.Confined : CursorLockMode.None;

            if (!state)
            {
                UnityEngine.InputSystem.Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2, Screen.height / 2));
            }

            Cursor.visible = !state;
        }

        private void OnEnable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.OnSelectPerformed += CheckForHitClickableObject;
                suscribed = true;
            }
        }

        private void OnDisable()
        {
            InputManager.Instance.OnSelectPerformed -= CheckForHitClickableObject;
        }
    }
}