using UnityEngine;
using FMODUnity;

namespace Game.Systems.Audio
{
    [CreateAssetMenu(menuName = "Audio/FMOD Event")]
    public class FMODEventSO : ScriptableObject
    {
        public EventReference eventReference;
    }
}