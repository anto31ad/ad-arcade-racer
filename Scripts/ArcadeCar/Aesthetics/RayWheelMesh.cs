using UnityEngine;

public class RayWheelMesh : MonoBehaviour
{
    [SerializeField] RayWheel target;

    private Rigidbody _rigidbody;
    private float _angularPos;

    void Start()
    {
        _rigidbody = GetComponentInParent<Rigidbody>();
    }

    void Update() {
        ComputeAngularPosition();
        UpdateTransform();
    }

    private void ComputeAngularPosition() {
        float angularVel = _rigidbody.velocity.magnitude / target.wheelRadius;
        _angularPos += angularVel * Time.deltaTime;

        while (Mathf.Sign(_angularPos) * _angularPos > 2 * Mathf.PI) {
            _angularPos -= Mathf.Sign(_angularPos) * 2 * Mathf.PI;
        }
    }

    private void UpdateTransform() {
        this.transform.position = target.WheelCenter;
        
        float angularPosDeg = _angularPos * Mathf.Rad2Deg;
        Vector3 angularPosEuler = new(angularPosDeg, -target.SteerAngleDeg, 0f);
        Quaternion rot = Quaternion.Euler(angularPosEuler);
        this.transform.localRotation = rot;
    }
}
