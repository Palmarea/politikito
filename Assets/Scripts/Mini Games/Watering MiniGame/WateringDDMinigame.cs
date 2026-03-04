using Game.Character;
using Game.Systems.Interaction.DragNDrop;
using UnityEngine;

namespace Game.Systems.Minigames
{
    public class WateringDDMinigame : DragDropMinigameBase
    {
        [Header("Dependencies")]
        [SerializeField] private Transform WateringOriginPoint;
        [SerializeField] private DragDropObject DDObject;
        [SerializeField] private TamaCharacterMovement CharacterMovement;

        [Header("Parameters")]
        [SerializeField] private float RayLength = 3f;
        [SerializeField] private LayerMask InteractableMasks;

        [Header("Minigame Parameters")]
        [SerializeField] private DifficultyValue PointsPerFrame;
        [SerializeField] private DifficultyValue CharacterMovementSpeed;
        [SerializeField] private DifficultyValue ProgressBarDepletitionPerFrame;

        public override void StartMinigame()
        {
            base.StartMinigame();
            CharacterMovement.ForceFlee(WateringOriginPoint);
            CharacterMovement.SetSpeedMultiplier(CharacterMovementSpeed.GetValue(0));
        }

        protected override void UpdateMinigame()
        {
            RaycastHit2D hit = Physics2D.Raycast(WateringOriginPoint.position, Vector2.down, RayLength, InteractableMasks);

            if (hit)
            {
                AddProgress(PointsPerFrame.GetValue(0) * Time.deltaTime);
            }

            AddProgress(ProgressBarDepletitionPerFrame.GetValue(0) * Time.deltaTime);
        }

        protected override void OnCompleted()
        {
            DDObject.StopDragging();
            CharacterMovement.StopFlee();
            CharacterMovement.SetSpeedMultiplier(1f);
            Receiver.UpdateActive(false);
        }

        public override void CloseMinigame()
        {
            base.CloseMinigame();
            DDObject.StopDragging();
            CharacterMovement.StopFlee();
            CharacterMovement.SetSpeedMultiplier(1f);
            Receiver.UpdateActive(false);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(WateringOriginPoint.position, Vector2.down * RayLength);
        }
    }
}
