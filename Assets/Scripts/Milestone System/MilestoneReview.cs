using Game.Managers.Timing;
using Game.Systems.CameraControl;
using Game.UI;
using UnityEngine;

namespace Game.Systems.Milestone
{
    public class MilestoneReview : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private CameraController CamController;
        [SerializeField] private GameHUDFollow GHFollow;
        [SerializeField] private TransitionHandler TransitionHandler;

        [Header("References")]
        [SerializeField] private Transform ViewAnchorPoint;
        [SerializeField] private Transform ZoomTargetPoint;

        [Header("Zoom Settings")]
        [SerializeField] private float ZoomSize = 2f;
        [SerializeField] private float ZoomDuration = 0.3f;

        #region VIEW CHANGE

        public void RequestViewChange()
        {
            InterruptionManager.Instance.EnableInterruption(InterruptionType.TRANSITION);

            TransitionHandler.OnTransBlackEnded += OnBlackEnded_View;
            TransitionHandler.RequestTransitionTB();

            CamController.ForceMove(ZoomTargetPoint, true);
            CamController.SetZoom(ZoomSize, ZoomDuration);
        }

        private void OnBlackEnded_View()
        {
            TransitionHandler.OnTransBlackEnded -= OnBlackEnded_View;

            CamController.ResetZoom(ZoomDuration);

            CamController.OnArrivedToForcedSection += OnArrived_View;
            CamController.ForceMove(ViewAnchorPoint, true);
        }

        private void OnArrived_View()
        {
            CamController.OnArrivedToForcedSection -= OnArrived_View;

            TransitionHandler.RequestTransitionTT();
            GHFollow.StopFollowing();
        }

        #endregion

        #region RESET

        public void RequestViewReset()
        {
            TransitionHandler.OnTransBlackEnded += OnBlackEnded_Reset;
            TransitionHandler.RequestTransitionTB();
        }

        private void OnBlackEnded_Reset()
        {
            TransitionHandler.OnTransBlackEnded -= OnBlackEnded_Reset;

            CamController.SetZoomImmediate(ZoomSize);

            CamController.OnArrivedToForcedSection += OnArrived_Reset;

            CamController.ForceMove(ZoomTargetPoint, true);
        }

        private void OnArrived_Reset()
        {
            CamController.OnArrivedToSection -= OnArrived_Reset;

            TransitionHandler.OnTransTransparentEnded += OnTransparentEnded;

            TransitionHandler.RequestTransitionTT();
        }

        private void OnTransparentEnded()
        {
            TransitionHandler.OnTransTransparentEnded -= OnTransparentEnded;

            CamController.ResetZoom(ZoomDuration);
            CamController.ResetForced(true);

            GHFollow.StartFollowing();
        }

        #endregion
    }
}