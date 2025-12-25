using UnityEngine;

public class WaterPump : MonoBehaviour
{
    public SteamEngine engine;
    public WaterTank tank;

    [Header("Pump Settings")]
    public bool pumpOn = false;
    public float pumpRate = 10f;           // water/sec
    public float energyCostPerSecond = 5f; // ENERGY/sec

    private void FixedUpdate()
    {
        if (!pumpOn || engine == null || tank == null) return;
        if (tank.IsFull) return;

        float dt = Time.fixedDeltaTime;

        float energyNeeded = energyCostPerSecond * dt;
        float energyGranted = engine.RequestEnergy(energyNeeded);
        if (energyGranted <= 0f) return;

        float efficiency = energyGranted / energyNeeded;
        tank.AddWater(pumpRate * efficiency * dt);
    }
}
