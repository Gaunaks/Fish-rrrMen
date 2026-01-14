using UnityEngine;

public class BoatDeckMassFix : MonoBehaviour
{
    public Rigidbody playerRb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == playerRb)
            playerRb.isKinematic = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody == playerRb)
            playerRb.isKinematic = false;
    }
}
