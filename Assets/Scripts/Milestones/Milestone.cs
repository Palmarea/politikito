using System.Collections;
using UnityEngine;

namespace Game.Systems.Milestone
{
    [System.Serializable]
    public class Milestone
    {
        public int level;
        public string title;
        public string description;

        public override string ToString()
        {
            return $"Milestone\nLevel: {level}\nTitle: {title}\nDescription: {description}";
        }
    }
}