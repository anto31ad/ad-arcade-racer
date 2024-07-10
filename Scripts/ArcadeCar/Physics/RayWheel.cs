using UnityEngine;

public class RayWheel : MonoBehaviour
{
    [Header ("Suspension Properties")]
    public float springRestLen = 0.3f;
    public float raycastExtension = 0.0f;
    
    public float springStiffness;
    public float damperRate;

    [Header ("Wheel Properties")]
    public float wheelRadius;
    public Vector3 forceAppPoint;

    [Header ("Tyre Properties")]
    public AnimationCurve maxLateralFrictionOverSlip;
    public float peakLongFrictionMult = 1.1f;
    public float rollingResistanceConstant;

    [Header("Debug")]
    private Rigidbody _chassisRigidbody;
    private bool _isGrounded;
    private RaycastHit _rayHit;

    private float _steerAngle;
    private Vector3 _suspensionForce;
    private float _curSpringLen;

    private float _wheelLoad;

    private float _longGripMult;
    private float _latGripMult;
    private float _rollingResMult;
    private float _driveForce;
    
    private float _brakeForce;
    private float _finalBrakeForce;
    private float _rollingResistanceForce;

    private float _longVel;
    private Vector3 _longDir;
    private float _netLongForce;
    private Vector3 _latDir;
    private float _latVel;
    private float _slipAngleRatio;
    private float _netLatForce;


    public float DriveForce
    {
        get {return _driveForce;}
        set {_driveForce = value;}
    }

    public float BrakeForce
    {
        get {return _brakeForce;}
        set {_brakeForce = value;}
    }

    public float SteerAngleDeg {
        get {return _steerAngle * Mathf.Rad2Deg;}
        set {_steerAngle = value * Mathf.Deg2Rad;}
    }

    public bool IsGrounded
    {
        get {return _isGrounded;}
    }

    public string HitColliderTag
    {
        get {return _rayHit.collider.tag;}
    }

    public float Load
    {
        get {return _wheelLoad;}
    }

    public float SlipAngleRatio {
        get => _slipAngleRatio;
    }

    public Vector3 WheelCenter {
        get {return transform.position + -transform.up *_curSpringLen;}
    }

    public Vector3 ContactPatchPosition {
        get {return transform.position + -transform.up * (_curSpringLen + wheelRadius);}
    }


    // Start is called before the first frame update
    void Start()
    {
        _chassisRigidbody = GetComponentInParent<Rigidbody>();
    }

    void Update() {

        // LONGITUDINAL DEBUG
        Vector3 longDir = CalculateLongDir();
        // pointing direction
        Debug.DrawRay(WheelCenter, longDir * 1, Color.black);
        // drive force
        Debug.DrawRay(WheelCenter, longDir * _driveForce / _chassisRigidbody.mass, Color.green);
        // brake force
        Debug.DrawRay(WheelCenter, -longDir * _brakeForce / _chassisRigidbody.mass, Color.red);
        // rolling resistance
        Debug.DrawRay(WheelCenter, longDir * _rollingResistanceForce / _chassisRigidbody.mass, Color.magenta);

        Debug.DrawRay(
            WheelCenter + transform.TransformDirection(0, 0.1f, 0),
            longDir * _netLongForce / _chassisRigidbody.mass,
            Color.cyan);

        // LATERAL DEBUG
        Vector3 latDir = CalculateLatDir();
        Debug.DrawRay(WheelCenter, latDir * _netLatForce / _chassisRigidbody.mass, Color.yellow);

        // SUSPENSION DEBUG
        Debug.DrawRay(transform.position, -_suspensionForce / _chassisRigidbody.mass, Color.blue);
    }

    private Vector3 CalculateLatDir() {
        return transform.TransformDirection(new (Mathf.Cos(_steerAngle), 0f, Mathf.Sin(_steerAngle)));
    }

    private Vector3 CalculateLongDir() {
        return transform.TransformDirection(new (-Mathf.Sin(_steerAngle), 0f, Mathf.Cos(_steerAngle)));
    }

