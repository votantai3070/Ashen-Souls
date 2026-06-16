using UnityEngine;

public class EnvironmentSlot : MonoBehaviour
{
    public EnvironmentType environmentType;

    private void OnValidate()
    {
        gameObject.name = $"Environment - {environmentType}";
    }
}
