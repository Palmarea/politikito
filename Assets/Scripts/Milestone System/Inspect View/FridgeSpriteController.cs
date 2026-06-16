using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Milestone
{
    public class FridgeSpriteController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<Sprite> Phases = new();

        private SpriteRenderer spriteRenderer;
        private int currentPhase = 0;
        private bool maxAdvance = false;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Phases[currentPhase];
        }

        public void AdvancePhase()
        {
            if (maxAdvance) return;
            
            currentPhase++;

            if (currentPhase >= 0 && currentPhase <= Phases.Count - 1)
            {
                spriteRenderer.sprite = Phases[currentPhase];
            }
            else
            {
                maxAdvance = true;
                return;
            }
        }
    }
}