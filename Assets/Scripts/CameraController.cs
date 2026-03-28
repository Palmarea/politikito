using Game.Managers.Timing;
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
        [Header("Section Configuration")]
        [SerializeField] private List<CameraSection> Sections = new List<CameraSection>();

        [Header("Parameters")]
        [SerializeField] private float CameraMoveDuration = 0.3f;
        private Vector3 CameraSpeed = Vector3.zero;

        private Camera m_MainCamera;
        private bool isMoving = false;
        private Transform target;
        private CameraSection currentSection;

        void Start()
        {
            m_MainCamera = GetComponent<Camera>();
            currentSection = Sections.FirstOrDefault(section => section.Type == CameraSectionType.MIDDLE);
            m_MainCamera.transform.position = currentSection.MainAnchor.position;
        }

        void LateUpdate()
        {
            if (!isMoving) return;
   
            Vector3 targetPosition = target.position;
            m_MainCamera.transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref CameraSpeed, CameraMoveDuration);

            if (Vector3.Distance(m_MainCamera.transform.position, target.position) <= 0.01f)
            {
                isMoving = false;
                InterruptionManager.Instance.DisableInteruption();
            }
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

        public CameraSectionType GetCurrentCameraSection() => currentSection.Type;
    }
}