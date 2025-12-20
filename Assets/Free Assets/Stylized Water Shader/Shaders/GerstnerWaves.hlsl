// ------------------------------------------------------------
// Single Gerstner wave
// ------------------------------------------------------------
float3 GerstnerWave(
    float3 position,
    float steepness,
    float wavelength,
    float speed,
    float direction,
    inout float3 tangent,
    inout float3 binormal)
{
    // Map 0–1 direction to full circle (0–2π)
    float angle = direction * 6.2831853; // 2 * PI
    float2 d = float2(cos(angle), sin(angle));

    float k = 2.0 * PI / wavelength;   // wave number
    float a = steepness / k;           // amplitude
    float f = k * (dot(d, position.xz) - speed * _Time.y);

    float cosF = cos(f);
    float sinF = sin(f);

    // Accumulate tangent & binormal for normal reconstruction
    tangent += float3(
        -d.x * d.x * steepness * sinF,
         d.x * steepness * cosF,
        -d.x * d.y * steepness * sinF
    );

    binormal += float3(
        -d.x * d.y * steepness * sinF,
         d.y * steepness * cosF,
        -d.y * d.y * steepness * sinF
    );

    // Vertex displacement
    return float3(
        d.x * a * cosF,
        a * sinF,
        d.y * a * cosF
    );
}


// ------------------------------------------------------------
// Gerstner wave stack (layered, stylized)
// ------------------------------------------------------------
void GerstnerWaves_float(
    float3 position,
    float steepness,
    float wavelength,
    float speed,
    float4 directions,
    out float3 Offset,
    out float3 normal)
{
    Offset = float3(0, 0, 0);

    float3 tangent  = float3(1, 0, 0);
    float3 binormal = float3(0, 0, 1);

    // === LARGE PRIMARY SWELL ===
    Offset += GerstnerWave(
        position,
        steepness * 1.0,
        wavelength * 1.2,
        speed * 0.8,
        directions.x,
        tangent,
        binormal
    );

    // === MEDIUM SWELL ===
    Offset += GerstnerWave(
        position,
        steepness * 0.6,
        wavelength * 0.6,
        speed * 1.2,
        directions.y,
        tangent,
        binormal
    );

    // === SMALL DETAIL WAVE ===
    Offset += GerstnerWave(
        position,
        steepness * 0.25,
        wavelength * 0.25,
        speed * 1.8,
        directions.z,
        tangent,
        binormal
    );

    // Final normal
    normal = normalize(cross(binormal, tangent));
}
