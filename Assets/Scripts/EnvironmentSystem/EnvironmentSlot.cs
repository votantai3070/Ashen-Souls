using UnityEngine;

public class EnvironmentSlot : MonoBehaviour
{
    public EnvironmentType environmentType;

    [Header("Tornado Settings")]
    public GameObject tornadoGo;
    [SerializeField] private int count = 1;

    private void OnValidate()
    {
        gameObject.name = $"Environment - {environmentType}";
    }

    public void SpawnTornadoByCount(Vector2 pos)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnTornado(pos);
        }
    }

    public void SpawnTornado(Vector2 pos)
    {
        if (tornadoGo == null)
            return;

        if (environmentType != EnvironmentType.Rain)
            return;

        ObjectPool.instance.Spawn(tornadoGo.name, pos, Quaternion.identity);
    }
}
