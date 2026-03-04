using Game.Character;
using Game.Character.StateMachine.States;
using Game.Systems.Interaction.DragNDrop;
using UnityEngine;

namespace Game.Systems.Minigames
{
    public class FeedingDDMinigame : DragDropMinigameBase
    {
        [Header("Dependencies")]
        [SerializeField] private TamaCharacterController Character;
        [SerializeField] private DragDropObject DDObject;

        [Header("Difficulty")]
        [SerializeField] private DifficultyValue ProgressPerFeed;
        [SerializeField] private DifficultyValue ProgressBarDepletitionPerFrame;
        [SerializeField] private DifficultyValue TimeRunning;
        [SerializeField] private DifficultyValue TimeStopped;
        [SerializeField] private DifficultyValue MouthOpenDuration;
        [SerializeField] private DifficultyValue MouthOpenCooldown;
        [SerializeField] private DifficultyValue MouthOpenRepeats;

        private float stateTimer;
        private float cooldownTimer;

        private int mouthOpensRemaining;
        private int level = 0;

        private enum FeedingPhase
        {
            Running,
            Waiting,
            MouthOpen
        }

        private FeedingPhase currentPhase;

        public override void StartMinigame()
        {
            base.StartMinigame();

            Receiver.OnObjectDropped -= OnFoodGiven;
            Receiver.OnObjectDropped += OnFoodGiven;
            Receiver.UpdateActive(false);

            level = 0;

            StartRunningPhase();
        }

        protected override void UpdateMinigame()
        {
            stateTimer -= Time.deltaTime;

            switch (currentPhase)
            {
                case FeedingPhase.Running:
                    if (stateTimer <= 0f)
                        StartWaitingPhase();
                    break;

                case FeedingPhase.Waiting:
                    HandleWaitingPhase();
                    break;
            }

            AddProgress(ProgressBarDepletitionPerFrame.GetValue(level) * Time.deltaTime);
        }

        #region Phases

        private void StartRunningPhase()
        {
            currentPhase = FeedingPhase.Running;

            Receiver.UpdateActive(false);

            Character.ChangeState(new FleeState(Character, DDObject.transform));

            stateTimer = TimeRunning.GetValue(level);
        }

        private void StartWaitingPhase()
        {
            currentPhase = FeedingPhase.Waiting;

            Character.ChangeState(new FrozenState(Character));

            mouthOpensRemaining = Mathf.RoundToInt(MouthOpenRepeats.GetValue(level));
            cooldownTimer = 0f;

            stateTimer = TimeStopped.GetValue(level);
        }

        private void HandleWaitingPhase()
        {
            if (mouthOpensRemaining <= 0)
            {
                if (stateTimer <= 0f)
                    StartRunningPhase();
                return;
            }

            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                OpenMouth();
            }

            if (stateTimer <= 0f)
            {
                StartRunningPhase();
            }
        }

        private void OpenMouth()
        {
            currentPhase = FeedingPhase.MouthOpen;

            Receiver.UpdateActive(true);

            Character.ChangeState(new MouthOpenState(
                Character,
                MouthOpenDuration.GetValue(level),
                OnMouthClosed));

            cooldownTimer = MouthOpenCooldown.GetValue(level);
        }

        private void OnMouthClosed()
        {
            Receiver.UpdateActive(false);

            mouthOpensRemaining--;

            currentPhase = FeedingPhase.Waiting;
        }

        #endregion

        #region Feeding

        private void OnFoodGiven(DragDropObject obj)
        {
            if (currentPhase != FeedingPhase.MouthOpen)
                return;

            AddProgress(ProgressPerFeed.GetValue(level));

            DDObject.BackToOrigin();
        }

        #endregion

        protected override void OnCompleted()
        {
            Cleanup();
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

            Character.ChangeState(new RoamState(Character));
        }

        private void OnDestroy()
        {
            if (Receiver != null)
                Receiver.OnObjectDropped -= OnFoodGiven;
        }
    }
}