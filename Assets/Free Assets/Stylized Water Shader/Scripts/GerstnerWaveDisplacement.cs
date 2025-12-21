using UnityEngine;

public static class GerstnerWaveDisplacement
{
    private static Vector3 GerstnerWave(
        Vector3 position,
        float steepness,
        float wavelength,
        float phaseSpeed,
        Vector2 direction,
        float time)
    {
        direction.Normalize();

        float k = 2f * Mathf.PI / wavelength;   // wave number
        float a = steepness / k;                // amplitude

        float f = k * Vector2.Dot(direction, new Vector2(position.x, position.z))
                - phaseSpeed * time;

        float cosF = Mathf.Cos(f);
        float sinF = Mathf.Sin(f);

        return new Vector3(
            direction.x * a * cosF,
            a * sinF,
            direction.y * a * cosF
        );
    }

    public static Vector3 GetWaveDisplacement(
        Vector3 position,
        float baseSteepness,
        float baseWavelength,
        float baseSpeed,
        float[] directions)
    {
        float time = Time.time * baseSpeed; // 🔑 unified wave time

        Vector3 offset = Vector3.zero;

        // LARGE SWELL
        offset += GerstnerWave(
            position,
            baseSteepness * 1.0f,
            baseWavelength * 1.3f,
            1.0f,
            DirectionFrom01(directions[0]),
            time
        );

        // MEDIUM SWELL
        offset += GerstnerWave(
            position,
            baseSteepness * 0.6f,
            baseWavelength * 0.7f,
            1.15f,
            DirectionFrom01(directions[1]),
            time
        );

        // SMALL DETAIL (calmer to avoid jitter)
        offset += GerstnerWave(
            position,
            baseSteepness * 0.2f,
            baseWavelength * 0.35f,
            1.3f,
            DirectionFrom01(directions[2]),
            time
        );

        return offset;
    }

    private static Vector2 DirectionFrom01(float value)
    {
        float angle = value * Mathf.PI * 2f;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    public static float GetWaveHeight(
        Vector3 worldPosition,
        float baseSteepness,
        float baseWavelength,
        float baseSpeed,
        float[] directions,
        float waterLevel = 0f)
    {
        return waterLevel + GetWaveDisplacement(
            worldPosition,
            baseSteepness,
            baseWavelength,
            baseSpeed,
            directions
        ).y;
    }
}
