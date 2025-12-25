using UnityEngine;

public class WaterTank : MonoBehaviour
{
    [Header("Storage")]
    public float currentWater = 0f;
    public float maxWater = 100f;

    public bool IsEmpty => currentWater <= 0f;
    public bool IsFull => currentWater >= maxWater;

    public void AddWater(float amount)
    {
        currentWater = Mathf.Clamp(currentWater + amount, 0f, maxWater);
    }

    public float RemoveWater(float amount)
    {
        float taken = Mathf.Min(amount, currentWater);
        currentWater -= taken;
        return taken;
    }
}
