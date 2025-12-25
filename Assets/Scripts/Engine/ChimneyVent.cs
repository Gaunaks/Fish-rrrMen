using UnityEngine;

public class ChimneyVent : MonoBehaviour
{
    public SteamEngine engine;

    [Header("Input")]
    public KeyCode ventKey = KeyCode.Space;

    private void FixedUpdate()
    {
        if (engine == null)
            return;

        if (Input.GetKey(ventKey))
        {
            engine.VentSteam(Time.fixedDeltaTime);
        }
    }
}
