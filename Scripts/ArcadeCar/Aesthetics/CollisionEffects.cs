using UnityEngine;

public class CollisionEffects : MonoBehaviour
{
    public AudioSource hitAudioSource;
    [Header("Hard body")]
    public AudioClip hardHitClip;
    public float hardHitMinIntensityNewton = 50f;
    public float hardHitMaxIntensityNewton = 300f;

    [Header("Soft body")]
    public AudioClip softHitClip;
    public float softHitMaxIntensityNewton = 100f;


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with " + collision.collider.tag);

        float intensity = collision.impulse.magnitude;        
        float hitVolume = 0f;
        AudioClip clip = null;

        switch (collision.collider.tag) {
            case "Softbody":
                hitVolume = Mathf.Clamp01(intensity / softHitMaxIntensityNewton);
                clip = softHitClip;
                break;
            case "Hardbody":
                if (intensity > hardHitMinIntensityNewton) {
                    hitVolume = Mathf.Clamp01(intensity / hardHitMaxIntensityNewton);
                    clip = hardHitClip;
                }
                break;
            default:
                break;
        }
        if (clip) {
            hitAudioSource.volume = hitVolume;
            hitAudioSource.pitch = Random.Range(0.5f, 1.5f);
            hitAudioSource.PlayOneShot(clip);
        }
    }

    void OnCollisionStay(Collision collision) {
        Debug.Log("Ongoing collision with " + collision.collider.tag);
    }
}
