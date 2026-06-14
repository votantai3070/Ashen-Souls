using UnityEngine;

public class DropSystem : MonoBehaviour
{
    private Enemy enemy;
    private BreakableObject_Base breakableObject;

    [SerializeField] private GameObject[] definitelyFallDropPrefabs;
    [SerializeField] private GameObject[] dropPrefabs;
    [SerializeField] private int definitelyFallDropCount = 1;
    [SerializeField] private int dropCount = 1;

    [Space]
    [SerializeField] private float dropRadius = 1.5f;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        breakableObject = GetComponent<BreakableObject_Base>();
    }

    public void SpawnDrop(Vector3 originalPos)
    {
        if (definitelyFallDropCount <= 0) return;

        for (int i = 0; i < definitelyFallDropCount; i++)
        {
            SpawnSingleItemDefiniteDrop(originalPos);
        }

        if (dropCount <= 0) return;

        for (int i = 0; i < dropCount; i++)
        {
            SpawnSingleDrop(originalPos);
        }

    }

    private void SpawnSingleItemDefiniteDrop(Vector3 originalPos)
    {
        foreach (var item in definitelyFallDropPrefabs)
        {
            if (item == null) continue;

            Vector3 dropPos = GetRandomDropPosition(originalPos);
            GameObject itemGo = ObjectPool.instance.Spawn(item.name, dropPos, Quaternion.identity);

            itemGo.transform.position = dropPos;

            if (itemGo.TryGetComponent(out SkillObject_Soul soul))
            {
                SetupSoulDrop(soul);
            }
        }
    }

    private void SpawnSingleDrop(Vector3 originalPos)
    {
        if (dropPrefabs == null || dropPrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, dropPrefabs.Length);
        GameObject prefab = dropPrefabs[randomIndex];

        Vector3 randomDropPos = GetRandomDropPosition(originalPos);
        GameObject go = ObjectPool.instance.Spawn(prefab.name, randomDropPos, Quaternion.identity);

        go.transform.position = randomDropPos;

        if (go.TryGetComponent(out SkillObject_Soul randomSoul))
        {
            SetupSoulDrop(randomSoul);
        }
    }

    private Vector3 GetRandomDropPosition(Vector3 originalPos)
    {
        Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(0.5f, dropRadius);
        return originalPos + new Vector3(randomOffset.x, randomOffset.y, 0f);
    }

    private void SetupSoulDrop(SkillObject_Soul soul)
    {
        Transform playerTransform = null;

        if (enemy != null && enemy.player != null)
            playerTransform = enemy.player;
        else if (breakableObject != null && breakableObject.player != null)
            playerTransform = breakableObject.player;

        if (playerTransform == null)
        {
            Debug.LogWarning("DropSystem: Player reference is missing.");
            return;
        }

        Player player = playerTransform.GetComponent<Player>();
        if (player == null || player.skillManager == null)
        {
            Debug.LogWarning("DropSystem: Player or SkillManager is missing.");
            return;
        }

        GameObject absorbSoulObj = player.skillManager.GetSkillByType(SkillType.AbsorbSoul).gameObject;
        if (absorbSoulObj == null)
            return;

        Skill_AbsorbSoul absorbSoul = absorbSoulObj.GetComponent<Skill_AbsorbSoul>();
        if (absorbSoul == null || player.skillManager.absorbSoul == null)
            return;

        bool canMove =
            Vector2.Distance(soul.transform.position, player.transform.position) < absorbSoul.checkEnemyRadius;

        soul.SetupSoul(
            player.skillManager.absorbSoul,
            canMove,
            player.skillManager.absorbSoul.defaultSpeedOfSoul,
            playerTransform
        );
    }
}