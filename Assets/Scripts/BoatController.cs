using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatController : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Movement")]
    public float forwardForce = 15f;
    public float turnTorque = 8f;

    [Header("Stability")]
    public float maxSpeed = 6f;
    [Range(0f, 1f)]
    public float turnStability = 0.9f;

    [Header("Engine")]
    public SteamEngine engine;

    [Tooltip("Energy consumed per second at full throttle")]
    public float moveEnergyCostPerSecond = 12f;

    private void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.angularDrag = 0f; // handled by water drag / floaters
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        float forwardInput = Input.GetAxis("Vertical");   // W / S
        float turnInput = Input.GetAxis("Horizontal");    // A / D

        // ----------------------------
        // FORWARD / BACKWARD MOVEMENT
        // ----------------------------
        if (Mathf.Abs(forwardInput) > 0.01f && engine != null)
        {
            // How much energy we WANT this frame
            float energyNeeded =
                moveEnergyCostPerSecond *
                Mathf.Abs(forwardInput) *
                dt;

            // Ask engine for that energy
            float energyGranted = engine.RequestEnergy(energyNeeded);

            // Scale force based on how much we actually got
            float power01 = energyNeeded > 0f ? energyGranted / energyNeeded : 0f;

            Vector3 force =
                transform.forward *
                forwardInput *
                forwardForce *
                power01;

            rb.AddForce(force, ForceMode.Acceleration);
        }

        // ----------------------------
        // TURNING (does not consume energy yet)
        // ----------------------------
        if (Mathf.Abs(turnInput) > 0.01f)
        {
            rb.AddTorque(
                Vector3.up * turnInput * turnTorque,
                ForceMode.Acceleration
            );
        }

        // ----------------------------
        // SPEED CLAMP
        // ----------------------------
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            Vector3 limited = horizontalVelocity.normalized * maxSpeed;
            rb.velocity = new Vector3(limited.x, rb.velocity.y, limited.z);
        }

        // ----------------------------
        // ANGULAR STABILIZATION
        // ----------------------------
        rb.angularVelocity *= turnStability;
    }
}
