using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Game.Systems.Audio
{
    public class SoundManager : MonoBehaviour
    {
        private void OnEnable()
        {
            AudioEvents.OnPlayOneShot += PlayOneShot;
            AudioEvents.OnPlayControlled += PlayControlled;
        }

        private void OnDisable()
        {
            AudioEvents.OnPlayOneShot -= PlayOneShot;
            AudioEvents.OnPlayControlled -= PlayControlled;
        }

        private void PlayOneShot(FMODEventSO sound, Vector3 worldPos)
        {
            RuntimeManager.PlayOneShot(sound.eventReference, worldPos);
        }

        private void PlayControlled(FMODEventSO sound, System.Action<EventInstance> onCreated)
        {
            var instance = RuntimeManager.CreateInstance(sound.eventReference);
            onCreated?.Invoke(instance);
            instance.start();
        }
    }
}