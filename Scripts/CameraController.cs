using UnityEngine;

[RequireComponent(typeof(Camera))]

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [Header ("Chase")]
    public Transform target;
    public Vector3 distance;
    public float pursueSpeed = 10f;
    public float lookSpeed = 10f;

    [Header ("Cinematic")]
    public Transform cinematicTarget;
    public float cinematicLookSpeed = 10f;
    public float cinematicTravelSpeed = 3f;

    [Header ("Stare")]
    public Vector3 stareDistance;
    public float stareLookSpeed = 10f;
    public float stareTravelSpeed;

    private Vector3 _curTargetPos;

    private Vector3 curVel;
    
    public enum Mode {
        Chase,
        Cinematic,
        Stare
    }
    private Mode _curMode;

    public Mode CurrentMode {
        get => _curMode;
        set {
            _curMode = value;
        }
    }

    void Awake() {
        instance = this;
    }

    void FixedUpdate() {
        switch (_curMode) {
            case Mode.Cinematic:
                CinematicUpdate();
                break;
            case Mode.Stare:
                StareUpdate();
                break;
            default:
                ChaseUpdate();
                break;
        }
    }

    private void CinematicUpdate() {
        LookAtTarget(cinematicLookSpeed);
        _curTargetPos = cinematicTarget.position;
        TravelToPosition(_curTargetPos, cinematicTravelSpeed);
    }

    private void StareUpdate() {
        LookAtTarget(stareLookSpeed);

        _curTargetPos = target.position + target.TransformDirection(stareDistance);
        TravelToPosition(_curTargetPos, stareTravelSpeed);
    }

    private void ChaseUpdate() {
        LookAtTarget(lookSpeed);

        _curTargetPos = target.position + target.TransformDirection(distance);
        TravelToPosition(_curTargetPos, pursueSpeed);
    }

    private void TravelToPosition(Vector3 targetPos, float speed) {
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref curVel, speed);
    }

    private void LookAtTarget(float speed) {
        Vector3 relativeTargetPosition = target.position - transform.position;
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            Quaternion.LookRotation(relativeTargetPosition),
            Time.deltaTime * speed);
    }
}