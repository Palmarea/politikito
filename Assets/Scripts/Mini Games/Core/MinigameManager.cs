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
        }
    }
}