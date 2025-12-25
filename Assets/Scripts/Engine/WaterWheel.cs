using UnityEngine;

public class WaterWheel : MonoBehaviour
{
    public WaterValve valve;

    public float minAngle = 0f;
    public float maxAngle = 180f;

    void Update()
    {
        float angle = transform.localEulerAngles.y;
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        valve.valveOpen =
            Mathf.InverseLerp(minAngle, maxAngle, angle);
    }
}
