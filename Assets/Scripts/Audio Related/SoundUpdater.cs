using UnityEngine;
using FMODUnity;
public class SoundUpdater : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public FMODUnity.StudioEventEmitter emitter;

    [Range(0f, 1f)]
    public float drumVol;
    [Range(0f, 1f)] 
    public float bassVol;
    [Range(0f, 1f)]
    public float leadVol;
    [Range(0f, 1f)]
    public float stringVol;

    [Range(0, 5)]
    public int Growth;

    void Start()
    {
        if(emitter == null)
        {
            emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        emitter.SetParameter("drumVol", drumVol);
        emitter.SetParameter("bassVol", bassVol);
        emitter.SetParameter("leadVol", leadVol);
        emitter.SetParameter("stringVol", stringVol);
        emitter.SetParameter("Growth", Growth);
    }
}
