using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidBody;

    [Header("Buoyancy")]
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;

    [Header("Water Drag")]
    public float waterDrag = 1f;
    public float waterAngularDrag = 0.5f;

    [Header("Wave Settings")]
    public float waterLevel = 0f;
    public float waveSteepness = 0.8f;
    public float waveLength = 12f;
    public float waveSpeed = 1.2f;
    public float[] waveDirections = new float[] { 0.13f, 0.37f, 0.61f };

    [Header("Floaters")]
    public int floatersCount = 1;

    [Header("Stabilization")]
    public float waveSmoothing = 5f;
    public float verticalDampingStrength = 0.5f;

    private float smoothedWaveHeight;

    private void FixedUpdate()
    {
        // Gravity at floater
        rigidBody.AddForceAtPosition(
            Physics.gravity / floatersCount,
            transform.position,
            ForceMode.Acceleration
        );

        // Sample & smooth wave height
        float targetWaveHeight =
            waterLevel +
            GerstnerWaveDisplacement.GetWaveDisplacement(
                transform.position,
                waveSteepness,
                waveLength,
                waveSpeed,
                waveDirections
            ).y;

        smoothedWaveHeight = Mathf.Lerp(
            smoothedWaveHeight,
            targetWaveHeight,
            Time.fixedDeltaTime * waveSmoothing
        );

        float depth = smoothedWaveHeight - transform.position.y;

        if (depth > 0f)
        {
            float submersion = Mathf.Clamp01(depth / depthBeforeSubmerged);
            submersion *= submersion; // softer response

            // Buoyancy
            Vector3 buoyancyForce =
                Vector3.up *
                Mathf.Abs(Physics.gravity.y) *
                submersion *
                displacementAmount /
                floatersCount;

            rigidBody.AddForceAtPosition(
                buoyancyForce,
                transform.position,
                ForceMode.Acceleration
            );

            // Linear drag
            rigidBody.AddForce(
                -rigidBody.velocity * waterDrag * submersion / floatersCount,
                ForceMode.Acceleration
            );

            // Angular drag
            rigidBody.AddTorque(
                -rigidBody.angularVelocity * waterAngularDrag * submersion / floatersCount,
                ForceMode.Acceleration
            );

            // Extra vertical damping
            rigidBody.AddForce(
                Vector3.up * (-rigidBody.velocity.y * verticalDampingStrength * submersion),
                ForceMode.Acceleration
            );
        }
    }
}
