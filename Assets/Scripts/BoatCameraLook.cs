using UnityEngine;

public class BoatCameraLook : MonoBehaviour
{
    [Header("Mouse Look")]
    public float mouseSensitivity = 3f;
    public float minPitch = -30f;
    public float maxPitch = 60f;

    private float yaw;
    private float pitch;

    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -5);
    public float followSmoothness = 5f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPos = target.position + transform.rotation * offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            Time.deltaTime * followSmoothness
        );
    }
}
