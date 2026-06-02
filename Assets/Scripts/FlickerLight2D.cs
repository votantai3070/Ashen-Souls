using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickerLight2D : MonoBehaviour
{
    [SerializeField] private Light2D light2D;
    [SerializeField] private float minIntensity = 0.7f;
    [SerializeField] private float maxIntensity = 1.2f;
    [SerializeField] private float flickerSpeed = 10f;

    private float seed;

    private void Awake()
    {
        if (light2D == null)
            light2D = GetComponentInChildren<Light2D>();

        seed = Random.Range(0f, 100f);
    }

    private void Update()
    {
        if (light2D == null) return;

        float noise = Mathf.PerlinNoise(seed, Time.time * flickerSpeed);
        light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}