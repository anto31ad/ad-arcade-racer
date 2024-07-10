using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager instance;

    [Header("Tarmac")]
    [Range(0f, 5f)]
    public float tarmacLongGrip = 1f;
    [Range(0f, 5f)]
    public float tarmacLatGrip = 1f;
    [Range(0f, 5f)]
    public float tarmacRollingResistance = 1f;

    [Header("Dirt")]
    [Range(0f, 5f)]
    public float dirtLongGrip = 0.9f;
    [Range(0f, 5f)]
    public float dirtLatGrip = 0.8f;
    [Range(0f, 5f)]
    public float dirtRollingResistance = 1.1f;

    [Header("Grass")]
    [Range(0f, 5f)]
    public float grassLongGrip = 0.7f;
    [Range(0f, 5f)]
    public float grassLatGrip = 0.4f;
    [Range(0f, 5f)]
    public float grassRollingResistance = 2f;


    void Awake() {
        instance = this;
    }

    public float GetLongitudinalGripCoefficient(string surface) {
        return surface switch
        {
            "Tarmac" => tarmacLongGrip,
            "Grass" => grassLongGrip,
            "Dirt" => dirtLongGrip,
            _ => 0f,
        };
    }

    public float GetLateralGripCoefficient(string surface) {
        return surface switch
        {
            "Tarmac" => tarmacLatGrip,
            "Grass" => grassLatGrip,
            "Dirt" => dirtLatGrip,
            _ => 0f,
        };
    }

    public float GetRollingResistanceCoefficient(string surface) {
        return surface switch
        {
            "Tarmac" => tarmacRollingResistance,
            "Grass" => grassRollingResistance,
            "Dirt" => dirtRollingResistance,
            _ => 0f,
        };
    }
}
