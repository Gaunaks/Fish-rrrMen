using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonControllerSimple : MonoBehaviour
{
    public CharacterController controller;
    public Camera playerCamera;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 2.5f;

    [Header("Look Limits")]
    public float minY = -80f;
    public float maxY = 80f;

    private float cameraPitch;
    private float cameraYaw;
    private Vector3 velocity;

    private bool allowMovement = true;
    private bool allowBodyRotation = true; // 👈 THIS IS NEW

    void Awake()
    {
        if (!controller) controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Look();

        if (allowMovement)
            Move();
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, minY, maxY);

        cameraYaw += mouseX;

        // Camera always rotates
        playerCamera.transform.localRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);

        // Player body rotates ONLY when allowed
        if (allowBodyRotation)
            transform.rotation = Quaternion.Euler(0f, cameraYaw, 0f);
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded)
            velocity.y = -2f;
        else
            velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    // ----------------------------
    // CONTROL MODES
    // ----------------------------
    public void SetMovementEnabled(bool enabled)
    {
        allowMovement = enabled;
    }

    public void SetBodyRotationEnabled(bool enabled)
    {
        allowBodyRotation = enabled;
    }
}
