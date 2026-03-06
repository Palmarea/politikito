using Game.Character;
using Game.Character.StateMachine.States;
using Game.Systems.Interaction.DragNDrop;
using UnityEngine;
using Game.Systems.Minigames.UI;

namespace Game.Systems.Minigames
{
    public class ExercisingDDMinigame : DragDropMinigameBase
    {
        [Header("Dependencies")]
        [SerializeField] private TamaCharacterController Character;
        [SerializeField] private TamaCharacterAnimation CharacterAnimator;
        [SerializeField] private Transform DumbbellOriginPoint;
        [SerializeField] private DragDropObject DDObject;
        [SerializeField] private HoldButton FlexButton;

        [Header("Minigame Parameters")]
        [SerializeField] private DifficultyValue PointsPerClick;
        [SerializeField] private DifficultyValue ProgressBarDepletitionPerSecond;

        private bool objectDelivered = false;
        private int level;

        protected override void Awake()
        {
            base.Awake();
            FlexButton.gameObject.SetActive(false);
        }

        public override void StartMinigame()
        {
            base.StartMinigame();

            level = CharacterStats.WillPower.Level;

            objectDelivered = false;

            Receiver.UpdateActive(true);
            Receiver.OnObjectDropped -= OnObjectDelivered;
            Receiver.OnObjectDropped += OnObjectDelivered;

            FlexButton.gameObject.SetActive(false);

            Character.ChangeState(new FleeState(Character, DumbbellOriginPoint));

            CharacterAnimator.SetMiniGame(3);
        }

        protected override void UpdateMinigame()
        {
            if (!objectDelivered) return;

            AddProgress(ProgressBarDepletitionPerSecond.GetValue(level) * Time.deltaTime);
        }

        protected override void OnCompleted()
        {
            Cleanup();
            CharacterStats.HandleExercisingAction();
        }

        public override void CloseMinigame()
        {
            base.CloseMinigame();
            Cleanup();
        }

        private void OnObjectDelivered(DragDropObject obj)
        {
            if (obj != DDObject) return;

            objectDelivered = true;

            DDObject.gameObject.SetActive(false);

            Receiver.UpdateActive(false);

            FlexButton.gameObject.SetActive(true);

            FlexButton.OnPressed -= OnFlexPressed;
            FlexButton.OnReleased -= OnFlexReleased;

            FlexButton.OnPressed += OnFlexPressed;
            FlexButton.OnReleased += OnFlexReleased;

            Character.ChangeState(new FrozenState(Character));

            CharacterAnimator.SetWaitingInput(true);
        }

        private void OnFlexPressed()
        {
            CharacterAnimator.SetHoldingWeight(true);
        }

        private void OnFlexReleased()
        {
            AddProgress(PointsPerClick.GetValue(level));
            CharacterAnimator.SetHoldingWeight(false);
        }

        private void Cleanup()
        {
            FlexButton.OnPressed -= OnFlexPressed;
            FlexButton.OnReleased -= OnFlexReleased;

            FlexButton.gameObject.SetActive(false);

            DDObject.gameObject.SetActive(true);
            DDObject.BackToOrigin();

            Receiver.UpdateActive(false);
            Receiver.OnObjectDropped -= OnObjectDelivered;

            Character.ChangeState(new RoamState(Character));

            CharacterAnimator.SetMiniGame(0);
            CharacterAnimator.SetHoldingWeight(false);
            CharacterAnimator.SetWaitingInput(false);
        }

        private void OnDestroy()
        {
            if (Receiver != null)
                Receiver.OnObjectDropped -= OnObjectDelivered;
        }
    }
}