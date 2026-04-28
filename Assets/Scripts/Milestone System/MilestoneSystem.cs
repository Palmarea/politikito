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
        public event Action<Milestone> OnForcedMilestoneReached;

        private bool forced = false;

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
            level = forced ? level + 1 : level;
            
            if (!MilestoneDictionary.TryGetValue(level, out Milestone milestone))
                return;

            OnMilestoneReached?.Invoke(milestone);
        }

        public void ForceAdvance(int level)
        {
            forced = true;
            level = forced ? level + 1 : level;

            if (!MilestoneDictionary.TryGetValue(level, out Milestone milestone))
                return;

            OnForcedMilestoneReached?.Invoke(milestone);
        }
    }
}