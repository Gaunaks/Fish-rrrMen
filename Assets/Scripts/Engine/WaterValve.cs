using UnityEngine;

public class WaterValve : MonoBehaviour
{
    public SteamEngine engine;
    public WaterTank tank;

    [Header("Valve Control")]
    [Range(0f, 1f)] public float valveOpen = 0f;

    [Header("Flow")]
    public float maxFlowRate = 8f; // water/sec at full open

    private void FixedUpdate()
    {
        if (!engine || !tank) return;
        if (valveOpen <= 0f) return;

        float dt = Time.fixedDeltaTime;

        float requested = maxFlowRate * valveOpen * dt;
        float taken = tank.RemoveWater(requested);

        if (taken > 0f)
            engine.AddBoilerWater(taken);
    }
}
