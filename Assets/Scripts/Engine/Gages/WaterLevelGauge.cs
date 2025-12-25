using UnityEngine;

public class WaterLevelGauge : MonoBehaviour
{
    public WaterTank tank;

    [Header("Needle")]
    public Transform needle;
    public float minAngle = -90f;
    public float maxAngle = 90f;

    [Header("Smoothing")]
    public float responseSpeed = 3f;

    private float currentAngle;

    void Update()
    {
        if (!tank || !needle) return;

        float normalized =
            tank.currentWater / tank.maxWater;

        float targetAngle =
            Mathf.Lerp(minAngle, maxAngle, normalized);

        currentAngle = Mathf.Lerp(
            currentAngle,
            targetAngle,
            Time.deltaTime * responseSpeed
        );

        needle.localRotation =
            Quaternion.Euler(0f, 0f, currentAngle);
    }
}
