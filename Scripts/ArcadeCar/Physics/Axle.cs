using UnityEngine;

public class Axle : MonoBehaviour
{
    public RayWheel leftWheel;
    public RayWheel rightWheel;

    [Header ("Debug")]
    public float _brakeForce;
    public float _driveForce;

    void FixedUpdate()
    {
        leftWheel.BrakeForce = _brakeForce;
        rightWheel.BrakeForce = _brakeForce;

        leftWheel.DriveForce = _driveForce;
        rightWheel.DriveForce = _driveForce;
    }
}
