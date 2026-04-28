using UnityEngine;

public class AudioKiller : MonoBehaviour
{
    private void Awake()
    {
        AudioManager[] audioManagers = FindObjectsByType<AudioManager>(FindObjectsSortMode.None);
        if (audioManagers.Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
