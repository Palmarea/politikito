using UnityEngine;

public class AnchorFollow : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private Transform TargetFollow;

    private void Update()
    {
        transform.position = TargetFollow.position;
    }
}
