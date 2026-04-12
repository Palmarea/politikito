using Game.Character;
using Game.Character.StateMachine.States;
using Game.Managers.Mouse;
using Game.Systems.Interaction.DragNDrop;
using UnityEngine;

namespace Game.Systems.Minigames
{
    public class FeedingDDMinigame : DragDropMinigameBase
    {
        [Header("Dependencies")]
        [SerializeField] private TamaCharacterController Character;
        [SerializeField] private TamaCharacterAnimation CharacterAnimator;

        [Header("Difficulty")]
        [SerializeField] private DifficultyValue ProgressPerFeed;
        [SerializeField] private DifficultyValue ProgressBarDepletitionPerFrame;

        [Header("Mouth Behavior")]
        [SerializeField] private DifficultyValue MouthOpenDuration;
        [SerializeField] private DifficultyValue MouthOpenCooldown;

        [Header("Movement")]
        [SerializeField] private DifficultyValue RunningSpeed;

        private float cooldownTimer;
        private int level = 0;

        private bool isMouthOpen = false;

        public override void StartMinigame()
        {
            if (!CheckForMinigameStart())
                return;

            Receiver.OnObjectDropped -= OnFoodGiven;
            Receiver.OnObjectDropped += OnFoodGiven;
            Receiver.UpdateActive(false);

            level = CharacterStats.Wisdom.Level;

            CharacterAnimator.SetMiniGame(2);
            MouseManager.Instance.SetHorizontalRestriction(true);

            StartRunning();
        }

        protected override void UpdateMinigame()
        {
            cooldownTimer -= Time.deltaTime;

            if (!isMouthOpen && cooldownTimer <= 0f)
            {
                OpenMouth();
            }

            AddProgress(ProgressBarDepletitionPerFrame.GetValue(level) * Time.deltaTime);
        }

        #region Core Behavior

        private void StartRunning()
        {
            isMouthOpen = false;

            Character.ChangeState(new BounceState(Character));

            // ⚡ Ajustar velocidad dinámica
            Character.MovementHandler.SetSpeedMultiplier(RunningSpeed.GetValue(level));

            cooldownTimer = MouthOpenCooldown.GetValue(level);
        }

        private void OpenMouth()
        {
            isMouthOpen = true;

            Receiver.UpdateActive(true);

            Character.ChangeState(new MouthOpenState(
                Character,
                MouthOpenDuration.GetValue(level),
                OnMouthClosed));

            CharacterAnimator.SetMouthOpen(true);
        }

        private void OnMouthClosed()
        {
            Receiver.UpdateActive(false);

            CharacterAnimator.SetMouthOpen(false);

            StartRunning();
        }

        #endregion

        #region Feeding

        private void OnFoodGiven(DragDropObject obj)
        {
            if (!isMouthOpen)
                return;

            AddProgress(ProgressPerFeed.GetValue(level));

            SFXCaller.Play("event:/actionBite");

            DDObject.BackToOrigin();
        }

        #endregion

        protected override void OnCompleted()
        {
            if (!Context.TutorialData.IsTutorialComplete())
            {
                Context.TutorialData.CompleteTutorialStep(TutorialData.COOKIE_STEP_INDEX);
            }

            Cleanup();
            CharacterStats.HandleFeedingAction();
        }

        public override void CloseMinigame()
        {
            base.CloseMinigame();
            Cleanup();
        }

        private void Cleanup()
        {
            Receiver.UpdateActive(false);
            Receiver.OnObjectDropped -= OnFoodGiven;

            DDObject.StopDragging();
            DDObject.BackToOrigin();

            Character.ChangeState(new RoamState(Character));

            CharacterAnimator.SetMiniGame(0);
            CharacterAnimator.SetMouthOpen(false);

            MouseManager.Instance.SetHorizontalRestriction(false);
        }

        private void OnDestroy()
        {
            if (Receiver != null)
                Receiver.OnObjectDropped -= OnFoodGiven;
        }
    }
}