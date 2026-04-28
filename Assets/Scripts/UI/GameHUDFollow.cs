using Game.Systems.CameraControl;
using UnityEngine;

public class GameHUDFollow : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CameraController CameraController;

    private Vector3 initialLocalPosition = Vector3.zero;

    private void Awake()
    {
        StartFollowing();
    }

    public void StartFollowing()
    {
        transform.SetParent(CameraController.transform);
        
        if (initialLocalPosition != Vector3.zero)
        {
            transform.localPosition = initialLocalPosition;
        }
        else
        {
            initialLocalPosition = transform.localPosition;
        }
    }

    public void StopFollowing()
    {
        transform.SetParent(null);
    }
}
