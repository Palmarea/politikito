using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Interaction.DragNDrop
{
    public class DragDropReceiver : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private List<Collider2D> Colliders;

        [Header("Layers")]
        [SerializeField] private LayerMask DraggableLayer;
        [SerializeField] private LayerMask InteractableLayer;

        public event Action<DragDropObject> OnObjectDropped;
        private bool receiveActive = false;

        public void UpdateActive(bool state)
        {
            receiveActive = state;

            //int draggableLayer = Mathf.RoundToInt(Mathf.Log(DraggableLayer.value, 2));
            //int interactableLayer = Mathf.RoundToInt(Mathf.Log(InteractableLayer.value, 2));

            //foreach (var col in Colliders)
            //{
            //    int playerLayer = col.gameObject.layer;

            //    // Si receiveActive es false -> ignoramos colisión
            //    Physics2D.IgnoreLayerCollision(playerLayer, draggableLayer, !receiveActive);
            //    Physics2D.IgnoreLayerCollision(playerLayer, interactableLayer, !receiveActive);
            //}
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!receiveActive) return;
            
            var draggable = other.GetComponent<DragDropObject>();
            if (draggable == null) return;

            draggable.StopDragging();
            OnObjectDropped?.Invoke(draggable);
        }
    }
}