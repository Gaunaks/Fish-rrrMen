using UnityEngine;

public class HeatGauge : MonoBehaviour
{
    public SteamEngine engine;

    [Header("Needle Pivot")]
    public Transform needlePivot;

    [Header("Mapping")]
    public float zeroAngle = -90f;
    public float maxAngle = 90f;

    [Header("Smoothing")]
    public float responseSpeed = 4f;

    private float currentAngle;

    void Awake()
    {
        currentAngle = zeroAngle;
        ApplyRotation(currentAngle);
    }

    void Update()
    {
        if (!engine || !needlePivot) return;

        float normalized = Mathf.Clamp01(engine.heat / engine.maxHeat);
        float targetAngle = Mathf.Lerp(zeroAngle, maxAngle, normalized);

        currentAngle = Mathf.Lerp(
            currentAngle,
            targetAngle,
            Time.deltaTime * responseSpeed
        );

        ApplyRotation(currentAngle);
    }

    void ApplyRotation(float angle)
    {
        needlePivot.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }
}
