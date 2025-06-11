using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Controls the bloom effect on the global volume by animating its intensity over time.
/// </summary>
public class ArrowController : MonoBehaviour
{
    public Volume globalVolume;
    private Bloom bloom;

    /// <summary>
    /// Retrieves the Bloom effect from the global volume profile.
    /// </summary>
    void Start()
    {
        if (globalVolume.profile.TryGet<Bloom>(out var bloomEffect))
        {
            bloom = bloomEffect;
        }
        else
        {
            Debug.LogWarning("Color Adjustments not found in Volume Profile");
        }
    }

    /// <summary>
    /// Updates the intensity of the Bloom effect.
    /// </summary>
    void Update()
    {
        if (bloom != null)
        {
            // Ping-pong value between 1 and 10 over time
            float intensity = Mathf.PingPong(Time.time * 2f, 9f) + 1f;
            bloom.intensity.value = intensity;
        }
    }
}
