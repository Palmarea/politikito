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
            Debug.Log($"[RECEIVER] Active = {receiveActive}");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TryReceive(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TryReceive(other);
        }

        //private void OnTriggerEnter2D(Collider2D other)
        //{
        //    if (!receiveActive) return;

        //    var draggable = other.GetComponent<DragDropObject>();
        //    if (draggable == null) return;

        //    if (draggable.AllowToDrop())
        //    {
        //        draggable.StopDragging();
        //        OnObjectDropped?.Invoke(draggable);
        //    }
        //}

        //private void OnTriggerStay2D(Collider2D other)
        //{
        //    if (!receiveActive) return;

        //    var draggable = other.GetComponent<DragDropObject>();
        //    if (draggable == null) return;

        //    if (draggable.AllowToDrop())
        //    {
        //        draggable.StopDragging();
        //        OnObjectDropped?.Invoke(draggable);
        //        receiveActive = false;
        //    }
        //}

        private void TryReceive(Collider2D other)
        {
            if (!receiveActive) return;

            var draggable = other.GetComponent<DragDropObject>();
            if (draggable == null) return;

            if (!draggable.AllowToDrop()) return;

            draggable.StopDragging();
            OnObjectDropped?.Invoke(draggable);

            // Evita múltiples triggers en el mismo frame / contacto
            receiveActive = false;
        }
    }
}