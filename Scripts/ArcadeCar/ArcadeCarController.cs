using UnityEngine;

public class ArcadeCarController : MonoBehaviour
{
    public static ArcadeCarController instance;


    [Header ("Steering")]
    
    public float steerSensitivityMaxSpeedKph = 50f;
    public float minSteerAngle = 10f;

    [Header ("Vertical Controls")]
    
    public float stationaryThresholdSpeedKph = 3f;

    private bool _controlsEnabled;
    private ArcadeCar target;

    public bool ControlsEnabled {
        get => _controlsEnabled;
        set {
            _controlsEnabled = value;
        }
    }


    void Awake() {
        instance = this;
        target = GameObject.FindWithTag("Player").GetComponent<ArcadeCar>();
    }

    void Update()
    {
        if (_controlsEnabled) {
            ProcessSteeringInput();
            ProcessVertInput();
        }
    }

    private void ProcessSteeringInput() {
        float input = Input.GetAxis("Horizontal");

        float _curSpeedKph = target.SpeedKph;
        float maxSteerAngle = target.maxSteeringAngle;

        float maxSteerAngleFromSpeed = maxSteerAngle * (1 - _curSpeedKph/steerSensitivityMaxSpeedKph);
        maxSteerAngleFromSpeed = Mathf.Clamp(maxSteerAngleFromSpeed, 0, maxSteerAngle);
    
        float counterSlipAngleDeg = Mathf.Abs(target.SlipAngleDeg);

        float targetSteerAngle = Mathf.Max(maxSteerAngleFromSpeed, counterSlipAngleDeg);

        float ratio = Mathf.Clamp(targetSteerAngle, minSteerAngle, maxSteerAngle) / maxSteerAngle;
        target.SteerInput = input * ratio;
    }

    private void ProcessVertInput() {
        float rawInput = Input.GetAxis("Vertical");

        float input;
        if (target.SpeedKph > stationaryThresholdSpeedKph) {
            input = target.ReverseOn ? -rawInput : rawInput;
        } else {
            if (rawInput < 0f) {
                target.ReverseOn = true;
                input = -rawInput;
            } else {
                target.ReverseOn = false;
                input = rawInput;
            }
        }

        target.Brake = Mathf.Clamp(-input, 0f, 1f);
        target.Throttle = Mathf.Clamp(input, 0f, 1f);
    }

    public void StopTheCar() {
        //disables player inputs
        _controlsEnabled = false;
        //stops the car
        target.Brake = 1f;
        target.Throttle = 0f;
        target.SteerInput = 0f;
    }
}
