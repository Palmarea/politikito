using Game.Character;
using Game.Managers.Timing;
using Game.Systems.CameraControl;
using Game.Systems.Interaction.DragNDrop;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Minigames
{
    [System.Serializable]
    public class MinigameDefinition
    {
        public DragDropMinigameBase DragDropMinigame;
        public DragDropObject DragDropObject;
        public GameObject DragDropButton;
    }
    
    public class MinigameManager : MonoBehaviour
    {
        public static MinigameManager Instance;

        public List<MinigameDefinition> MinigameDefinitions = new List<MinigameDefinition>();

        [Header("Dependencies")]
        [SerializeField] private TamaCharacterMovement CharacterMovement;
        [SerializeField] private CameraController CameraController;

        public bool IsMinigameActive { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        public void StartMinigame(DragDropMinigameBase ddminigame)
        {
            IsMinigameActive = true;

            foreach (MinigameDefinition definition in MinigameDefinitions)
            {
                if (definition.DragDropMinigame != ddminigame)
                {
                    definition.DragDropMinigame.enabled = false;
                    definition.DragDropObject.gameObject.SetActive(false);
                    definition.DragDropButton.gameObject.SetActive(false);
                }
            }

            float dir = 0;
            switch (CameraController.GetCurrentCameraSection())
            {
                case CameraSectionType.LEFT:
                    dir = -1;
                    break;
                case CameraSectionType.MIDDLE: 
                    dir = 0; 
                    break;
                case CameraSectionType.RIGHT: 
                    dir = 1; 
                    break;
            }

            float newOrigin = CameraController.ForceMove(CharacterMovement.transform);
            CharacterMovement.SetReducedBounds(dir, newOrigin);
            InterruptionManager.Instance.EnableInterruption(InterruptionType.MINIGAME);
        }

        public void EndMinigame()
        {
            IsMinigameActive = false;

            foreach (MinigameDefinition definition in MinigameDefinitions)
            {
                definition.DragDropMinigame.enabled = true;
                definition.DragDropObject.gameObject.SetActive(true);
                definition.DragDropButton.gameObject.SetActive(true);
            }

            CharacterMovement.ResetBounds();
            CameraController.ResetForced();
        }
    }
}