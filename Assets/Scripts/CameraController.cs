using Game.Character;
using Game.Managers.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Systems.CameraControl
{
    public enum CameraSectionType
    {
        LEFT,
        MIDDLE,
        RIGHT
    }

    [System.Serializable]
    public class CameraSection
    {
        public CameraSectionType Type;
        public Transform MainAnchor;
        public Transform LeftAnchor;
        public Transform RightAnchor;
    }

    public class CameraController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TamaCharacterStats CharacterStats;

        [Header("Section Configuration")]
        [SerializeField] private List<CameraSection> Sections = new();

        [Header("Parameters")]
        [SerializeField] private float CameraMoveDuration = 0.3f;
        [SerializeField] private float MinHorizontalBound;
        [SerializeField] private float MaxHorizontalBound;
        private Vector3 CameraSpeed = Vector3.zero;

        [Header("Zoom")]
        [SerializeField] private float DefaultZoom = 5f;
        private float zoomVelocity;
        private float targetZoom;
        private float zoomSmoothTime = 0.3f;

        public event Action OnArrivedToSection;
        public event Action OnArrivedToForcedSection;
        public event Action<CameraSection> OnSectionChanged;

        private Camera m_MainCamera;
        private bool isMoving = false;
        private bool isForced = false;
        private bool levelUpRequested = false;
        private Transform target;
        private Vector3 m_forcedTarget;
        private CameraSection currentSection;

        void Start()
        {
            m_MainCamera = GetComponent<Camera>();
            currentSection = Sections.FirstOrDefault(section => section.Type == CameraSectionType.MIDDLE);
            m_MainCamera.transform.position = currentSection.MainAnchor.position;

            targetZoom = DefaultZoom;
            m_MainCamera.orthographicSize = DefaultZoom;
        }

        void LateUpdate()
        {
            HandleMovement();
            HandleZoom();
        }

        #region MOVEMENT

        private void HandleMovement()
        {
            if (!isMoving) return;

            if (isForced)
            {
                MoveCamera(m_forcedTarget);

                if (Vector3.Distance(m_MainCamera.transform.position, m_forcedTarget) <= 0.01f)
                {
                    isMoving = false;
                    isForced = false;

                    OnArrivedToForcedSection?.Invoke();
                }
            }
            else
            {
                MoveCamera(target.position);

                if (Vector3.Distance(m_MainCamera.transform.position, target.position) <= 0.01f)
                {
                    isMoving = false;

                    if (!levelUpRequested)
                    {
                        OnArrivedToSection?.Invoke();
                        OnSectionChanged?.Invoke(currentSection);
                        InterruptionManager.Instance.DisableInteruption();
                    }
                    else
                    {
                        levelUpRequested = false;
                    }
                }
            }
        }

        private void MoveCamera(Vector3 targetPosition)
        {
            m_MainCamera.transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref CameraSpeed,
                CameraMoveDuration
            );
        }

        public void MoveLeft()
        {
            MoveToSide(currentSection.LeftAnchor);
        }

        public void MoveRight()
        {
            MoveToSide(currentSection.RightAnchor);
        }

        private void MoveToSide(Transform compareAnchor)
        {
            if (compareAnchor == null) return;

            CameraSection newSection = Sections.FirstOrDefault(section => section.MainAnchor == compareAnchor);

            if (newSection == null) return;

            target = newSection.MainAnchor;
            currentSection = newSection;
            isMoving = true;

            InterruptionManager.Instance.EnableInterruption(InterruptionType.TRANSITION);
        }

        public float ForceMove(Transform forcedTarget, bool outBounds = false)
        {
            isForced = true;
            isMoving = true;

            Vector3 desired = Vector3.zero;

            if (!outBounds)
            {
                desired = new Vector3(
                    forcedTarget.position.x,
                    m_MainCamera.transform.position.y,
                    m_MainCamera.transform.position.z
                );
            }
            else
            {
                desired = new Vector3(
                    forcedTarget.position.x,
                    forcedTarget.position.y,
                    m_MainCamera.transform.position.z
                );
            }

            float offset = ClampForcedX(ref desired);

            m_forcedTarget = desired;

            return desired.x;
        }

        public void ResetForced(bool backToLast = false)
        {
            isForced = false;
            isMoving = false;

            if (backToLast)
            {
                float minDistance = 1000f;
                CameraSection cs = currentSection;

                foreach (var section in Sections)
                {
                    float distance = Vector3.Distance(m_forcedTarget, section.MainAnchor.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        cs = section;
                    }
                }

                m_forcedTarget = Vector3.zero;

                if (cs == null) return;

                target = cs.MainAnchor;
                currentSection = cs;
            }

            isMoving = true;

            InterruptionManager.Instance.EnableInterruption(InterruptionType.TRANSITION);
        }

        public CameraSectionType GetCurrentCameraSection() => currentSection.Type;

        #endregion

        #region ZOOM

        private void HandleZoom()
        {
            m_MainCamera.orthographicSize = Mathf.SmoothDamp(
                m_MainCamera.orthographicSize,
                targetZoom,
                ref zoomVelocity,
                zoomSmoothTime
            );
        }

        public void SetZoom(float zoomSize, float duration)
        {
            targetZoom = zoomSize;
            zoomSmoothTime = duration;
        }

        public void SetZoomImmediate(float zoomSize)
        {
            targetZoom = zoomSize;
            zoomVelocity = 0f;
            m_MainCamera.orthographicSize = zoomSize;
        }

        public void ResetZoom(float duration)
        {
            targetZoom = DefaultZoom;
            zoomSmoothTime = duration;
        }

        #endregion

        #region HELPERS

        private float ClampForcedX(ref Vector3 targetPosition)
        {
            float originalX = targetPosition.x;

            float halfWidth = m_MainCamera.orthographicSize * m_MainCamera.aspect;

            float leftEdge = targetPosition.x - halfWidth;
            float rightEdge = targetPosition.x + halfWidth;

            float offset = 0f;

            if (leftEdge < MinHorizontalBound)
            {
                offset = MinHorizontalBound - leftEdge;
            }
            else if (rightEdge > MaxHorizontalBound)
            {
                offset = MaxHorizontalBound - rightEdge;
            }

            if (Mathf.Abs(offset) > Mathf.Epsilon)
            {
                targetPosition.x += offset;
            }
            else
            {
                targetPosition.x = originalX;
            }

            return offset;
        }

        private void HandleLevelUp(TamaStat stat)
        {
            levelUpRequested = true;
        }

        private void OnEnable()
        {
            if (CharacterStats == null) return;

            CharacterStats.OnStatLevelUp += HandleLevelUp;
        }

        private void OnDisable()
        {
            CharacterStats.OnStatLevelUp -= HandleLevelUp;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            float height = 20f;

            Gizmos.DrawLine(
                new Vector3(MinHorizontalBound, -height, 0),
                new Vector3(MinHorizontalBound, height, 0)
            );

            Gizmos.DrawLine(
                new Vector3(MaxHorizontalBound, -height, 0),
                new Vector3(MaxHorizontalBound, height, 0)
            );
        }

        #endregion
    }
}