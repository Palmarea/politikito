using Game.Managers.Timing;
using Game.Systems.Interaction;
using Game.Systems.Input;
using System;
using UnityEngine;
using Game.Systems.Interaction.Detail;

namespace Game.Managers.Mouse
{
    public class MouseManager : MonoBehaviour
    {
        public static MouseManager Instance;

        [Header("Configuration")]
        [SerializeField] private LayerMask ClickableLayerMask;

        public event Action OnSimpleClickPerformed;
        public event Action<bool> OnOcuppiedStateChanged;

        private ClickableObject currentHover;
        private bool ocuppied = false;
        private bool suscribed = false;
        private bool clickBlocked = false;

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
        }

        private void Update()
        {
            //if (TimeManager.Instance.TimeStop)
            //{
            //}
                if (currentHover != null)
                {
                    currentHover.SetHover(false);
                    currentHover = null;
                }

            CheckHover();
        }

        private void CheckHover()
        {
            if (clickBlocked) return;

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
                    currentHover.SetHover(true);
            }
        }

        private void CheckForHitClickableObject()
        {
            if (clickBlocked) return;

            RaycastHit2D hit = Physics2D.Raycast(InputManager.Instance.GetMousePosition(), Vector2.zero, ClickableLayerMask);

            if (hit.collider != null && !ocuppied)
            {
                ClickableObject clickable = hit.collider.GetComponent<ClickableObject>();
                if (clickable != null)
                {
                    clickable.Click();

                    DetailObject detObj = hit.collider.GetComponent<DetailObject>();

                    if (detObj != null)
                    {
                        //OnClickableExpositionObject?.Invoke(expObj);
                        UpdateOcuppiedState(true);
                    }
                }
            }
            else
            {
                OnSimpleClickPerformed?.Invoke();
            }
        }

        public void UpdateOcuppiedState(bool state)
        {
            ocuppied = state;
            OnOcuppiedStateChanged?.Invoke(ocuppied);
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