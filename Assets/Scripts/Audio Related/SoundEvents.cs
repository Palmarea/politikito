using FMOD.Studio;
using System;
using UnityEngine;

namespace Game.Systems.Audio
{
    public static class AudioEvents
    {
        public static Action<FMODEventSO, Vector3> OnPlayOneShot;
        public static Action<FMODEventSO, Action<EventInstance>> OnPlayControlled;
    }
}