using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Milestone
{
    public class MilestoneSystem : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private MilestoneCreator Creator;

        [Header("Parameters")]
        [SerializeField] private MilestoneDatabaseSO Database;

        private Dictionary<int, Milestone> MilestoneDictionary = new();
        public event Action<Milestone> OnMilestoneReached;

        private void Awake()
        {
            MilestoneDictionary = Creator.CreateAllMilestones(Database);
        }

        private void Start()
        {
            Context.TutorialData.CompleteTutorialStep(TutorialData.CHARACTER_STEP_INDEX);
        }

        public void AdvanceMilestone(int level)
        {
            if (!MilestoneDictionary.TryGetValue(level, out Milestone milestone))
                return;

            OnMilestoneReached?.Invoke(milestone);
        }
    }
}