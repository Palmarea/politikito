using Game.Character;
using Game.Character.StateMachine.States;
using Game.Systems.Interaction.DragNDrop;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems.Minigames
{
    public class ExercisingDDMinigame : DragDropMinigameBase
    {
        [Header("Dependencies")]
        [SerializeField] private TamaCharacterController Character;
        [SerializeField] private Transform DumbbellOriginPoint;
        [SerializeField] private DragDropObject DDObject;
        [SerializeField] private Button FlexButton;

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

            // Personaje huye de la mancuerna
            Character.ChangeState(new FleeState(Character, DumbbellOriginPoint));
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
            FlexButton.onClick.RemoveAllListeners();
            FlexButton.onClick.AddListener(OnClickFlex);

            // Se queda quieto mientras hace ejercicio
            Character.ChangeState(new FrozenState(Character));
        }

        private void OnClickFlex()
        {
            AddProgress(PointsPerClick.GetValue(level));
        }

        private void Cleanup()
        {
            FlexButton.onClick.RemoveListener(OnClickFlex);
            FlexButton.gameObject.SetActive(false);

            DDObject.gameObject.SetActive(true);
            DDObject.BackToOrigin();

            Receiver.UpdateActive(false);
            Receiver.OnObjectDropped -= OnObjectDelivered;

            // Regresa a roam normal
            Character.ChangeState(new RoamState(Character));
        }

        private void OnDestroy()
        {
            if (Receiver != null)
                Receiver.OnObjectDropped -= OnObjectDelivered;
        }
    }
}