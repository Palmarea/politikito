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
        [SerializeField] private TextAsset MilestoneJSON;
        [SerializeField] private int MaxAchievementList = 5;

        private Dictionary<int, Milestone> MilestoneDictionary = new();
        public event Action<Milestone> OnMilestoneReached;

        private void Awake()
        {
            var milestones = Creator.CreateAllMilestones(MilestoneJSON);

            foreach (var milestone in milestones)
            {
                MilestoneDictionary[milestone.level] = milestone;
            }
            
            Context.TutorialData.CompleteTutorialStep(TutorialData.CHARACTER_STEP_INDEX);
        }

        public void AdvanceMilestone(int level)
        {
            Debug.Log("CALLED FOR LEVEL " + level);
            if (!MilestoneDictionary.TryGetValue(level, out Milestone milestone))
                return;

            Debug.Log(milestone.ToString());

            OnMilestoneReached?.Invoke(milestone);
        }
    }
}