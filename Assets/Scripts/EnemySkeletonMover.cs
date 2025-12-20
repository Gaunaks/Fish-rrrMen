using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonMover : MonoBehaviour
{
    public Transform target;
    public float speed = 1.5f;

    void Update()
    {
        if (!target) return;

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }
}
