using UnityEngine;

public class AudioKiller : MonoBehaviour
{
    private void Awake()
    {
        AudioManager[] audioManagers = FindObjectsOfType<AudioManager>();
        if (audioManagers.Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
