using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Character.Visual
{
    public enum TamaType
    {
        MASCULINE,
        FEMALE,
        BUSINESS
    }
    
    [System.Serializable]
    public class TamaVisual
    {
        public TamaType CharacterType;
        public List<AnimatorOverrideController> EvolutionsAOC;
    }
    
    public class TamaCharacterVisual : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TamaCharacterAnimation AnimationHandler; 

        [Header("Sprite References")]
        [SerializeField] private List<TamaVisual> CharacterLevelAOC= new List<TamaVisual>();
        private AnimatorOverrideController currentCharacterAnimator;

        public void RequestVisualEvolution(TamaType characterType, int level)
        {
            var aoc = GetCharacterAOC(characterType, level);

            if (aoc == null)
            {
                Debug.LogError($"No AnimatorOverrideController found for {characterType} at level {level}");
                return;
            }

            currentCharacterAnimator = aoc;
            AnimationHandler.RequestAnimatorOverride(currentCharacterAnimator);
        }

        private AnimatorOverrideController GetCharacterAOC(TamaType characterType, int level)
        {
            var characterData = CharacterLevelAOC
                .FirstOrDefault(c => c.CharacterType == characterType);

            if (characterData == null)
            {
                Debug.LogError($"Character type {characterType} not found.");
                return null;
            }

            if (characterData.EvolutionsAOC == null || characterData.EvolutionsAOC.Count == 0)
            {
                Debug.LogError($"No evolutions defined for {characterType}");
                return null;
            }

            // Clamp level to valid range
            level = Mathf.Clamp(level, 0, characterData.EvolutionsAOC.Count - 1);

            return characterData.EvolutionsAOC[level];
        }
    }
}
