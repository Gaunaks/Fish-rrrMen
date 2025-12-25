using UnityEngine;

public class PressureGauge : MonoBehaviour
{
    public SteamEngine engine;

    [Header("Needle Pivot (rotate this)")]
    public Transform needlePivot;

    [Header("Mapping")]
    [Tooltip("Angle when pressure = 0")]
    public float zeroAngle = -90f;

    [Tooltip("Angle when pressure = maxSteamPressure")]
    public float maxAngle = 90f;

    [Header("Smoothing")]
    public float responseSpeed = 5f;

    private float currentAngle;

    private void Awake()
    {
        // Start at the correct resting position
        currentAngle = zeroAngle;
        ApplyRotation(currentAngle);
    }

    void Update()
    {
        if (!engine || !needlePivot) return;

        float maxP = Mathf.Max(0.0001f, engine.maxSteamPressure);
        float normalized = Mathf.Clamp01(engine.steamPressure / maxP);

        float targetAngle = Mathf.Lerp(zeroAngle, maxAngle, normalized);

        currentAngle = Mathf.Lerp(
            currentAngle,
            targetAngle,
            Time.deltaTime * responseSpeed
        );

        ApplyRotation(currentAngle);
    }

    private void ApplyRotation(float angle)
    {
        // Rotate on X axis (your mesh needs X)
        needlePivot.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }
}
