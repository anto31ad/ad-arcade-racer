using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EngineSound : MonoBehaviour
{

    public AudioClip clip;
    public float minPitch = 1f;
    public float maxPitch = 2f;
    public float interpolationSpeed = 1;
    public float minVol = 0.5f;
    public float maxVol = 1f;

    private ArcadeCar target;
    private AudioSource audioSource;

    void Start()
    {
        target = GetComponentInParent<ArcadeCar>();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.Play();
    }

    void Update()
    {
        float pitchRange = maxPitch - minPitch;
        float pitchDiff = pitchRange * target.SpeedRatio;
        float targetPitch = pitchDiff  + minPitch;
        audioSource.pitch = Mathf.Lerp(audioSource.pitch, targetPitch, interpolationSpeed * Time.deltaTime);

        //volume control
        float volCoeff = maxVol - minVol;
        audioSource.volume = target.Throttle * volCoeff + minVol;
    }
}
