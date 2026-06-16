using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnvironmentSystem : MonoBehaviour
{
    public static EnvironmentSystem instance;

    [SerializeField] private Light2D light2D;
    [SerializeField] private EnvironmentSlot[] environmentSlots;

    private void Awake()
    {
        instance = this;

        environmentSlots = GetComponentsInChildren<EnvironmentSlot>(true);
    }

    public void GetRandomEnvironment()
    {
        if (environmentSlots == null || environmentSlots.Length == 0)
            return;

        int random = Random.Range(0, environmentSlots.Length);

        for (int i = 0; i < environmentSlots.Length; i++)
        {
            environmentSlots[i].gameObject.SetActive(i == random);
        }

        SwitchToEnvironmentLight(environmentSlots[random].environmentType);
    }

    private void SwitchToEnvironmentLight(EnvironmentType type)
    {
        switch (type)
        {
            case EnvironmentType.Fog:
                light2D.intensity = 0.5f;
                break;

            case EnvironmentType.Rain:
                light2D.intensity = 0.4f;
                break;

            default:
                light2D.intensity = 0.8f;
                break;
        }
    }
}
