using Game.Systems.CameraControl;
using Game.Systems.Interaction.DragNDrop;
using System.Collections.Generic;
using UnityEngine;

public class GameHUDFollow : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CameraController CameraController;

    [Header("References")]
    [SerializeField] private List<DragDropObject> DragDropObjects = new List<DragDropObject>();

    private void Awake()
    {
        //transform.parent = CameraController.transform;
        transform.SetParent(CameraController.transform);
    }

    private void UpdateDNDOBjects()
    {
        foreach (var dragDropObject in DragDropObjects)
        {
            dragDropObject.ResetInitialPosition();
        }
    }

    private void OnEnable()
    {
        CameraController.OnArrivedToSection += UpdateDNDOBjects;
    }
    
    private void OnDisable()
    {
        CameraController.OnArrivedToSection -= UpdateDNDOBjects;
    }
}
