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
    public float turnStability = 0.9f;

    private void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.angularDrag = 0f; // handled by water drag
    }

    private void FixedUpdate()
    {
        float forwardInput = Input.GetAxis("Vertical");   // W / S
        float turnInput = Input.GetAxis("Horizontal");    // A / D

        // --- Forward / backward movement ---
        if (Mathf.Abs(forwardInput) > 0.01f)
        {
            Vector3 force = transform.forward * forwardInput * forwardForce;
            rb.AddForce(force, ForceMode.Acceleration);
        }

        // --- Turning (yaw) ---
        if (Mathf.Abs(turnInput) > 0.01f)
        {
            rb.AddTorque(Vector3.up * turnInput * turnTorque, ForceMode.Acceleration);
        }

        // --- Speed clamp (prevents infinite acceleration) ---
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = horizontalVelocity.normalized * maxSpeed;
            rb.velocity = new Vector3(
                limitedVelocity.x,
                rb.velocity.y,
                limitedVelocity.z
            );
        }

        // --- Gentle angular stabilization (prevents spin-out) ---
        rb.angularVelocity *= turnStability;
    }
}