    void FixedUpdate()
    {
        _isGrounded = Physics.Raycast(
            transform.position,
            -transform.up,
            out _rayHit,
            springRestLen + raycastExtension + wheelRadius,
            -5,
            QueryTriggerInteraction.Ignore);

        UpdateOverallGripCoefficients();

        _latDir = CalculateLatDir();
        _longDir = CalculateLongDir();
  
        if (_isGrounded) {
            Vector3 _vel = _chassisRigidbody.GetPointVelocity(_rayHit.point);    //change to _rayhit
            _latVel = Vector3.Dot(_latDir, _vel);
            _longVel = Vector3.Dot(_longDir, _vel);
            _slipAngleRatio = Mathf.Atan2(_latVel, Mathf.Abs(_longVel)) / Mathf.PI;    //this is the normalized slip angle.

            ApplySuspensionForce();
            _wheelLoad = Mathf.Clamp(Vector3.Dot(transform.up, _suspensionForce), 0f, float.MaxValue);
            ApplyTyreForces();
        } else {
            _curSpringLen = springRestLen;
            _suspensionForce = Vector3.zero;
            _wheelLoad = 0f;
            _netLatForce = 0f;
            _netLongForce = 0f;
            _longVel = 0f;
            _latVel = 0f;
            _slipAngleRatio = 0f;
        }
    }

    private void UpdateOverallGripCoefficients() {
        if (IsGrounded) {
            string surface = _rayHit.collider.tag;

            _latGripMult = EnvironmentManager.instance.GetLateralGripCoefficient(surface);
            _longGripMult = EnvironmentManager.instance.GetLongitudinalGripCoefficient(surface);
            _rollingResMult = EnvironmentManager.instance.GetRollingResistanceCoefficient(surface);
        }
    }

    private void ApplySuspensionForce() {
        float offset = _rayHit.distance - wheelRadius - springRestLen;
        
        float prevSpringLen = _curSpringLen;
        _curSpringLen = springRestLen + offset;
        _curSpringLen = Mathf.Clamp(_curSpringLen, 0, springRestLen);

        float curSpringSpeed = (_curSpringLen - prevSpringLen) / Time.deltaTime;
        float curSpringForceMag = - offset * springStiffness;
        float curDamperForceMag = curSpringSpeed * damperRate;
        
        _suspensionForce = transform.up * (curSpringForceMag - curDamperForceMag);
        _chassisRigidbody.AddForceAtPosition(_suspensionForce, transform.position);
    }

    private void ApplyTyreForces() {
        ApplyLateralFriction();
        ApplyLongitudinalFriction();
        Vector3 finalFrictionForce = _latDir * _netLatForce + _longDir * _netLongForce;

        Vector3 _forceAppPoint = _rayHit.point + transform.TransformDirection(forceAppPoint);
        _chassisRigidbody.AddForceAtPosition(finalFrictionForce, _forceAppPoint);
    }

    private void ApplyLateralFriction() {
        float _latFrictionCoeff = maxLateralFrictionOverSlip.Evaluate(Mathf.Abs(_slipAngleRatio));
        float maxFriction = _wheelLoad > 0f ? _wheelLoad : 0f;
        float clampFactor = Mathf.Clamp01(Mathf.Abs(_latVel));
        
        _netLatForce = - maxFriction * Mathf.Sign(_latVel) * _latFrictionCoeff * clampFactor * _latGripMult;
    }

    private void ApplyLongitudinalFriction() {
        float brakeFactor = Mathf.Clamp01(Mathf.Abs(_longVel));
        _finalBrakeForce = _brakeForce * Mathf.Sign(_longVel) * brakeFactor;
       
        float gripCoeffAffectedForces = (_driveForce - _finalBrakeForce) * _longGripMult;
        _rollingResistanceForce = _longVel * rollingResistanceConstant * _rollingResMult;

        _netLongForce = gripCoeffAffectedForces - _rollingResistanceForce;

        float maxFriction = _wheelLoad > 0f ? _wheelLoad : 0f;
        _netLongForce = Mathf.Clamp(_netLongForce, -maxFriction, maxFriction) * peakLongFrictionMult;
    }
}
