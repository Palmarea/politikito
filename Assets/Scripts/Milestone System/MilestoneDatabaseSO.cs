using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Milestone
{
    [CreateAssetMenu(fileName = "MilestoneDB", menuName = "Game/Milestone Database")]
    public class MilestoneDatabaseSO : ScriptableObject
    {
        public List<Milestone> MilestoneDB = new List<Milestone>();
    }
}