using UnityEngine;

public class HelmWheel : MonoBehaviour
{
    [Header("References")]
    public FirstPersonControllerSimple playerController; // drag Player here
    public BoatController boatController;

    [Header("Helm Anchors")]
    public Transform helmAnchor;
    public Transform helmCameraMount;

    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;

    private bool steering;
    private Transform cameraTransform;
    private Transform originalPlayerParent;
    private Transform originalCameraParent;

    void Start()
    {
        cameraTransform = playerController.playerCamera.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
            ToggleHelm();
    }

    void ToggleHelm()
    {
        steering = !steering;

        // Enable / disable systems
        boatController.enabled = steering;
        playerController.SetMovementEnabled(!steering);

        if (steering)
            EnterHelm();
        else
            ExitHelm();
    }

    void EnterHelm()
    {
        boatController.enabled = true;

        playerController.SetMovementEnabled(false);
        playerController.SetBodyRotationEnabled(false); // 🔥 IMPORTANT

        playerController.transform.SetParent(helmAnchor);
        playerController.transform.localPosition = Vector3.zero;
        playerController.transform.localRotation = Quaternion.identity;

        var cam = playerController.playerCamera.transform;
        cam.SetParent(helmCameraMount);
        cam.localPosition = Vector3.zero;
        cam.localRotation = Quaternion.identity;
    }

    void ExitHelm()
    {
        boatController.enabled = false;

        playerController.SetMovementEnabled(true);
        playerController.SetBodyRotationEnabled(true);

        playerController.transform.SetParent(null);

        var cam = playerController.playerCamera.transform;
        cam.SetParent(playerController.transform);
    }
}
