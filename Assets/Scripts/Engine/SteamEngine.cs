using UnityEngine;

public class SteamEngine : MonoBehaviour
{
    [Header("Fuel")]
    public float coal = 50f;

    [Header("Boiler Water")]
    public float boilerWater = 20f;
    public float maxBoilerWater = 60f;

    [Header("Core State")]
    public float heat = 20f;
    public float steamPressure = 0f;
    public float energy = 0f;

    [Header("Maximums (for gauges)")]
    public float maxHeat = 140f;
    public float maxSteamPressure = 120f;
    public float maxEnergy = 150f;

    [Header("Heat Model")]
    public float ambientHeat = 10f;
    public float heatPerCoal = 45f;
    public float coolingPerWaterUnit = 20f;

    [Header("Rates")]
    public float coalBurnRate = 1f;
    public float waterUseRate = 1.2f;

    [Header("Response")]
    public float heatResponseSpeed = 15f;
    public float pressureResponseSpeed = 12f;

    [Header("Passive Decay")]
    public float passiveCooling = 4f;
    public float passivePressureRelease = 4f;

    [Header("Steam & Energy")]
    public float pressureFromHeat = 1.1f;
    public float energyGenerationRate = 40f;

    [Header("Optimal Heat Window")]
    public float optimalHeatMin = 50f;
    public float optimalHeatMax = 95f;

    [Header("Overclock")]
    public bool overclock = false;
    public float overclockMultiplier = 1.6f;

    [Header("Chimney Venting")]
    public float chimneyPressureReleaseRate = 40f;
    public float chimneyHeatReleaseRate = 18f;

    [Header("Debug")]
    [Range(0f, 1f)] public float efficiency;

    void FixedUpdate()
    {
        RunEngine(Time.fixedDeltaTime);
    }

    void RunEngine(float dt)
    {
        bool hasFuel = coal > 0f && boilerWater > 0f;
        float load = overclock ? overclockMultiplier : 1f;

        // ----------------------------
        // TARGET HEAT
        // ----------------------------
        float targetHeat = ambientHeat;

        if (hasFuel)
        {
            float coalUsed = coalBurnRate * load * dt;
            float waterUsed = waterUseRate * dt;

            coal = Mathf.Max(0f, coal - coalUsed);
            boilerWater = Mathf.Max(0f, boilerWater - waterUsed);

            targetHeat += heatPerCoal * load;
            targetHeat -= coolingPerWaterUnit * waterUsed;
        }

        heat = Mathf.MoveTowards(heat, targetHeat, heatResponseSpeed * dt);
        heat = Mathf.MoveTowards(heat, ambientHeat, passiveCooling * dt);
        heat = Mathf.Clamp(heat, 0f, maxHeat);

        // ----------------------------
        // EFFICIENCY
        // ----------------------------
        if (heat < optimalHeatMin)
            efficiency = heat / optimalHeatMin;
        else if (heat > optimalHeatMax)
            efficiency = Mathf.Clamp01(1f - (heat - optimalHeatMax) / 40f);
        else
            efficiency = 1f;

        // ----------------------------
        // TARGET PRESSURE
        // ----------------------------
        float targetPressure = heat * pressureFromHeat * efficiency;

        steamPressure = Mathf.MoveTowards(
            steamPressure,
            targetPressure,
            pressureResponseSpeed * dt
        );

        steamPressure -= passivePressureRelease * dt;
        steamPressure = Mathf.Clamp(steamPressure, 0f, maxSteamPressure);

        // ----------------------------
        // ENERGY
        // ----------------------------
        float energyProduced =
            energyGenerationRate *
            (steamPressure / maxSteamPressure) *
            efficiency *
            dt;

        energy = Mathf.Clamp(energy + energyProduced, 0f, maxEnergy);
    }

    // ----------------------------
    // INTERFACE
    // ----------------------------
    public float RequestEnergy(float amount)
    {
        float granted = Mathf.Min(amount, energy);
        energy -= granted;
        return granted;
    }

    public void AddBoilerWater(float amount)
    {
        boilerWater = Mathf.Clamp(boilerWater + amount, 0f, maxBoilerWater);
    }

    public void VentSteam(float dt)
    {
        steamPressure = Mathf.Max(
            0f,
            steamPressure - chimneyPressureReleaseRate * dt
        );

        heat = Mathf.Max(
            ambientHeat,
            heat - chimneyHeatReleaseRate * dt
        );
    }
}
