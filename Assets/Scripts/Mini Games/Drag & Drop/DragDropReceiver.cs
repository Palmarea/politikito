using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Interaction.DragNDrop
{
    public class DragDropReceiver : MonoBehaviour
    {
        public event Action<DragDropObject> OnObjectDropped;
        private bool receiveActive = false;

        public void UpdateActive(bool state)
        {
            receiveActive = state;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!receiveActive) return;
            
            var draggable = other.GetComponent<DragDropObject>();
            if (draggable == null) return;

            if (draggable.AllowToDrop())
            {
                draggable.StopDragging();
                OnObjectDropped?.Invoke(draggable);
            }
        }
    }
}