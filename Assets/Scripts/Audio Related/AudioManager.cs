using System.Collections;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    [SerializeField] private StudioEventEmitter musicEmitter;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(string eventPath)
    {
        musicEmitter.EventReference = RuntimeManager.PathToEventReference(eventPath);
        musicEmitter.Play();
    }

    public void StopMusic()
    {
        musicEmitter.Stop();
    }
}