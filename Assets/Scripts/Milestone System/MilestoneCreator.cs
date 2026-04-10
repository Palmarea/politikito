using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Systems.Milestone
{
    [System.Serializable]
    public class Milestones
    {
        public Milestone[] milestones;
    }

    public class MilestoneCreator : MonoBehaviour
    {
        public Dictionary<int ,Milestone> CreateAllMilestones(MilestoneDatabaseSO database)
        {
            Dictionary<int, Milestone> dict = new();

            foreach (Milestone milestone in database.MilestoneDB)
            {
                if (!dict.ContainsKey(milestone.level))
                {
                    dict[milestone.level] = milestone;
                }
            }

            return dict;
        }
    }
}