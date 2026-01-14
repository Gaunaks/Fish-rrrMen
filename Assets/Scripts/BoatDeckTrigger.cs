using UnityEngine;

public class BoatDeckTrigger : MonoBehaviour
{
    public Transform boatRoot;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        other.transform.SetParent(boatRoot);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        other.transform.SetParent(null);
    }
}
