using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : MonoBehaviour
{
    public Rigidbody[] bones;
    public Rigidbody head;
    public float scatterForce = 3f;

    private bool collapsed = false;

    public float reassembleForce = 5f;
    public float reassembleDistance = 1.2f;
    public float headRange = 5f;


    public void RemoveHead()
    {
        if (collapsed) return;
        collapsed = true;

        foreach (var rb in bones)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            Vector3 force = Random.onUnitSphere * scatterForce;
            rb.AddForce(force, ForceMode.Impulse);
        }
        var joint = head.GetComponent<Joint>();
        if (joint) Destroy(joint);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            RemoveHead();
        }
    }


    void FixedUpdate()
    {
        if (!collapsed) return;

        if (Vector3.Distance(head.position, transform.position) > headRange)
            return; // head too far, skeleton is defeated

        foreach (var rb in bones)
        {
            Vector3 dir = (head.position - rb.position);
            rb.AddForce(dir.normalized * reassembleForce);
        }
    }


}
