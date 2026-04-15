using System.Collections;
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

    private Coroutine fadeCoroutine;

    void Start()
    {
        if(emitter == null)
        {
            emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        }
    }

    public void FadeOutVolumes(float duration)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOutCoroutine(duration));
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float startDrum = drumVol;
        float startBass = bassVol;
        float startLead = leadVol;
        float startString = stringVol;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = duration > 0f ? elapsed / duration : 1f;
            drumVol = Mathf.Lerp(startDrum, 0f, t);
            bassVol = Mathf.Lerp(startBass, 0f, t);
            leadVol = Mathf.Lerp(startLead, 0f, t);
            stringVol = Mathf.Lerp(startString, 0f, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        drumVol = 0f;
        bassVol = 0f;
        leadVol = 0f;
        stringVol = 0f;
        fadeCoroutine = null;
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
