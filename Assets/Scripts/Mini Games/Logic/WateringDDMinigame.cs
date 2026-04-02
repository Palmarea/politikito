using Game.Character;
using Game.Character.StateMachine.States;
using Game.Systems.Interaction.DragNDrop;
using UnityEngine;

namespace Game.Systems.Minigames
{
    public class WateringDDMinigame : DragDropMinigameBase
    {
        [Header("Dependencies")]
        [SerializeField] private TamaCharacterController Character;
        [SerializeField] private TamaCharacterAnimation CharacterAnimator;
        [SerializeField] private Transform WateringOriginPoint;

        [Header("Parameters")]
        [SerializeField] private float RayLength = 3f;
        [SerializeField] private LayerMask InteractableMasks;

        [Header("Minigame Parameters")]
        [SerializeField] private DifficultyValue PointsPerFrame;
        [SerializeField] private DifficultyValue ProgressBarDepletitionPerFrame;

        private int level;

        public override void StartMinigame()
        {
            if (!CheckForMinigameStart())
                return;

            level = CharacterStats.Charisma.Level;
            CharacterAnimator.SetMiniGame(1);
            Character.ChangeState(new FleeState(Character, WateringOriginPoint));
        }

        protected override void UpdateMinigame()
        {
            RaycastHit2D hit = Physics2D.Raycast(
                WateringOriginPoint.position,
                Vector2.down,
                RayLength,
                InteractableMasks);

            if (hit)
            {
                AddProgress(PointsPerFrame.GetValue(level) * Time.deltaTime);
                CharacterAnimator.SetReceivingWater(true);
            }
            else
            {
                CharacterAnimator.SetReceivingWater(false);
            }

            AddProgress(ProgressBarDepletitionPerFrame.GetValue(level) * Time.deltaTime);
        }

        protected override void OnCompleted()
        {
            if (!Context.TutorialData.IsTutorialComplete())
            {
                Context.TutorialData.CompleteTutorialStep(TutorialData.WATERING_CAN_STEP_INDEX);
            }
            
            Cleanup();
            SFXCaller.Play("event:/actionWater");
            CharacterStats.HandleWateringAction();
        }

        public override void CloseMinigame()
        {
            base.CloseMinigame();
            Cleanup();
        }

        private void Cleanup()
        {
            DDObject.StopDragging();

            Receiver.UpdateActive(false);

            Character.ChangeState(new RoamState(Character));

            CharacterAnimator.SetMiniGame(0);
            CharacterAnimator.SetReceivingWater(false);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(WateringOriginPoint.position, Vector2.down * RayLength);
        }
    }
}
