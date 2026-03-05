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
        [SerializeField] private Transform WateringOriginPoint;
        [SerializeField] private DragDropObject DDObject;

        [Header("Parameters")]
        [SerializeField] private float RayLength = 3f;
        [SerializeField] private LayerMask InteractableMasks;

        [Header("Minigame Parameters")]
        [SerializeField] private DifficultyValue PointsPerFrame;
        [SerializeField] private DifficultyValue ProgressBarDepletitionPerFrame;

        private int level;

        public override void StartMinigame()
        {
            base.StartMinigame();

            // Empieza huyendo del agua
            level = CharacterStats.Charisma.Level;
            Debug.Log(level);
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
            }

            AddProgress(ProgressBarDepletitionPerFrame.GetValue(level) * Time.deltaTime);
            Debug.Log(ProgressBarDepletitionPerFrame.GetValue(level));
        }

        protected override void OnCompleted()
        {
            Cleanup();
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

            // Regresa al comportamiento normal
            Character.ChangeState(new RoamState(Character));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(WateringOriginPoint.position, Vector2.down * RayLength);
        }
    }
}
