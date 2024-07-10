using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    public static PhysicsController instance;

    [Range(0.001f, 2f)]
    public float targetTimeScale = 1f;
    
    [Range(50, 200)]
    public int targetTimeStepFreqHz = 100;
    public bool syncronizeTimeStepWithScale = true;

    void Awake() {
        instance = this;
    }    

    void Start() {
        SetTimeScale(targetTimeScale);
        SetTimeStep(targetTimeStepFreqHz);
    }

    public void SetTimeStep(float valueHz) {
        float value = 1/ (float) valueHz;

        if (syncronizeTimeStepWithScale) {
            Time.fixedDeltaTime = Time.timeScale * value;
        } else {
            Time.fixedDeltaTime = value;
        }
    }

    public void SetTimeScale(float value) {
        Time.timeScale = value;
    }

    public void Pause() {
        SetTimeScale(0.000001f);
    }

    public void Unpause() {
        SetTimeScale(targetTimeScale);
        SetTimeStep(targetTimeStepFreqHz);
    }
}
