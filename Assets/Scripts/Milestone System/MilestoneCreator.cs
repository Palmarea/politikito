using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Milestone
{
    public class MilestoneCreator : MonoBehaviour
    {
        public Dictionary<int, Milestone> CreateAllMilestones(MilestoneDatabaseSO database)
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