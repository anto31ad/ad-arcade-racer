using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class ArcadeCar : MonoBehaviour
{
    public Axle frontAxle;
    public Axle rearAxle;

    [Header ("Drivetrain")]
    public bool frontDrive = false;
    public bool rearDrive = true;

    [Header ("Engine")]
    public AnimationCurve forceCurve;
    public float peakEngineForce;
    public float peakBrakeForce;
    public float peakEngineBrakeForce;
    public float maxForwardSpeedKph;
    public float maxReverseSpeedKph;

    [Header ("Brakes")]
    [Range(0f, 1f)]
    public float brakeBias = 0.5f;

    [Header ("Steering")]
    public float maxSteeringAngle = 30f;

    [Header ("Aereodynamics")]
    public float downforceConstant = 40f;

    private float _curSpeedKph;
    private float _steerInput;
    private float _throttle;
    private float _brake;
    private float _slipAngleDeg;
    private float _downforce;

    private bool _reverseGearOn = false;

    private float _speedRatio;

    public float Throttle {
        get => _throttle;
        set {
            _throttle = Mathf.Clamp01(value);
        }
    }

    public float Brake {
        get => _brake;
        set {
            _brake = Mathf.Clamp01(value);
        }
    }

    public float SteerInput {
        get => _steerInput;
        set {
            _steerInput = Mathf.Clamp(value, -1f, 1f);
        }
    }

    public bool ReverseOn {
        get => _reverseGearOn;
        set {
            _reverseGearOn = value;
        }
    }

    public float SpeedKph {
        get {return _curSpeedKph;}
    }

    public float SlipAngleDeg {
        get {return _slipAngleDeg;}
    }

    public float SpeedRatio {
        get {return _speedRatio;}
    }

    private Rigidbody _rb;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 vel = _rb.velocity;
        _curSpeedKph = vel.magnitude * 3.6f;
        
        float latVel = Vector3.Dot(transform.right, vel);
        float longVel = Vector3.Dot(transform.forward, vel);
        _slipAngleDeg = Mathf.Atan2(latVel, Mathf.Abs(longVel)) * Mathf.Rad2Deg;

        ComputeBraking();
        ComputeSteering();
        ComputePower();
        ComputeDownforce();
    }

    private void ComputeSteering() {
        float steerAngle = - maxSteeringAngle * _steerInput;

        frontAxle.leftWheel.SteerAngleDeg = steerAngle;
        frontAxle.rightWheel.SteerAngleDeg = steerAngle;
    }

    private void ComputeBraking() {
        frontAxle._brakeForce = _brake * peakBrakeForce * brakeBias;
        rearAxle._brakeForce = _brake * peakBrakeForce * (1 - brakeBias);
    }

    private void ComputePower() {
        //computes maxSpeed based on whether reverse gear is engaged or not
        float maxSpeedKph = _reverseGearOn ? maxReverseSpeedKph : maxForwardSpeedKph;
        
        //computes the engine drive force...
        //... firstly, the forward force...
        _speedRatio = _curSpeedKph / maxSpeedKph;
        float engineForwardForce = 0f;
        if (_speedRatio < 1) {
            engineForwardForce = peakEngineForce * forceCurve.Evaluate(_speedRatio) * _throttle;

        }
        //... then the engine brake force, which increases linearly as speed increases...
        float engineBrakeForce = Mathf.Clamp01(1 - _throttle) * peakEngineBrakeForce * _speedRatio;

        //then computes final engine force
        float gear = _reverseGearOn ? -1 : 1;
        float driveForce = (engineForwardForce - engineBrakeForce) * gear;

        frontAxle._driveForce = frontDrive ? driveForce : 0f;
        rearAxle._driveForce = rearDrive ? driveForce : 0f;
    }

    private void ComputeDownforce() {
        _downforce = downforceConstant * _rb.velocity.magnitude;
        _rb.AddForce(-transform.up * _downforce);
    }

}
