using Game.Managers.Timing;
using Game.Systems.CameraControl;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CameraPresenter : MonoBehaviour, IInterruptible
    {
        [Header("Dependencies")]
        [SerializeField] private CameraController Controller;
        
        [Header("References")]
        [SerializeField] private Button LeftButton;
        [SerializeField] private Button RightButton;

        public void HandleInterruptionStart(InterruptionType type)
        {
            LeftButton.interactable = false;
            LeftButton.gameObject.SetActive(false);
            RightButton.interactable = false;
            RightButton.gameObject.SetActive(false);
        }

        public void HandleInterruptionEnd()
        {
            LeftButton.gameObject.SetActive(true);
            LeftButton.interactable = true;
            RightButton.gameObject.SetActive(true);
            RightButton.interactable = true;
        }

        private void OnEnable()
        {
            InterruptionManager.OnInterruptStart += HandleInterruptionStart;
            InterruptionManager.OnInterruptEnd += HandleInterruptionEnd;

            if (Controller != null)
            {
                LeftButton.onClick.AddListener(Controller.MoveLeft);
                RightButton.onClick.AddListener(Controller.MoveRight);
            }
        }

        private void OnDisable()
        {
            InterruptionManager.OnInterruptStart -= HandleInterruptionStart;
            InterruptionManager.OnInterruptEnd -= HandleInterruptionEnd;

            LeftButton.onClick.RemoveListener(Controller.MoveLeft);
            RightButton.onClick.RemoveListener(Controller.MoveRight);
        }
    }
}