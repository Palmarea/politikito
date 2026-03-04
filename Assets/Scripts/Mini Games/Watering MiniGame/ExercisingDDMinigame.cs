using Game.Character;
using Game.Systems.Interaction.DragNDrop;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems.Minigames
{
    public class ExercisingDDMinigame : DragDropMinigameBase
    {
        [Header("Dependencies")]
        [SerializeField] private Transform DumbbellOriginPoint;
        [SerializeField] private DragDropObject DDObject;
        [SerializeField] private TamaCharacterMovement CharacterMovement;

        [SerializeField] private Button FlexButton;

        [Header("Minigame Parameters")]
        [SerializeField] private DifficultyValue PointsPerClick;
        [SerializeField] private DifficultyValue ProgressBarDepletitionPerSecond;

        private bool objectDelivered = false;

        protected override void Awake()
        {
            base.Awake();

            FlexButton.gameObject.SetActive(false);
        }

        public override void StartMinigame()
        {
            base.StartMinigame();

            Receiver.UpdateActive(true);
            Receiver.OnObjectDropped += OnObjectDelivered;
            objectDelivered = false;
            FlexButton.gameObject.SetActive(false);
            CharacterMovement.ForceFlee(DumbbellOriginPoint);
        }

        protected override void UpdateMinigame()
        {
            if (!objectDelivered) return;

            AddProgress(ProgressBarDepletitionPerSecond.GetValue(0) * Time.deltaTime);
        }

        protected override void OnCompleted()
        {
            FlexButton.onClick.RemoveListener(OnClickFlex);
            FlexButton.gameObject.SetActive(false);
            
            DDObject.gameObject.SetActive(true);
            DDObject.BackToOrigin();
            
            CharacterMovement.SetSpeedMultiplier(1f);
        }

        public override void CloseMinigame()
        {
            base.CloseMinigame();
            
            FlexButton.onClick.RemoveListener(OnClickFlex);
            FlexButton.gameObject.SetActive(false);

            DDObject.gameObject.SetActive(true);
            DDObject.BackToOrigin();
            
            CharacterMovement.SetSpeedMultiplier(1f);
        }

        private void OnObjectDelivered(DragDropObject obj)
        {
            if (obj != DDObject) return;

            objectDelivered = true;

            DDObject.gameObject.SetActive(false);
            FlexButton.gameObject.SetActive(true);

            FlexButton.onClick.AddListener(OnClickFlex);
            CharacterMovement.StopFlee();
            CharacterMovement.SetSpeedMultiplier(0f);
            Receiver.UpdateActive(false);
        }

        private void OnClickFlex()
        {
            AddProgress(PointsPerClick.GetValue(0));
        }

        private void OnDestroy()
        {
            if (Receiver != null)
                Receiver.OnObjectDropped -= OnObjectDelivered;
        }
    }
}