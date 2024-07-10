using UnityEngine;

public class RayWheelEffects : MonoBehaviour
{

    [Header("Audio")]
    public AudioSource audioSource;
    [Range (0f, 1f)]
    public float maxVolume = 1f;
    public float fadeSpeed = 10f;

    [Range (0f, 1f)]
    public float minAudioSlip = 0.1f;
    [Range (0f, 1f)]
    public float maxAudioSlip = 0.4f;
    public float peakAudioVolumeSpeed = 10f; 

    [Header ("Tarmac VFX")]
    public ParticleSystem smokeSource;
    public float maxSmokeRate = 50f;
    [Range (0f, 1f)]
    public float minSlip = 0.1f;
    [Range (0f, 1f)]
    public float maxSlip = 0.4f;
    public float peakSlipEffectSpeed = 20f;

    [Header ("Grass VFX")]
    public ParticleSystem grassSource;
    public float maxGrassRate = 50f;
    public float peakGrassEffectSpeed = 20f;


    [Header ("Debug")]
    private RayWheel target;
    private Rigidbody _rigidbody;


    void Start()
    {
        _rigidbody = GetComponentInParent<Rigidbody>();
        target = GetComponent<RayWheel>();
    
        smokeSource.Play();
        grassSource.Play();

        audioSource.loop = true;
        audioSource.volume = 0f;
        audioSource.Play();
    }


    void Update()
    {
        var smokeEmission = smokeSource.emission;
        var grassEmission = grassSource.emission;

        //resets every value, assuming the wheel is not grounded
        audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, fadeSpeed);
        smokeEmission.rateOverDistance = 0;
        grassEmission.rateOverDistance = 0;

        if (!target.IsGrounded) {
            return;
        }

        Vector3 hitPoint = target.ContactPatchPosition;
        //tarmac
        smokeSource.transform.position = hitPoint;
        audioSource.transform.position = hitPoint;
        //grass
        grassSource.transform.position = hitPoint;

        switch (target.HitColliderTag) {
            case "Tarmac":
                audioSource.volume = ComputeTarmacVolume();
                smokeEmission.rateOverDistance = ComputeTarmacSmokeRate();
                break;
            case "Grass": 
                grassEmission.rateOverDistance = ComputeGrassEffectRate();
                break;
            default:
                break;
        }
    }

    private float ComputeTarmacVolume() {
        float slip = Mathf.Abs(target.SlipAngleRatio);
        float x = Mathf.Clamp01(slip - minSlip);
        float volumeCoeff = maxVolume / (maxAudioSlip - minAudioSlip);

        float speedFactor = Mathf.Clamp01(_rigidbody.velocity.magnitude / peakAudioVolumeSpeed);
        return Mathf.Clamp01(speedFactor * volumeCoeff * x);
    }

    private float ComputeTarmacSmokeRate() {
        float slip = Mathf.Abs(target.SlipAngleRatio);
        float x = Mathf.Clamp01(slip - minSlip);

        float smokeRateCoeff = maxSmokeRate / (maxSlip - minSlip);
        float speedFactor = Mathf.Clamp01(_rigidbody.velocity.magnitude / peakAudioVolumeSpeed);
        float targetRate = speedFactor * smokeRateCoeff * x;
        return Mathf.Clamp(targetRate, 0f, maxSmokeRate);
    }

    private float ComputeGrassEffectRate() {
        return Mathf.Clamp01(_rigidbody.velocity.magnitude / peakGrassEffectSpeed) * peakGrassEffectSpeed;
    }
}
