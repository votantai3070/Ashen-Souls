using UnityEngine;

public class DropSystem : MonoBehaviour
{
    private Enemy enemy;
    private BreakableObject_Base breakableObject;

    [SerializeField] private GameObject[] dropPrefabs;
    [SerializeField] private int dropCount = 1;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        breakableObject = GetComponent<BreakableObject_Base>();
    }

    public void SpawnDrop()
    {
        if (dropPrefabs == null || dropPrefabs.Length == 0) return;
        if (dropCount <= 0) return;

        for (int i = 0; i < dropCount; i++)
        {
            SpawnSingleDrop();
        }
    }

    private void SpawnSingleDrop()
    {
        int randomIndex = Random.Range(0, dropPrefabs.Length);
        GameObject prefab = dropPrefabs[randomIndex];

        if (prefab == null) return;

        GameObject go = ObjectPool.instance.Spawn(prefab.name, transform.position, Quaternion.identity);
        if (go == null) return;

        if (go.TryGetComponent(out SkillObject_Soul soul))
        {
            SetupSoulDrop(soul);
        }
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
            player.skillManager.absorbSoul.speedOfSoul,
            playerTransform
        );
    }
}