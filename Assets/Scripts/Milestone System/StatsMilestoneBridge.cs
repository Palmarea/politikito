using FMOD;
using Game.Character;
using System.Collections;
using UnityEngine;

namespace Game.Systems.Milestone
{
    public class StatsMilestoneBridge : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TamaCharacterStats CharacterStats;
        [SerializeField] private MilestoneSystem MilestoneSystem;

        public SoundUpdater SoundUpdater;

        private void Start()
        {
            if (CharacterStats != null)
                CharacterStats.OnAllStatsReachedSameLevel += RequestLevelUpMilestone;
        }

        private void RequestLevelUpMilestone(int level)
        {
            if (MilestoneSystem == null) return;

            MilestoneSystem.AdvanceMilestone(level);

            //This is when the character Levels Up. 0=Chibolo, 1=Joven, 2=Adulto, 3=Adulto2 4=TeVas.
            //Screw Transitions
            if (level == 1)
            {
                SoundUpdater.stringVol=0.7f;
                SoundUpdater.leadVol=0.8f;
                SoundUpdater.bassVol=0.8f;
                SoundUpdater.drumVol=0.8f;
            }
            else if (level == 2)
            {
                SoundUpdater.Growth = 2;
            }
            else if (level == 3)
            {
                SoundUpdater.Growth = 4;
            }
            else if (level == 4)
            {
                SoundUpdater.Growth = 5;
            }
        }

        private void OnDestroy()
        {
            if (CharacterStats != null)
                CharacterStats.OnAllStatsReachedSameLevel -= RequestLevelUpMilestone;
        }
    }
}