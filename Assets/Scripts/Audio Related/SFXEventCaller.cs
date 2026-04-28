using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SFXCaller : MonoBehaviour
{
    // =============================
    // One-shot SFX
    // =============================
    public static void Play(string eventPath)
    {
        RuntimeManager.PlayOneShot(eventPath);
    }

    // =============================
    // Looping SFX
    // =============================
    public static EventInstance PlayLoop(string eventPath)
    {
        EventInstance instance = RuntimeManager.CreateInstance(eventPath);
        instance.start();
        return instance;
    }

    // =============================
    // Stop Loop
    // =============================
    public static void Stop(EventInstance instance, bool allowFadeout = true)
    {
        if (!instance.isValid()) return;

        instance.stop(allowFadeout ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE);
        instance.release();
    }

    // =============================
    // Set Parameter
    // =============================
    public static void SetParameter(EventInstance instance, string paramName, float value)
    {
        if (!instance.isValid()) return;

        instance.setParameterByName(paramName, value);
    }
}