using System.Collections;
using System.Collections.Generic;
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
        public List<Milestone> CreateAllMilestones(TextAsset milestoneJSON)
        {
            Milestones deserialized =
                JsonUtility.FromJson<Milestones>(milestoneJSON.text);

            return new List<Milestone>(deserialized.milestones);
        }
    }
}