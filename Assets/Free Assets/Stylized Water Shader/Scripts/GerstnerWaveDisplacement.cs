using UnityEngine;

public static class GerstnerWaveDisplacement
{
    // Single Gerstner wave
    private static Vector3 GerstnerWave(
        Vector3 position,
        float steepness,
        float wavelength,
        float speed,
        Vector2 direction,
        float time)
    {
        direction.Normalize();

        float k = 2f * Mathf.PI / wavelength;        // wave number
        float a = steepness / k;                     // amplitude
        float f = k * (Vector2.Dot(direction, new Vector2(position.x, position.z)) - speed * time);

        float cosF = Mathf.Cos(f);
        float sinF = Mathf.Sin(f);

        return new Vector3(
            direction.x * a * cosF,   // X displacement
            a * sinF,                 // Y displacement
            direction.y * a * cosF    // Z displacement
        );
    }

    // Public API – call this from buoyancy or other systems
    public static Vector3 GetWaveDisplacement(
        Vector3 position,
        float baseSteepness,
        float baseWavelength,
        float baseSpeed,
        float[] directions)
    {
        float time = Time.time;

        Vector3 offset = Vector3.zero;

        // ---- LARGE SWELL (primary motion) ----
        offset += GerstnerWave(
            position,
            baseSteepness * 1.0f,
            baseWavelength * 1.2f,
            baseSpeed * 0.8f,
            DirectionFrom01(directions[0]),
            time
        );

        // ---- MEDIUM SWELL ----
        offset += GerstnerWave(
            position,
            baseSteepness * 0.6f,
            baseWavelength * 0.6f,
            baseSpeed * 1.2f,
            DirectionFrom01(directions[1]),
            time
        );

        // ---- SMALL DETAIL WAVE ----
        offset += GerstnerWave(
            position,
            baseSteepness * 0.25f,
            baseWavelength * 0.25f,
            baseSpeed * 1.8f,
            DirectionFrom01(directions[2]),
            time
        );

        return offset;
    }

    // Converts 0–1 value into a unit direction vector
    private static Vector2 DirectionFrom01(float value)
    {
        float angle = value * Mathf.PI * 2f;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}
